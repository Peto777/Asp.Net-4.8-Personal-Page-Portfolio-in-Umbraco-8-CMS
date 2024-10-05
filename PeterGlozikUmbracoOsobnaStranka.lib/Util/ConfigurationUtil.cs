using System.Configuration;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Util
{
    public class ConfigurationUtil
    {
        public const string LoginFormId = "osobnaStranka.LoginFormId";
        public const string AfterLoginFormId = "osobnaStranka.AfterLoginFormId";
        public const string AfterPasswordResetFormId = "osobnaStranka.AfterPasswordResetFormId";
        public const string RegistrationOkFormId = "osobnaStranka.RegistrationOkFormId";
        public const string ChangePasswordFormId = "osobnaStranka.ChangePasswordFormId";

        public const string PropId_BlogFilterModel = "PropId_BlogFilterModel";
        public const string OsobnaStrankaBlogsFormId = "osobnaStranka.BlogFormId";
        public const string OsobnaStranka_BlogPublic_DetailPageId = "osobnaStranka.Blog.ProductPublic_DetailPageId";
        public const string OsobnaStranka_BlogPublic_CategoryPageId = "osobnaStranka.Blog.ProductPublic_CategoryPageId";
        public const string PropId_ProducerFilterModel = "PropId_ProducerFilterModel";
        public const string PropId_CustomerFilterModel = "PropId_CustomerFilterModel";
        public const string DashboardProducersFormId = "osobnaStranka.Dashboard.ProducersFormId";
        public const string DashboardBlogCountriesFormId = "osobnaStranka.DashboardBlog.CountriesFormId";
        public static int GetPageId(string pageKey)
        {
            return int.Parse(ConfigurationManager.AppSettings[pageKey]);
        }

        public static string GetCfgValue(string cfgKey)
        {
            return ConfigurationManager.AppSettings[cfgKey];
        }
    }
}
