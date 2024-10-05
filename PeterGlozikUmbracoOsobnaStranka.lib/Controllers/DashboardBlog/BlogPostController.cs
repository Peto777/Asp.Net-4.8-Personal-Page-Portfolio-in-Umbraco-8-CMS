using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using PeterGlozikUmbracoOsobnaStranka.lib.Models.DashboardBlog;
using System;
using PeterGlozikUmbracoOsobnaStranka.lib.Controllers;
using PeterGlozikUmbracoOsobnaStranka.lib.Models;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System.Net;
using PeterGlozikUmbracoOsobnaStranka.lib.Repositories;
using System.IO;
using System.Web;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Controllers.DashboardBlog
{
    [PluginController("DashboardBlog")]
    public class BlogPostController : _BaseController
    {
        [Authorize(Roles = "Admin")]
        public ActionResult GetRecords(int page = 1, string sort = "BlogName", string sortDir = "ASC")
        {
            try
            {
                return GetRecordsView(page, sort, sortDir);
            }
            catch
            {
                BlogFilterModel filter = GetOsobnaStrankaBlogFilterForEdit();
                if (filter != null)
                {
                    filter.SearchText = string.Empty;
                    OsobnaStrankaUserPropRepository repository = new OsobnaStrankaUserPropRepository();
                    repository.Save(this.CurrentSessionId, BlogFilterModel.CreateCopyFrom(filter));
                }
                return GetRecordsView(page, sort, sortDir);
            }
        }
        ActionResult GetRecordsView(int page, string sort, string sortDir)
        {
            BlogFilterModel filter = GetOsobnaStrankaBlogFilterForEdit();

            BlogPostRepository repository = new BlogPostRepository();
            BlogPagingListModel model = BlogPagingListModel.CreateCopyFrom(
                 repository.GetPage(page, _PagingModel.DefaultItemsPerPage, sort, sortDir,
                    new OsobnaStrankaBlogPostFilter()
                    {
                        SearchText = filter.SearchText
                    })
                );

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult InsertRecord()
        {
            return View("EditRecord", new MemberBlogModel());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditRecord(string id)
        {
            MemberBlogModel model = MemberBlogModel.CreateCopyFrom(new BlogPostRepository().Get(new Guid(id)));

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRecord(MemberBlogModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (model.BlogImage != null && model.BlogImage.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.BlogImage.FileName);
                var path = Path.Combine(Server.MapPath("~/Styles/images/"), fileName);
                model.BlogImage.SaveAs(path);

                model.BlogImagePath = Url.Content("~/Styles/images/" + fileName);
            }

            if (!new BlogPostRepository().Save(MemberBlogModel.CreateCopyFrom(model)))
            {
                ModelState.AddModelError("", "Nastala chyba pri zápise záznamu do systému. Skúste akciu zopakovať a ak sa chyba vyskytne znovu, kontaktujte nás prosím.");
            }

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.OsobnaStrankaBlogsFormId);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmDeleteRecord(string id)
        {
            MemberBlogModel model = MemberBlogModel.CreateCopyFrom(new BlogPostRepository().Get(new Guid(id)));

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(MemberBlogModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            BlogPostRepository repository = new BlogPostRepository();
            try
            {
                if (!repository.Delete(MemberBlogModel.CreateCopyFrom(model)))
                {
                    ModelState.AddModelError("", "Nastala chyba pri zápise záznamu do systému. Skúste akciu zopakovať a ak sa chyba vyskytne znovu, kontaktujte nás prosím.");
                }
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", "Výrobcu nie je možné odstrániť pretože je priradený k niektorým produktom.");
                this.Logger.Error(typeof(BlogPostController), "DeleteRecord error", exc);
            }
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.OsobnaStrankaBlogsFormId);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult GetFilter()
        {
            return View(GetOsobnaStrankaBlogFilterForEdit());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFilter(BlogFilterModel model)
        {
            model.ModelErrors.Clear();
            if (model.ModelErrors.Count == 0)
            {
                OsobnaStrankaUserPropRepository repository = new OsobnaStrankaUserPropRepository();
                if (!repository.Save(this.CurrentSessionId, BlogFilterModel.CreateCopyFrom(model)))
                {
                    model.ModelErrors.Add("Nastala chyba pri zápise záznamu do systému. Skúste akciu zopakovať a ak sa chyba vyskytne znovu, kontaktujte nás prosím.");
                }
            }
            if (model.ModelErrors.Count > 0)
            {
                return RedirectToCurrentUmbracoPageAfterSaveRecordFilter(model);
            }

            return RedirectToCurrentUmbracoPageAfterSaveRecordFilter();
        }
        RedirectToUmbracoPageResult RedirectToCurrentUmbracoPageAfterSaveRecordFilter(BlogFilterModel rec = null)
        {
            SetOsobnaStrankaBlogFilterForEdit(rec);
            return RedirectToCurrentUmbracoPage();
        }
        void SetOsobnaStrankaBlogFilterForEdit(BlogFilterModel rec = null)
        {
            TempData["stirilabBlogFilterForEdit"] = rec;
        }
        BlogFilterModel GetOsobnaStrankaBlogFilterForEdit()
        {
            if (TempData["stirilabBlogFilterForEdit"] == null)
            {
                OsobnaStrankaUserPropRepository repository = new OsobnaStrankaUserPropRepository();
                TempData["stirilabBlogFilterForEdit"] = BlogFilterModel.CreateCopyFrom(repository.Get(this.CurrentSessionId, ConfigurationUtil.PropId_BlogFilterModel));
            }

            return (BlogFilterModel)TempData["stirilabBlogFilterForEdit"];
        }
    }
}