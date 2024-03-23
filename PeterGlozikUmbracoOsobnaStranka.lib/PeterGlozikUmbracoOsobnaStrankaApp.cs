using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using Umbraco.Web;

namespace PeterGlozikUmbracoOsobnaStranka.lib
{
    public class PeterGlozikUmbracoOsobnaStrankaApp : UmbracoApplication
    {
        public override void Init()
        {
            base.Init();
            TranslateUtil.RegisterTranslations();
        }
    }
}