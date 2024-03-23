using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Xml;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Util
{
    public class TranslateUtil
    {
        public static TranslateLangCollection TranslateManager = new TranslateLangCollection();

        public string CultureId { get; private set; }
        public string AreaId { get; private set; }

        public string this[string text]
        {
            get
            {
                return TranslateText(text);
            }
            private set
            {
            }
        }

        public static void RegisterTranslations()
        {
            LoadFromDirectory(string.Format("{0}\\App_Data\\Lang", HttpRuntime.AppDomainAppPath.TrimEnd('\\')));
        }

        static void LoadFromDirectory(string dir)
        {
            TranslateManager.Clear();

            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles("*.xml");
            foreach (FileInfo file in files)
            {
                LoadFromFile(file.FullName);
            }
        }

        static void LoadFromFile(string fileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileName);

            foreach (XmlNode languageNode in document.ChildNodes)
            {
                if (languageNode.Name.ToLower() == "language")
                {
                    string alias = languageNode.Attributes["alias"].Value;
                    string intName = languageNode.Attributes["intName"].Value;
                    string localName = languageNode.Attributes["localName"].Value;
                    string culture = languageNode.Attributes["culture"].Value;

                    TranslateLang lang = TranslateManager.Add(new TranslateLang(alias, intName, localName, culture));

                    foreach (XmlNode areaNode in languageNode.ChildNodes)
                    {
                        if (areaNode.Name.ToLower() == "area")
                        {
                            TranslateArea area = lang.AddArea(areaNode.Attributes["alias"].Value);
                            foreach (XmlNode itemNode in areaNode.ChildNodes)
                            {
                                if (itemNode.Name.ToLower() == "key")
                                {
                                    area.AddItem(itemNode.Attributes["alias"].Value, itemNode.InnerXml);
                                }
                            }
                        }
                    }
                }
            }
        }

        public TranslateUtil(string aCultureId, string aAreaId)
        {
            this.CultureId = aCultureId;
            this.AreaId = aAreaId;
        }

        public string TranslateText(string textId)
        {
            return TranslateUtil.TranslateText(this.CultureId, this.AreaId, textId);
        }

        public static string TranslateText(string cultureId, string areaId, string textId)
        {
            string text = textId;

            TranslateLang lang = GetLangInstance(cultureId);
            if (lang != null)
            {
                TranslateArea area = lang.GetArea(areaId);
                if (area != null)
                {
                    string item = area.GetItem(textId);
                    if (item != null)
                    {
                        text = item;
                    }
                }
            }

            return text;
        }

        private static TranslateLang GetLangInstance(string cultureId)
        {
            return TranslateUtil.TranslateManager.GetForCulture(cultureId);
        }
    }

    public class TranslateLangCollection
    {
        Hashtable htAlias = new Hashtable();
        Hashtable htCulture = new Hashtable();

        public void Clear()
        {
            htAlias.Clear();
            htCulture.Clear();
        }

        public TranslateLang Add(TranslateLang lang)
        {
            if (!htAlias.ContainsKey(lang.Alias))
            {
                htAlias.Add(lang.Alias, lang);
                htCulture.Add(lang.Culture, lang);
            }

            return lang;
        }


        public TranslateLang GetForAlias(string alias)
        {
            return htAlias.ContainsKey(alias) ? (TranslateLang)htAlias[alias] : null;
        }

        public TranslateLang GetForCulture(string culture)
        {
            return htCulture.ContainsKey(culture) ? (TranslateLang)htCulture[culture] : null;
        }
    }

    public class TranslateLang
    {
        public string Alias { get; private set; }
        public string IntName { get; private set; }
        public string LocalName { get; private set; }
        public string Culture { get; private set; }

        Hashtable htAreas = new Hashtable();

        public TranslateLang(string alias, string intName, string localName, string culture)
        {
            this.Alias = alias;
            this.IntName = IntName;
            this.LocalName = localName;
            this.Culture = culture;
        }

        public TranslateArea AddArea(string alias)
        {
            TranslateArea area = new TranslateArea(alias);
            htAreas.Add(alias, area);

            return area;
        }

        public TranslateArea GetArea(string alias)
        {
            return htAreas.ContainsKey(alias) ? (TranslateArea)htAreas[alias] : null;
        }
    }

    public class TranslateArea
    {
        public string Alias { get; private set; }

        Hashtable htItems = new Hashtable();

        public TranslateArea(string alias)
        {
            this.Alias = alias;
        }

        public void AddItem(string alias, string text)
        {
            htItems.Add(alias, text);
        }

        public string GetItem(string alias)
        {
            return htItems.ContainsKey(alias) ? (string)htItems[alias] : null;
        }
    }

    public class CurrentLang
    {
        public const string CultureId_Sk = "sk";
        public const string CultureId_Cz = "cz";
        public const string CultureId_En = "en";

        public static string GetText(string areaId, string textId)
        {
            return new TranslateUtil(GetCurrentCulture(), areaId).TranslateText(textId);
        }
        //public static string GetCurrentCultureText(string textSk, string textCz)
        //{
        //    switch (GetCurrentCulture())
        //    {
        //        case "cs-CZ":
        //            return textCz;
        //        case "sk-SK":
        //            return textSk;
        //    }

        //    return null;
        //}
        //public static decimal GetCurrentCultureDecimal(decimal decimalSk, decimal decimalCz)
        //{
        //    switch (GetCurrentCulture())
        //    {
        //        case "cs-CZ":
        //            return decimalCz;
        //        case "sk-SK":
        //            return decimalSk;
        //    }

        //    return 0M;
        //}

        public static void SetCurrentCulture(string cultureId)
        {
            string currentCulture;
            switch (cultureId)
            {
                case CultureId_En:
                    currentCulture = "en-US";
                    break;
                case CultureId_Cz:
                case "cs":
                    currentCulture = "cs-CZ";
                    break;
                default:
                    currentCulture = "sk-SK";
                    break;
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentCulture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentCulture);
        }

        public static string GetCurrentCulture()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetCurrentCultureId()
        {
            switch (GetCurrentCulture())
            {
                case "en-US":
                    return CultureId_En;
                case "cs-CZ":
                    return CultureId_Cz;
                default:
                    return CultureId_Sk;
            }

        }
    }

    public class DisplayCurrentLangAttribute : DisplayNameAttribute
    {
        private readonly string m_AreaId;
        private readonly string m_TextId;
        public DisplayCurrentLangAttribute(string areaId, string textId)
        {
            m_AreaId = areaId;
            m_TextId = textId;
        }

        public override string DisplayName
        {
            get
            {
                // get and return the resource object
                return CurrentLang.GetText(m_AreaId, m_TextId);
            }
        }
    }

    public class RequiredCurrentLangAttribute : RequiredAttribute
    {
        private readonly string m_AreaId;
        private readonly string m_TextId;
        public RequiredCurrentLangAttribute(string areaId = null, string textId = null)
        {
            m_AreaId = areaId;
            m_TextId = textId;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(m_AreaId) || string.IsNullOrEmpty(m_AreaId))
            {
                return string.Format(CurrentLang.GetText("RequiredCurrentLangAttribute", "The {0} field is required."), name);
            }

            return CurrentLang.GetText(m_AreaId, m_TextId);
        }
    }

    public class EmailCurrentLangAttribute : RegularExpressionAttribute
    {
        private readonly string m_AreaId;
        private readonly string m_TextId;

        public EmailCurrentLangAttribute(string areaId = null, string textId = null)
            : base("^[ a-zA-Z0-9_\\+-]+(\\.[ a-zA-Z0-9_\\+-]+)*@[ a-zA-Z0-9-]+(\\.[ a-zA-Z0-9-]+)*\\.([ a-zA-Z]{2,10})$")
        {
            m_AreaId = areaId;
            m_TextId = textId;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(m_AreaId) || string.IsNullOrEmpty(m_AreaId))
            {
                return string.Format(CurrentLang.GetText("EmailCurrentLangAttribute", "The {0} field is not a valid e-mail address."), name);
            }

            return CurrentLang.GetText(m_AreaId, m_TextId);
        }
    }

    public class PhoneCurrentLangAttribute : RegularExpressionAttribute
    {
        private readonly string m_AreaId;
        private readonly string m_TextId;

        public PhoneCurrentLangAttribute(string areaId = null, string textId = null)
            : base("^[ a-zA-Z0-9_\\+-]+(\\.[ a-zA-Z0-9_\\+-]+)*@[ a-zA-Z0-9-]+(\\.[ a-zA-Z0-9-]+)*\\.([ a-zA-Z]{2,10})$")
        {
            m_AreaId = areaId;
            m_TextId = textId;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(m_AreaId) || string.IsNullOrEmpty(m_AreaId))
            {
                return string.Format(CurrentLang.GetText("PhoneCurrentLangAttribute", "The {0} field is not a valid e-mail address."), name);
            }

            return CurrentLang.GetText(m_AreaId, m_TextId);
        }
    }
    public class DecimalCurrentLangAttribute : RegularExpressionAttribute
    {
        public DecimalCurrentLangAttribute()
            : base("^[+-]?[0-9.,]*$")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CurrentLang.GetText("DecimalCurrentLangAttribute", "{0} must be a decimal number."), name);
        }
    }
    public class RequiredGuidDropDownCurrentLangAttribute : RequiredCurrentLangAttribute
    {
        public RequiredGuidDropDownCurrentLangAttribute(string areaId = null, string textId = null) : base(areaId, textId)
        {
        }

        public override bool IsValid(object value)
        {
            if (!base.IsValid(value))
            {
                return false;
            }

            return value.ToString() != Guid.Empty.ToString();
        }
    }
}
