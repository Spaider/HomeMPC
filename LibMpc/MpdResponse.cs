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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Libmpc
{
  /// <summary>
  /// The MpdResponse class parses the response to an MPD command in it's most
  /// basic structure.
  /// </summary>
  public class MpdResponse : IEnumerable<KeyValuePair<string, string>>
  {
    private const string OK = "OK";
    private const string ACK = "ACK";

    private static readonly Regex LINE_REGEX = new Regex("^(?<key>[A-Za-z]*):[ ]{0,1}(?<value>.*)$");

    private readonly int commandListNum;
    private readonly string currentCommand;
    private readonly int errorCode;
    private readonly string errorMessage;
    private readonly bool isError;
    private readonly ReadOnlyCollection<string> message;

    private Dictionary<string, string> dictionary;

    /// <summary>
    /// Creates a new MpdResponse from a list of lines in case no error occured.
    /// </summary>
    /// <param name="message">The response to an MPD command.</param>
    public MpdResponse(ReadOnlyCollection<string> message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      isError = false;
      errorCode = -1;
      commandListNum = 0;
      currentCommand = null;
      errorMessage = null;
      this.message = message;
    }

    /// <summary>
    /// Creates a new MpdResponse in case an error occured.
    /// </summary>
    /// <param name="errorCode">The code of the error.</param>
    /// <param name="commandListNum">The index of the command which raised the error.</param>
    /// <param name="currentCommand">The command that raised the error.</param>
    /// <param name="errorMessage">The message describing the error.</param>
    /// <param name="message">The text of the standard MPD response.</param>
    public MpdResponse(int errorCode, int commandListNum, string currentCommand, string errorMessage,
                       ReadOnlyCollection<string> message)
    {
      if (currentCommand == null)
        throw new ArgumentNullException("currentCommand");
      if (errorMessage == null)
        throw new ArgumentNullException("errorMessage");
      if (message == null)
        throw new ArgumentNullException("message");

      isError = true;
      this.errorCode = errorCode;
      this.commandListNum = commandListNum;
      this.currentCommand = currentCommand;
      this.errorMessage = errorMessage;
      this.message = message;
    }

    /// <summary>
    /// If the response denotes an error in the last command.
    /// </summary>
    public bool IsError
    {
      get { return isError; }
    }

    /// <summary>
    /// The error code if an error occured.
    /// </summary>
    public int ErrorCode
    {
      get { return errorCode; }
    }

    /// <summary>
    /// If an error occured the index of the invalid command in a command list.
    /// </summary>
    public int CommandListNum
    {
      get { return commandListNum; }
    }

    /// <summary>
    /// The command executed.
    /// </summary>
    public string CurrentCommand
    {
      get { return currentCommand; }
    }

    /// <summary>
    /// The description of the error, if occured.
    /// </summary>
    public string ErrorMessage
    {
      get { return errorMessage; }
    }

    /// <summary>
    /// The lines of the response message.
    /// </summary>
    public ReadOnlyCollection<string> Message
    {
      get { return message; }
    }

    /// <summary>
    /// The value of an attribute in the MPD response.
    /// </summary>
    /// <param name="key">The name of the attribute.</param>
    /// <returns>The value of the attribute</returns>
    public string this[string key]
    {
      get
      {
        if (dictionary == null)
        {
          dictionary = new Dictionary<string, string>();

          foreach (string line in message)
          {
            Match match = LINE_REGEX.Match(line);
            if (match.Success)
              dictionary[match.Result("$key")] = match.Result("$value");
          }
        }

        return dictionary[key];
      }
    }

    /// <summary>
    /// The number of lines in the response message.
    /// </summary>
    public int Count
    {
      get { return message.Count; }
    }

    /// <summary>
    /// A line in the MPD response as KeyValuePair. If the message cannot be separated
    /// into key and value according to the MPD protocol spec, a KeyValuePair is returned
    /// with the key null and the value the whole text of the line.
    /// </summary>
    /// <param name="line">The index of the line.</param>
    /// <returns>The requested line as KeyValuePair.</returns>
    public KeyValuePair<string, string> this[int line]
    {
      get
      {
        Match match = LINE_REGEX.Match(message[line]);
        return match.Success 
          ? new KeyValuePair<string, string>(match.Result("${key}"), match.Result("${value}")) 
          : new KeyValuePair<string, string>(null, message[line]);
      }
    }

    #region IEnumerable<KeyValuePair<string,string>> Members

    /// <summary>
    /// Returns an enumerator for all KeyValuePairs in the MPD response.
    /// </summary>
    /// <returns>An enumerator for all KeyValuePairs in the MPD response.</returns>
    IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
    {
      return new MpdResponseEnumerator(this);
    }

    /// <summary>
    /// Returns an enumerator for all KeyValuePairs in the MPD response.
    /// </summary>
    /// <returns>An enumerator for all KeyValuePairs in the MPD response.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return new MpdResponseEnumerator(this);
    }

    #endregion

    /// <summary>
    /// Returns the values in all lines with the given attribute.
    /// </summary>
    /// <param name="attribute">The attribute who's values are reguested.</param>
    /// <returns>The values in all lines with the given attribute.</returns>
    public List<string> getAttributeValueList(string attribute)
    {
      var ret = new List<string>();

      foreach (string line in message)
      {
        Match match = LINE_REGEX.Match(line);
        if (match.Success)
        {
          string key = match.Result("${key}");
          if (key.Equals(attribute))
          {
            string value = match.Result("${value}");
            ret.Add(value);
          }
        }
      }

      return ret;
    }

    /// <summary>
    /// Returns only the value parts in all key/value pairs in the response.
    /// </summary>
    /// <returns>The list of values in all key/value pairs in the response.</returns>
    public List<string> getValueList()
    {
      var ret = new List<string>();

      foreach (string line in message)
      {
        Match match = LINE_REGEX.Match(line);
        if (match.Success)
        {
          string value = match.Result("${value}");
          ret.Add(value);
        }
      }

      return ret;
    }

    /// <summary>
    /// Builds the response text of the MPD server from the object.
    /// </summary>
    /// <returns>The response text of the MPD server from the object.</returns>
    public override string ToString()
    {
      var builder = new StringBuilder();
      foreach (string line in message)
        builder.AppendLine(line);

      if (isError)
      {
        builder.Append(ACK);
        builder.Append(" [");
        builder.Append(errorMessage);
        builder.Append('@');
        builder.Append(commandListNum);
        builder.Append("] {");
        builder.Append(currentCommand);
        builder.Append("} ");
        builder.Append(errorMessage);
        //ACK [50@1] {play} song doesn't exist: "10240"
      }
      else
        builder.Append(OK);

      return builder.ToString();
    }
  }

  /// <summary>
  /// A class for enumerating over the KeyValuePairs in the response.
  /// </summary>
  public class MpdResponseEnumerator : IEnumerator<KeyValuePair<string, string>>
  {
    private readonly MpdResponse response;
    private KeyValuePair<string, string> current;
    private int position = -1;

    /// <summary>
    /// Creates a new MpdResponseEnumerator.
    /// </summary>
    /// <param name="response">The response to enumerate over.</param>
    protected internal MpdResponseEnumerator(MpdResponse response)
    {
      this.response = response;
    }

    #region IEnumerator<KeyValuePair<string,string>> Members

    /// <summary>
    /// Returns the current element of the enumerator.
    /// </summary>
    KeyValuePair<string, string> IEnumerator<KeyValuePair<string, string>>.Current
    {
      get { return current; }
    }

    void IDisposable.Dispose()
    {
      position = -1;
    }

    /// <summary>
    /// Returns the current element of the enumerator.
    /// </summary>
    object IEnumerator.Current
    {
      get { return current; }
    }

    /// <summary>
    /// Moves the enumerator to the next KeyValuePair in the MPD response.
    /// </summary>
    /// <returns>If the enumerator has any values left.</returns>
    bool IEnumerator.MoveNext()
    {
      position++;
      if (position < response.Count)
      {
        current = response[position];
        return true;
      }
      return false;
    }

    /// <summary>
    /// Sets the enumerator to it's initial state.
    /// </summary>
    void IEnumerator.Reset()
    {
      position = -1;
    }

    #endregion
  }
}