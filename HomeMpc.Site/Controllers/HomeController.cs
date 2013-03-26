using System.Net;
using System.Web.Mvc;
using Libmpc;

namespace HomeMpc.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
//      var mpc = new Mpc
//        {
//          Connection = new MpcConnection(new IPEndPoint(new IPAddress(new byte[] {127, 0, 0, 1}), 6600))
//        };
      return View();
    }
  }
}
