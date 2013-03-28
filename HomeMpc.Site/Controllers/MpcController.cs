using System.Web.Mvc;
using HomeMpc.Repositories;

namespace HomeMpc.Controllers
{
  public class MpcController : Controller
  {
    private readonly MpcRepository _repository = new MpcRepository();

    public ActionResult PlayList()
    {
      return Json(_repository.GetCurrentPlayList(), JsonRequestBehavior.AllowGet);
    }

    public ActionResult Files(string path)
    {           
      return Json(_repository.GetDirectoryListing(path), JsonRequestBehavior.AllowGet);
    }
  }
}