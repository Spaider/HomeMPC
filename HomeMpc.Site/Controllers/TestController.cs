using System.Text;
using System.Web.Mvc;

namespace HomeMpc.Controllers
{
  public class TestController : Controller
  {
    public ActionResult Index()
    {
      var sb = new StringBuilder();
      foreach (var eng in ViewEngines.Engines)
      {
        sb.AppendLine(eng.GetType().FullName);
      }
      return Content(sb.ToString());
    }
  }
}