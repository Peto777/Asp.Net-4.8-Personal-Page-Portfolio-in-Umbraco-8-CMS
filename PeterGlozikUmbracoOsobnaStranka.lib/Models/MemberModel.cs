using dufeksoft.lib.Model;
using PeterGlozikUmbracoOsobnaStranka.lib.Repositories;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System.ComponentModel.DataAnnotations;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Models
{
    public class LoginModel : _BaseModel
    {

        [DisplayCurrentLang("Models/LoginModel", "Prihlasovacie meno")]
        [RequiredCurrentLang("Models/LoginModel", "Prihlasovacie meno musí byť zadané")]
        public string Username { get; set; }

        [DisplayCurrentLang("Models/LoginModel", "Heslo")]
        [RequiredCurrentLang("Models/LoginModel", "Heslo musí byť zadané")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class LostPasswordModel : _BaseModel
    {
        [DisplayCurrentLang("Models/LoginModel", "Váš e-mail")]
        [EmailCurrentLang("Models/LoginModel", "Neplatný formát e-mailu")]
        [RequiredCurrentLang("Models/LoginModel", "E-mail musí byť zadaný")]
        public string Email { get; set; }
    }

    public class ChangePasswordModel : _BaseModel
    {
        [DisplayCurrentLang("Models/ChangePasswordModel", "Staré heslo")]
        [RequiredCurrentLang("Models/ChangePasswordModel", "Staré heslo musí byť zadané")]
        public string OldPassword { get; set; }
        [DisplayCurrentLang("Models/ChangePasswordModel", "Nové heslo")]
        [RequiredCurrentLang("Models/ChangePasswordModel", "Nové heslo musí byť zadané")]
        public string NewPassword { get; set; }
        [DisplayCurrentLang("Models/ChangePasswordModel", "Zopakované nové heslo")]
        [RequiredCurrentLang("Models/ChangePasswordModel", "Zopakované nové heslo musí byť zadané")]
        public string NewPasswordRepeat { get; set; }
    }
    public class RegisterModel
    {
        [DisplayCurrentLang("Models/RegisterModel", "Prihlasovacie meno")]
        [RequiredCurrentLang("Models/RegisterModel", "Prihlasovacie meno musí byť zadané")]
        public string Name { get; set; }
        [DisplayCurrentLang("Models/RegisterModel", "E-mail")]
        [RequiredCurrentLang("Models/RegisterModel", "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadajte platnú emailovú adresu.")]
        public string Email { get; set; }

        [DisplayCurrentLang("Models/RegisterModel", "Heslo")]
        [RequiredCurrentLang("Models/RegisterModel", "Heslo je povinné.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayCurrentLang("Models/RegisterModel", "Opakujte heslo")]
        [RequiredCurrentLang("Models/RegisterModel", "Potvrdenie hesla je povinné.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Heslá sa nezhodujú.")]
        public string PasswordRepeat { get; set; }
        public string ConfirmPassword { get; set; }

    }

    public class OsobnaStrankaMemberModel : _BaseModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "Id musí byť zadané")]
        public int MemberId { get; set; }
        [Email(ErrorMessage = "Neplatný tvar e-mailovej adresy")]
        [Display(Name = "Užívateľské meno (e-mail)")]
        [Required(ErrorMessage = "Užívateľské meno (e-mail) musí byť zadané")]
        public string Name { get; set; }

        [Display(Name = "Povolený")]
        public bool IsApproved { get; set; }
        [Display(Name = "Uzamknutý")]
        public bool IsLockedOut { get; set; }


        [Display(Name = "Administrátor")]
        public bool IsAdminUser { get; set; }
        [Display(Name = "Zákazník")]
        public bool IsCustomerUser { get; set; }

        [Display(Name = "Heslo")]
        public string Password { get; set; }
        [Display(Name = "Zopakované heslo")]
        public string PasswordRepeat { get; set; }

        public string IsCustomerEdit { get; set; }

        public override bool IsNew
        {
            get
            {
                return this.MemberId <= 0;
            }
        }

        public void CopyDataFrom(OsobnaStrankaMember src)
        {
            this.MemberId = src.MemberId;
            this.Name = src.Name;
            this.IsApproved = src.IsApproved;
            this.IsLockedOut = src.IsLockedOut;
            this.IsAdminUser = src.IsAdminUser;
            this.IsCustomerUser = src.IsCustomerUser;
            this.Password = src.Password;
            this.PasswordRepeat = src.PasswordRepeat;
        }

        public void CopyDataTo(OsobnaStrankaMember trg)
        {
            trg.MemberId = this.MemberId;
            trg.Name = this.Name;
            trg.Username = this.Name;
            trg.Email = this.Name;
            trg.IsApproved = this.IsApproved;
            trg.IsLockedOut = this.IsLockedOut;
            trg.IsAdminUser = this.IsAdminUser;
            trg.IsCustomerUser = this.IsCustomerUser;
            trg.Password = this.Password;
            trg.PasswordRepeat = this.PasswordRepeat;
        }

        public static OsobnaStrankaMemberModel CreateCopyFrom(OsobnaStrankaMember src)
        {
            OsobnaStrankaMemberModel trg = new OsobnaStrankaMemberModel();
            trg.CopyDataFrom(src);

            return trg;
        }

        public static OsobnaStrankaMember CreateCopyFrom(OsobnaStrankaMemberModel src)
        {
            OsobnaStrankaMember trg = new OsobnaStrankaMember();
            src.CopyDataTo(trg);

            return trg;
        }

        public static OsobnaStrankaMember CreateCopyFrom(RegisterModel src)
        {
            OsobnaStrankaMember trg = new OsobnaStrankaMember();
            trg.MemberId = 0;
            trg.Name = src.Email;
            trg.Username = src.Email;
            trg.Email = src.Email;
            trg.IsApproved = true;
            trg.IsLockedOut = false;
            trg.IsAdminUser = false;
            trg.IsCustomerUser = true;

            trg.Password = src.Password;
            trg.PasswordRepeat = src.PasswordRepeat;

            return trg;
        }
    }
}