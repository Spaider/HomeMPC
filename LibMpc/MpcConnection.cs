/*
 * Copyright 2008 Matthias Sessler
 * 
 * This file is part of LibMpc.net.
 *
 * LibMpc.net is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 2.1 of the License, or
 * (at your option) any later version.
 *
 * LibMpc.net is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with LibMpc.net.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Libmpc
{
  /// <summary>
  /// The delegate for the <see cref="MpcConnection.OnConnected"/> and <see cref="MpcConnection.OnDisconnected"/> events.
  /// </summary>
  /// <param name="connection">The connection firing the event.</param>
  public delegate void MpcConnectionEventDelegate(MpcConnection connection);

  /// <summary>
  /// Keeps the connection to the MPD server and handels the most basic structure of the
  /// MPD protocol. The high level commands are handeled in the <see cref="Libmpc.Mpc"/>
  /// class.
  /// </summary>
  public class MpcConnection
  {
    private const string FIRST_LINE_PREFIX = "OK MPD ";

    private const string OK = "OK";
    private const string ACK = "ACK";

    private static readonly Regex ACK_REGEX =
      new Regex("^ACK \\[(?<code>[0-9]*)@(?<nr>[0-9]*)] \\{(?<command>[a-z]*)} (?<message>.*)$");

    private bool autoConnect;

    private IPEndPoint ipEndPoint;

    private NetworkStream networkStream;

    private StreamReader reader;
    private TcpClient tcpClient;

    private StreamWriter writer;

    /// <summary>
    /// Creates a new MpdConnection.
    /// </summary>
    public MpcConnection()
    {
    }

    /// <summary>
    /// Creates a new MpdConnection.
    /// </summary>
    /// <param name="server">The IPEndPoint of the MPD server.</param>
    public MpcConnection(IPEndPoint server)
    {
      Connect(server);
    }

    /// <summary>
    /// If the connection to the MPD is connected.
    /// </summary>
    public bool Connected
    {
      get { return (tcpClient != null) && tcpClient.Connected; }
    }

    /// <summary>
    /// The version of the MPD.
    /// </summary>
    public string Version { get; private set; }

    /// <summary>
    /// If a connection should be established when a command is to be
    /// executed in disconnected state.
    /// </summary>
    public bool AutoConnect
    {
      get { return autoConnect; }
      set { autoConnect = value; }
    }

    /// <summary>
    /// The IPEndPoint of the MPD server.
    /// </summary>
    /// <exception cref="AlreadyConnectedException">When a conenction to a MPD server is already established.</exception>
    public IPEndPoint Server
    {
      get { return ipEndPoint; }
      set
      {
        if (Connected)
          throw new AlreadyConnectedException();

        ipEndPoint = value;

        ClearConnectionFields();
      }
    }

    /// <summary>
    /// Is fired when a connection to a MPD server is established.
    /// </summary>
    public event MpcConnectionEventDelegate OnConnected;

    /// <summary>
    /// Is fired when the connection to the MPD server is closed.
    /// </summary>
    public event MpcConnectionEventDelegate OnDisconnected;

    /// <summary>
    /// Connects to a MPD server.
    /// </summary>
    /// <param name="server">The IPEndPoint of the server.</param>
    public void Connect(IPEndPoint server)
    {
      Server = server;
      Connect();
    }

    /// <summary>
    /// Connects to the MPD server who's IPEndPoint was set in the Server property.
    /// </summary>
    /// <exception cref="InvalidOperationException">If no IPEndPoint was set to the Server property.</exception>
    public void Connect()
    {
      if (ipEndPoint == null)
        throw new InvalidOperationException("Server IPEndPoint not set.");

      if (Connected)
        throw new AlreadyConnectedException();

      tcpClient = new TcpClient(
        ipEndPoint.Address.ToString(),
        ipEndPoint.Port);
      networkStream = tcpClient.GetStream();

      reader = new StreamReader(networkStream, Encoding.UTF8);
      writer = new StreamWriter(networkStream, Encoding.UTF8) {NewLine = "\n"};

      string firstLine = reader.ReadLine();
      if (string.IsNullOrEmpty(firstLine))
      {
        throw new InvalidDataException("Response of mpd does not start with \"" + FIRST_LINE_PREFIX + "\".");        
      }
      if (!firstLine.StartsWith(FIRST_LINE_PREFIX))
      {
        Disconnect();
        throw new InvalidDataException("Response of mpd does not start with \"" + FIRST_LINE_PREFIX + "\".");
      }
      Version = firstLine.Substring(FIRST_LINE_PREFIX.Length);

      writer.WriteLine();
      writer.Flush();

      readResponse();

      if (OnConnected != null)
        OnConnected.Invoke(this);
    }

    /// <summary>
    /// Disconnects from the current MPD server.
    /// </summary>
    public void Disconnect()
    {
      if (tcpClient == null)
        return;

      networkStream.Close();

      ClearConnectionFields();

      if (OnDisconnected != null)
        OnDisconnected.Invoke(this);
    }

    /// <summary>
    /// Executes a simple command without arguments on the MPD server and returns the response.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>The MPD server response parsed into a basic object.</returns>
    /// <exception cref="ArgumentException">If the command contains a space of a newline charakter.</exception>
    public MpdResponse Exec(string command)
    {
      if (command == null)
        throw new ArgumentNullException("command");
      if (command.Contains(" "))
        throw new ArgumentException("command contains space");
      if (command.Contains("\n"))
        throw new ArgumentException("command contains newline");

      CheckConnected();

      try
      {
        writer.WriteLine(command);
        writer.Flush();

        return readResponse();
      }
      catch (Exception)
      {
        try
        {
          Disconnect();
        }
// ReSharper disable EmptyGeneralCatchClause
        catch
// ReSharper restore EmptyGeneralCatchClause
        {
        }
        throw;
      }
    }

    /// <summary>
    /// Executes a MPD command with arguments on the MPD server.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="argument">The arguments of the command.</param>
    /// <returns>The MPD server response parsed into a basic object.</returns>
    /// <exception cref="ArgumentException">If the command contains a space of a newline charakter.</exception>
    public MpdResponse Exec(string command, string[] argument)
    {
      if (command == null)
        throw new ArgumentNullException("command");
      if (command.Contains(" "))
        throw new ArgumentException("command contains space");
      if (command.Contains("\n"))
        throw new ArgumentException("command contains newline");

      if (argument == null)
        throw new ArgumentNullException("argument");
      for (int i = 0; i < argument.Length; i++)
      {
        if (argument[i] == null)
          throw new ArgumentNullException("argument[" + i + "]");
        if (argument[i].Contains("\n"))
          throw new ArgumentException("argument[" + i + "] contains newline");
      }

      CheckConnected();

      try
      {
        writer.Write(command);
        foreach (string arg in argument)
        {
          writer.Write(' ');
          WriteToken(arg);
        }
        writer.WriteLine();
        writer.Flush();

        return readResponse();
      }
      catch (Exception)
      {
        try
        {
          Disconnect();
        }
// ReSharper disable EmptyGeneralCatchClause
        catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
        {
        }
        throw;
      }
    }

    private void CheckConnected()
    {
      if (Connected) return;

      if (autoConnect)
        Connect();
      else
        throw new NotConnectedException();
    }

    private void WriteToken(string token)
    {
      if (token.Contains(" "))
      {
        writer.Write("\"");
        foreach (char chr in token)
          if (chr == '"')
            writer.Write("\\\"");
          else
            writer.Write(chr);
        writer.Write("\"");
      }
      else
        writer.Write(token);
    }

    private MpdResponse readResponse()
    {
      var ret = new List<string>();
      string line = reader.ReadLine();
      if (line == null)
      {
        throw new InvalidDataException("Unable to read response from mpd");        
      }
      while (!(line.Equals(OK) || line.StartsWith(ACK)))
      {
        ret.Add(line);
        line = reader.ReadLine();
      }
      if (line.Equals(OK))
        return new MpdResponse(new ReadOnlyCollection<string>(ret));
      Match match = ACK_REGEX.Match(line);

      if (match.Groups.Count != 5)
        throw new InvalidDataException("Error response not as expected");

      return new MpdResponse(
        int.Parse(match.Result("${code}")),
        int.Parse(match.Result("${nr}")),
        match.Result("${command}"),
        match.Result("${message}"),
        new ReadOnlyCollection<string>(ret)
        );
    }

    private void ClearConnectionFields()
    {
      tcpClient = null;
      networkStream = null;
      reader = null;
      writer = null;
      Version = null;
    }
  }
}