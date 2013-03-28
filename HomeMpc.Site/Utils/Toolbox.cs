namespace HomeMpc.Utils
{
  public static class Toolbox
  {
    internal static string GetFileNameFromPath(string fullPath)
    {
      var pos = fullPath.LastIndexOf('/');
      return pos == -1
               ? fullPath
               : fullPath.Substring(pos + 1);
    }
  }
}