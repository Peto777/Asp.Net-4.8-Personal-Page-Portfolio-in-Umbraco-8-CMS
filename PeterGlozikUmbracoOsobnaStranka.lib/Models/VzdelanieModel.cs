namespace PeterGlozikUmbracoOsobnaStranka.lib.Models
{
    public class VzdelanieModel
    {
        public string ZakladnaSkola;
        public string StrednaSkola;
        public string VysokaSkola;
        public Kurzy ZoznamKurzov;

        public VzdelanieModel()
        {
            ZoznamKurzov = new Kurzy();
        }
    }
}
