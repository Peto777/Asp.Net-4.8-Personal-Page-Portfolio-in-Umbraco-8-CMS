using PeterGlozikUmbracoOsobnaStranka.lib.Models.UmbracoCmsContent;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Repositories.UmbracoCmsContent
{
    public class TextRepository : _BaseRepository
    {
        public const int TextyId_Sk = 1071;
        public const int TextyId_En = 1131;

        public static Text GetFromUmbraco(UmbracoHelper umbraco)
        {
            string cultureId = CurrentLang.GetCurrentCultureId();

            IPublishedContent content = umbraco.Content(cultureId == CurrentLang.CultureId_En ? TextyId_En : TextyId_Sk);

            return content == null ? null : new Text(content);
        }
    }
}
