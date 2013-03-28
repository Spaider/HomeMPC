using System;
using System.Configuration;
using System.Net;

namespace HomeMpc.Utils
{
  public static class Toolbox
  {
    public static string GetFileNameFromPath(string fullPath)
    {
      var pos = fullPath.LastIndexOf('/');
      return pos == -1
               ? fullPath
               : fullPath.Substring(pos + 1);
    }

    public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP = false)
    {
      var addresses = Dns.GetHostAddresses(hostName);
      if (addresses.Length == 0)
      {
        throw new ArgumentException(
            "Unable to retrieve address from specified host name.",
            "hostName"
        );
      }
      
      if (throwIfMoreThanOneIP && addresses.Length > 1)
      {
        throw new ArgumentException(
            "There is more that one IP address to the specified host.",
            "hostName"
        );
      }
      return new IPEndPoint(addresses[0], port); // Port gets validated here.
    }

    public static T GetAppSettings<T>(string key)
    {
      var settings = ConfigurationManager.AppSettings[key];
      return settings == null
               ? default(T)
               : (T) Convert.ChangeType(settings, typeof(T));
    }
  }
}