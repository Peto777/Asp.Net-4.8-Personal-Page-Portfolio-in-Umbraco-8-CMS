using PeterGlozikUmbracoOsobnaStranka.lib.Util;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Models
{
    public class Kurzy
    {
        public string[] zoznamKurzov = new string[]
        {
            CurrentLang.GetText("Models/EducationModel", "Kurz HTML"),
            CurrentLang.GetText("Models/EducationModel", "Kurz CSS"),
            CurrentLang.GetText("Models/EducationModel", "Kurz MVC"),
            CurrentLang.GetText("Models/EducationModel", "Kurz Bootstrap"),
            CurrentLang.GetText("Models/EducationModel", "Kurz C#"),
            CurrentLang.GetText("Models/EducationModel", "Kurz Umbraco"),
            CurrentLang.GetText("Models/EducationModel", "Kurz SQL"),
            CurrentLang.GetText("Models/EducationModel", "Kurz JavaScript")
        };
    }
}
