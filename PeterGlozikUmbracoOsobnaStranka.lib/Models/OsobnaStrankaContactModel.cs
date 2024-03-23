using dufeksoft.lib.Mail;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Models
{
    public class OsobnaStrankaContactModel
    {
        [DisplayCurrentLang("Models/OsobnaStrankaContactModel", "Priezvisko a meno")]
        [RequiredCurrentLang("Models/OsobnaStrankaContactModel", "Priezvisko a meno musí byť zadané")]
        public string Name { get; set; }
        [DisplayCurrentLang("Models/OsobnaStrankaContactModel", "E-mail")]
        [EmailCurrentLang("Models/OsobnaStrankaContactModel", "Neplatný email")]
        [RequiredCurrentLang("Models/OsobnaStrankaContactModel", "E-mail musí byť zadaný")]
        public string Email { get; set; }
        [DisplayCurrentLang("Models/OsobnaStrankaContactModel", "Telefón")]
        public string Telefon { get; set; }
        [DisplayCurrentLang("Models/OsobnaStrankaContactModel", "Text správy")]
        [RequiredCurrentLang("Models/OsobnaStrankaContactModel", "Text správy musí byť zadaný")]
        public string Text { get; set; }

        [Display(Name = "Heslo")]
        public string Password { get; set; }
        [Display(Name = "Potvrdenie hesla")]
        public string ConfirmPassword { get; set; }

        public bool SendContactRequest()
        {
            List<TextTemplateParam> paramList = new List<TextTemplateParam>();
            paramList.Add(new TextTemplateParam("NAME", this.Name));
            paramList.Add(new TextTemplateParam("EMAIL", this.Email));
            paramList.Add(new TextTemplateParam("TELEFON", this.Telefon));
            paramList.Add(new TextTemplateParam("TEXT", this.Text));

            // Odoslanie uzivatelovi
            OsobnaStrankaMailer.SendMailTemplate(
                "Vaša správa",
                TextTemplate.GetTemplateText("ContactSendSuccess_Sk", paramList),
                this.Email, null);

            return true;
        }
    }
}
