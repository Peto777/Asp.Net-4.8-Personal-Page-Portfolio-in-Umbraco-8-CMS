using dufeksoft.lib.Mail;
using PeterGlozikUmbracoOsobnaStranka.lib.Models;
using PeterGlozikUmbracoOsobnaStranka.lib.Repositories;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Controllers
{
    [PluginController("OsobnaStranka")]
    public class MemberController : _BaseController
    {

        public ActionResult Login()
        {
            return View("Login", new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Members.Login(model.Username, model.Password))
                {
                    //FormsAuthentication.SetAuthCookie(model.Username, false);
                    UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);
                    if (myHelper.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.AfterLoginFormId);
                    }
                }
                else
                {
                    ModelState.AddModelError("", CurrentLang.GetText("Member/ContactController", "Neplatné meno alebo heslo."));
                }
            }
            return CurrentUmbracoPage();
        }

        public ActionResult Logout()
        {
            TempData.Clear();
            Session.Clear();
            Members.Logout();
            return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.LoginFormId);
        }

        public ActionResult MemberInfo()
        {
            return View();
        }

        public ActionResult LostPassword()
        {
            return View("LostPassword", new LostPasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitLostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                OsobnaStrankaMemberRepository repository = new OsobnaStrankaMemberRepository();
                OsobnaStrankaMember member = repository.GetMemberByEmail(model.Email);
                if (member == null)
                {
                    ModelState.AddModelError("", CurrentLang.GetText("Member/ContactController",  "Užívateľ nenájdený."));
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    string pswd = dt.Ticks.ToString();

                    member.IsLockedOut = false;
                    member.IsApproved = true;
                    member.Password = pswd;
                    member.PasswordRepeat = pswd;
                    MembershipCreateStatus status = repository.Save(this, member, true);
                    if (status == MembershipCreateStatus.Success)
                    {
                        status = repository.SavePassword(member);
                    }
                    if (status != MembershipCreateStatus.Success)
                    {
                        ModelState.AddModelError("", string.Format("Vyskytla sa chyba. {0} Skúste akciu zopakovať alebo nás kontaktujte.", repository.GetErrorMessage(status)));
                    }
                    else
                    {
                        List<TextTemplateParam> paramList = new List<TextTemplateParam>();
                        paramList.Add(new TextTemplateParam("LOGIN", member.Email));
                        paramList.Add(new TextTemplateParam("PASSWORD", member.Password));

                        //Odoslanie uzivatelovi
                        OsobnaStrankaMailer.SendMailTemplateWithoutBcc(
                            "Obnovenie prístupu",
                            TextTemplate.GetTemplateText("LostPassword_Sk", paramList),
                            member.Email);

                        return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.AfterPasswordResetFormId);
                    }
                }
            }
            return CurrentUmbracoPage();
        }

        [Authorize(Roles = OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole)]
        public ActionResult ChangePassword()
        {
            return View("ChangePassword", new ChangePasswordModel());
        }

        [HttpPost]
        [Authorize(Roles = OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole)]
        [ValidateAntiForgeryToken]
        public ActionResult SavePassword(ChangePasswordModel model)
        {
            if (model.NewPassword != model.NewPasswordRepeat)
            {
                ModelState.AddModelError("NewPasswordRepeat", "Zopakované nové heslo a Nové heslo musia byť rovnaké.");
            }

            if (ModelState.IsValid)
            {
                OsobnaStrankaMemberRepository repository = new OsobnaStrankaMemberRepository();
                OsobnaStrankaMember currentMember = repository.GetCurrentMember();
                currentMember.Password = model.NewPassword;
                MembershipCreateStatus status = repository.SavePassword(currentMember);
                if (status != MembershipCreateStatus.Success)
                {
                    ModelState.AddModelError("", string.Format("Vyskytla sa chyba. {0} Skúste akciu zopakovať alebo nás kontaktujte.", repository.GetErrorMessage(status)));
                }
                else
                {
                    return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.ChangePasswordFormId);
                }
            }

            return CurrentUmbracoPage();
        }

        public ActionResult Registration()
        {
            return View("Registration", new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRegistration(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                OsobnaStrankaMember newMember = OsobnaStrankaMemberModel.CreateCopyFrom(model);

                OsobnaStrankaMemberRepository memberRep = new OsobnaStrankaMemberRepository();
                OsobnaStrankaMember member = null;
                MembershipCreateStatus status = memberRep.Save(this, newMember);
                if (status != MembershipCreateStatus.Success)
                {
                    ModelState.AddModelError("", string.Format("Nastala chyba pri zápise záznamu do systému. {0}. Opravte chyby a skúste akciu zopakovať. Ak sa chyba vyskytne znovu, kontaktujte nás prosím.", memberRep.GetErrorMessage(status)));
                }
                else
                {
                    member = memberRep.GetMemberByEmail(model.Email);
                }
                if (!ModelState.IsValid)
                {
                    if (status == MembershipCreateStatus.Success && member != null)
                    {
                        // On error delete member
                        memberRep.Delete(member);
                    }
                    return CurrentUmbracoPage();
                }


                List<TextTemplateParam> paramList = new List<TextTemplateParam>();
                paramList.Add(new TextTemplateParam("LOGIN", newMember.Email));
                paramList.Add(new TextTemplateParam("LOGIN_URL", string.Format("{0}/sk/clenska-sekcia/prihlasenie", new _BaseControllerUtil().SiteRootUrl)));

                // Odoslanie uzivatelovi
                OsobnaStrankaMailer.SendMailTemplateWithoutBcc(
                    "Potvrdenie registrácie",
                    TextTemplate.GetTemplateText("NewRegistration_Sk", paramList),
                    newMember.Email);

                return this.RedirectToOsobnaStrankaUmbracoPage(ConfigurationUtil.RegistrationOkFormId);
            }
            return CurrentUmbracoPage();
        }
    }
}