using PeterGlozikUmbracoOsobnaStranka.lib.Models.DashboardBlog;
using PeterGlozikUmbracoOsobnaStranka.lib.Models;
using PeterGlozikUmbracoOsobnaStranka.lib.Repositories;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Controllers.DashboardBlog
{
    [PluginController("DashboardBlog")]
    public class CountryController : _BaseController
    {
        [Authorize(Roles = "Admin")]
        public ActionResult GetRecords(int page = 1, string sort = "Code", string sortDir = "ASC")
        {
            CountryPagingListModel model = CountryPagingListModel.CreateCopyFrom(
                new CountryRepository().GetPage(page, _PagingModel.DefaultItemsPerPage, sort, sortDir));

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult InsertRecord()
        {
            return View("EditRecord", GetCountryForEdit(null));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditRecord(string id)
        {
            return View(GetCountryForEdit(id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRecord(CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (!new CountryRepository().Save(CountryModel.CreateCopyFrom(model)))
            {
                ModelState.AddModelError("", "Nastala chyba pri zápise záznamu do systému. Skúste akciu zopakovať a ak sa chyba vyskytne znovu, kontaktujte nás prosím.");
            }
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.DashboardBlogCountriesFormId);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmDeleteRecord(string id)
        {
            return View(GetCountryForEdit(id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (!new CountryRepository().Delete(CountryModel.CreateCopyFrom(model)))
            {
                ModelState.AddModelError("", "Nastala chyba pri zápise záznamu do systému. Skúste akciu zopakovať a ak sa chyba vyskytne znovu, kontaktujte nás prosím.");
            }

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.DashboardBlogCountriesFormId);
        }


        CountryModel GetCountryForEdit(string id)
        {
            return string.IsNullOrEmpty(id) ? new CountryModel() : CountryModel.CreateCopyFrom(new CountryRepository().Get(new Guid(id)));
        }
    }
}
