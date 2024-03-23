using Umbraco.Core.Services;
using Umbraco.Web.Composing;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Repositories.UmbracoCmsContent
{
    public class _BaseRepository
    {
        protected readonly IMemberService MemberService;

        public _BaseRepository()
        {
            MemberService = Current.Services.MemberService;
        }
    }
}
