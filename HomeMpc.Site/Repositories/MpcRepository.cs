using System.Collections.Generic;
using System.Linq;
using HomeMpc.Models;
using HomeMpc.Utils;
using Libmpc;

namespace HomeMpc.Repositories
{
  public class MpcRepository
  {
    private readonly MpcConnection _connection;

    public MpcRepository()
    {
      var mpcEndPoint = Toolbox.GetIPEndPointFromHostName(Toolbox.GetAppSettings<string>("MpcHost"), Toolbox.GetAppSettings<int>("MpcPort"));
      _connection = new MpcConnection(mpcEndPoint);
    }

    public IEnumerable<FileTreeModel> GetDirectoryListing(string path)
    {
      var mpc = new Mpc {Connection = _connection};
      try
      {
        var dirList = mpc.LsInfo(path);

        var models = 
          (from f in dirList.DirectoryList
           select new FileTreeModel
             {
               leaf = false,
               text = Toolbox.GetFileNameFromPath(f),
               id = f
             })
          .Concat(from f in dirList.FileList
                    select new FileTreeModel
                      {
                        leaf = true,
                        text = f.HasTitle 
                                  ? f.Title
                                  : f.HasName
                                    ? f.Name
                                    : Toolbox.GetFileNameFromPath(f.File),
                        id = f.File
                      });
        return models;
      }
      finally
      {
        mpc.Disconnect();
      }
    }

    public IEnumerable<MpdFile> GetCurrentPlayList()
    {
      var mpc = new Mpc {Connection = _connection};
      try
      {
        return mpc.PlaylistInfo();
      }
      finally
      {
        mpc.Disconnect();
      }
    }

    public MpdStatus GetStatus()
    {
      var mpc = new Mpc {Connection = _connection};
      try
      {
        return mpc.Status();
      }
      finally
      {
        mpc.Disconnect();
      }
    }

    public void Stop()
    {
      var mpc = new Mpc {Connection = _connection};
      try
      {
        mpc.Stop();
      }
      finally
      {
        mpc.Disconnect();
      }
    }    
    
    public void Play()
    {
      var mpc = new Mpc {Connection = _connection};
      try
      {
        mpc.Play();
      }
      finally
      {
        mpc.Disconnect();
      }
    }
  }
}