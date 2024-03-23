using PeterGlozikUmbracoOsobnaStranka.lib.Models;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Controllers
{
    [PluginController("OsobnaStranka")]
    public class ContactController : _BaseController
    {
        public ActionResult Index()
        {
            return View("ContactForm", new OsobnaStrankaContactModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleFormSubmit(OsobnaStrankaContactModel model)
        {
            if (ModelState.IsValid)
            {
                if (!new ApiKeyValidator().IsValid(model.Password, model.ConfirmPassword))
                {
                    ModelState.AddModelError("", CurrentLang.GetText("Controllers/ContactController", "Musíte označiť, že nie ste robot."));
                }
            }

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            TempData["success"] = model.SendContactRequest();

            return RedirectToCurrentUmbracoPage();
        }
    }
}
