using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace PeterGlozikUmbracoOsobnaStranka.lib
{
    public class PeterGlozikUmbracoOsobnaStrankaApp : UmbracoApplication
    {
        public override void Init()
        {
            base.Init();
            TranslateUtil.RegisterTranslations();
        }

        private new void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // Get current session.
            HttpSessionState objSession = ((UmbracoApplication)sender).Context.Session;

            // Make sure that there is an active session.
            if (objSession != null)
            {
                // Work with the session here.
                objSession["eshopzkurzuInit"] = 1;
            }
        }

        public class CategoryContentFinder : IContentFinder
        {
            public const string CategoryPath = "/eshop/kategoria/";
            public const string CategoryUrl_All = "-vsetko-";

            public bool TryFindContent(PublishedRequest contentRequest)
            {
                string path = contentRequest.Uri.AbsolutePath.ToLower();
                if (!IsCategoryPath(path))
                    return false; // not found

                var contentCache = contentRequest.UmbracoContext.Content;
                var content = contentCache.GetById(ConfigurationUtil.GetPageId(ConfigurationUtil.OsobnaStranka_BlogPublic_CategoryPageId));
                if (content == null) return false; // not found

                // render that node
                contentRequest.PublishedContent = content;

                return true;
            }

            public static bool IsCategoryPath(string path)
            {
                return path.StartsWith(CategoryContentFinder.CategoryPath);
            }

            public static string GetCategoryUrl(Uri uri)
            {
                int segmentsCnt = uri.Segments.Count();
                if (segmentsCnt < 4)
                {
                    return null;
                }

                StringBuilder str = new StringBuilder();
                for (int i = 3; i < segmentsCnt; i++)
                {
                    str.Append(string.Format("{0}/", uri.Segments[i].TrimEnd('/')));
                }
                return str.ToString().TrimEnd('/');
            }
        }

        public class ProductContentFinder : IContentFinder
        {
            public const string ProductPath = "/eshop/produkt/";

            public bool TryFindContent(PublishedRequest contentRequest)
            {
                string path = contentRequest.Uri.AbsolutePath.ToLower();
                if (!path.StartsWith(ProductContentFinder.ProductPath))
                    return false; // not found

                var contentCache = contentRequest.UmbracoContext.Content;
                var content = contentCache.GetById(ConfigurationUtil.GetPageId(ConfigurationUtil.OsobnaStranka_BlogPublic_DetailPageId));
                if (content == null) return false; // not found

                // render that node
                contentRequest.PublishedContent = content;

                return true;
            }

            public static string GetProductUrl(Uri uri)
            {
                if (uri.Segments.Count() < 4)
                {
                    return null;
                }

                return uri.Segments[3].TrimEnd('/');
            }
        }
    }
}