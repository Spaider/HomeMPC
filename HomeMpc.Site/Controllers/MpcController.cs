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

    public ActionResult Status()
    {
      return Json(_repository.GetStatus(), JsonRequestBehavior.AllowGet);
    }

    public ActionResult Play()
    {
      _repository.Play();
      return Json("OK", JsonRequestBehavior.AllowGet);
    }

    public ActionResult Pause()
    {
      _repository.Pause();
      return Json("OK", JsonRequestBehavior.AllowGet);
    }    
    
    public ActionResult Stop()
    {
      _repository.Stop();
      return Json("OK", JsonRequestBehavior.AllowGet);
    }

    public ActionResult PlayNext()
    {
      _repository.PlayNext();
      return Json("OK", JsonRequestBehavior.AllowGet);
    }    
    
    public ActionResult PlayPrevious()
    {
      _repository.PlayPrevious();
      return Json("OK", JsonRequestBehavior.AllowGet);
    }

    public ActionResult ClearPlaylist()
    {
      _repository.ClearPlaylist();
      return Json("OK", JsonRequestBehavior.AllowGet);
    }
  }
}