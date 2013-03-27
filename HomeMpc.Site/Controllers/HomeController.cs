using System.Linq;
using System.Net;
using System.Web.Mvc;
using HomeMpc.Models;
using Libmpc;

namespace HomeMpc.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {

      return View();
    }

    public ActionResult Files(string path)
    {      
      var mpc = new Mpc
      {
        Connection = new MpcConnection(new IPEndPoint(new IPAddress(new byte[] {127, 0, 0, 1}), 6600))
      };
      var dirList = mpc.LsInfo(path);
      var models = 
          (from f in dirList.DirectoryList
           select new FileTreeModel
             {
               leaf = false,
               text = GetFileNameFromPath(f),
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
                                    : GetFileNameFromPath(f.File),
                        id = f.File
                      });
      return Json(models, JsonRequestBehavior.AllowGet);
    }

    private static string GetFileNameFromPath(string fullPath)
    {
      var pos = fullPath.LastIndexOf('/');
      return pos == -1
               ? fullPath
               : fullPath.Substring(pos + 1);
    }
  }
}
