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
