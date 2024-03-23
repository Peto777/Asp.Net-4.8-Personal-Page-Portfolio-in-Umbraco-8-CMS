using PeterGlozikUmbracoOsobnaStranka.lib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Repositories
{
    public class OsobnaStrankaMemberRepository : _BaseRepository
    {
        public const string OsobnaStrankaMemberTypeAlias = "clenOsobnejStranky";
        public const string OsobnaStrankaMemberAdminRole = "Admin";
        public const string OsobnaStrankaMemberCustomerRole = "User";

        public List<OsobnaStrankaMember> GetAll(string sortBy = "Name", string sortDir = "ASC")
        {
            List<OsobnaStrankaMember> dataList = new List<OsobnaStrankaMember>();
            OsobnaStrankaMemberRolesInfo rolesInfo = new OsobnaStrankaMemberRolesInfo();

            foreach (IMember member in GetAllMembers(sortBy, sortDir))
            {
                OsobnaStrankaMember dataRec = OsobnaStrankaMember.CreateCopyFrom(member);
                dataRec.IsAdminUser = rolesInfo.IsAdmin(this.MemberService, member);
                dataRec.IsCustomerUser = rolesInfo.IsCustomer(this.MemberService, member);
                dataList.Add(dataRec);
            }

            return dataList;
        }

        public List<OsobnaStrankaMember> GetCustomerUsers(string sortBy = "Name", string sortDir = "ASC")
        {
            List<OsobnaStrankaMember> dataList = new List<OsobnaStrankaMember>();
            OsobnaStrankaMemberRolesInfo rolesInfo = new OsobnaStrankaMemberRolesInfo();

            foreach (IMember member in GetAllMembers(sortBy, sortDir))
            {
                OsobnaStrankaMember dataRec = OsobnaStrankaMember.CreateCopyFrom(member);
                if (rolesInfo.IsCustomer(this.MemberService, member))
                {
                    dataList.Add(dataRec);
                }
            }

            return dataList;
        }

        IEnumerable<IMember> GetAllMembers(string sortBy = "Name", string sortDir = "ASC")
        {
            long totalRecords;

            return this.MemberService.GetAll(0, _PagingModel.AllItemsPerPage, out totalRecords, sortBy, sortDir == "DESC" ? Direction.Descending : Direction.Ascending, OsobnaStrankaMemberRepository.OsobnaStrankaMemberTypeAlias);
        }

        public OsobnaStrankaMember GetCurrentMember()
        {
            return Get(OsobnaStrankaMember.GetCurrentMemberId());
        }
        public OsobnaStrankaMember Get(int id)
        {
            return CreateCopyFrom(this.MemberService.GetById(id));
        }

        public OsobnaStrankaMember Get(string id)
        {
            return Get(int.Parse(id));
        }

        public OsobnaStrankaMember GetMemberByEmail(string email)
        {
            IMember member = this.MemberService.GetByEmail(email);
            return member == null ? null : CreateCopyFrom(member);
        }

        OsobnaStrankaMember CreateCopyFrom(IMember imember)
        {
            OsobnaStrankaMember member = OsobnaStrankaMember.CreateCopyFrom(imember);

            member.IsAdminUser = System.Web.Security.Roles.IsUserInRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberAdminRole);
            member.IsCustomerUser = System.Web.Security.Roles.IsUserInRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole);

            return member;

        }
        public MembershipCreateStatus Save(PluginController ctrl, OsobnaStrankaMember member, bool updatePermissions = true)
        {
            if (member.IsNew)
            {
                return Insert(ctrl, member);
            }
            else
            {
                return Update(member, updatePermissions);
            }
        }

        public MembershipCreateStatus Insert(PluginController ctrl, OsobnaStrankaMember member)
        {
            if (this.MemberService.GetById(member.MemberId) != null)
            {
                return MembershipCreateStatus.DuplicateProviderUserKey;
            }

            var registerModel = ctrl.Members.CreateRegistrationModel(OsobnaStrankaMemberRepository.OsobnaStrankaMemberTypeAlias);
            registerModel.Name = member.Name;
            registerModel.Email = member.Email;
            registerModel.Password = member.Password;
            registerModel.Username = member.Email;
            registerModel.UsernameIsEmail = true;

            MembershipCreateStatus status;
            var newMember = ctrl.Members.RegisterMember(registerModel, out status, false);

            if (status == MembershipCreateStatus.Success)
            {
                // Assign user roles
                if (member.IsAdminUser)
                {
                    System.Web.Security.Roles.AddUserToRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberAdminRole);
                }
                if (member.IsCustomerUser)
                {
                    System.Web.Security.Roles.AddUserToRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole);
                }
            }

            return status;
            ;
        }

        public MembershipCreateStatus Update(OsobnaStrankaMember member, bool updatePermissions)
        {
            IMember updateMember = this.MemberService.GetById(member.MemberId);
            if (updateMember == null)
            {
                return MembershipCreateStatus.UserRejected;
            }

            bool wasChange = false;

            if (updateMember.Name != member.Name)
            {
                updateMember.Name = member.Name;
                wasChange = true;
            }
            if (updateMember.Email != member.Email)
            {
                updateMember.Username = member.Email;
                updateMember.Email = member.Email;
                IMember checkMember = this.MemberService.GetByEmail(updateMember.Email);
                if (checkMember != null)
                {
                    return MembershipCreateStatus.DuplicateEmail;
                }
                wasChange = true;
            }
            if (updatePermissions)
            {
                if (updateMember.IsApproved != member.IsApproved)
                {
                    updateMember.IsApproved = member.IsApproved;
                    wasChange = true;
                }
                if (updateMember.IsLockedOut != member.IsLockedOut)
                {
                    updateMember.IsLockedOut = member.IsLockedOut;
                    wasChange = true;
                }
            }

            if (wasChange)
            {
                this.MemberService.Save(updateMember);
            }

            if (updatePermissions)
            {
                // Assign user roles
                if (member.IsAdminUser)
                {
                    System.Web.Security.Roles.AddUserToRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberAdminRole);
                }
                else
                {
                    System.Web.Security.Roles.RemoveUserFromRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberAdminRole);
                }
                if (member.IsCustomerUser)
                {
                    System.Web.Security.Roles.AddUserToRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole);
                }
                else
                {
                    System.Web.Security.Roles.RemoveUserFromRole(member.Username, OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole);
                }
            }

            return MembershipCreateStatus.Success;
        }

        public MembershipCreateStatus SavePassword(OsobnaStrankaMember member)
        {
            IMember updateMember = this.MemberService.GetById(member.MemberId);
            if (updateMember == null)
            {
                return MembershipCreateStatus.UserRejected;
            }

            try
            {
                this.MemberService.SavePassword(updateMember, member.Password);
            }
            catch
            {
                return MembershipCreateStatus.InvalidPassword;
            }

            return MembershipCreateStatus.Success;
        }

        public MembershipCreateStatus Delete(OsobnaStrankaMember member)
        {
            IMember deleteMember = this.MemberService.GetById(member.MemberId);
            if (deleteMember == null)
            {
                return MembershipCreateStatus.UserRejected;
            }
            this.MemberService.Delete(deleteMember);

            return MembershipCreateStatus.Success;
        }

        public string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.Success:
                    return string.Empty;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return "Užívateľ už existuje";
                case MembershipCreateStatus.InvalidUserName:
                    return "Neznámy užívateľ";
                case MembershipCreateStatus.InvalidPassword:
                    return "Neplatné heslo. Zadajte heslo aspoň na 8 znakov";
                case MembershipCreateStatus.InvalidQuestion:
                    return "Nesprávna otázka";
                case MembershipCreateStatus.InvalidAnswer:
                    return "Nesprávna odpoveď";
                case MembershipCreateStatus.InvalidEmail:
                    return "Nesprávny email";
                case MembershipCreateStatus.DuplicateUserName:
                case MembershipCreateStatus.DuplicateEmail:
                    return "Užívateľ pre zadaný email už existuje";
                case MembershipCreateStatus.UserRejected:
                case MembershipCreateStatus.InvalidProviderUserKey:
                    return "Neznámy užívateľ";
                case MembershipCreateStatus.ProviderError:
                    return "Neznámy typ chyby";
            }

            return "Neznámy typ chyby";
        }
    }

    public class OsobnaStrankaMember
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }

        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }

        public bool IsAdminUser { get; set; }
        public bool IsCustomerUser { get; set; }

        public bool IsNew
        {
            get
            {
                return this.MemberId <= 0;
            }
        }

        public static OsobnaStrankaMember CreateCopyFrom(IMember member)
        {
            return new OsobnaStrankaMember()
            {
                MemberId = member.Id,
                Name = member.Name,
                Username = member.Username,
                Email = member.Email,
                IsApproved = member.IsApproved,
                IsLockedOut = member.IsLockedOut,
            };
        }
        public static OsobnaStrankaMember CreateNewCustomerMember(string email)
        {
            OsobnaStrankaMember trg = new OsobnaStrankaMember();
            trg.MemberId = 0;
            trg.Name = email;
            trg.Username = email;
            trg.Email = email;
            trg.IsApproved = true;
            trg.IsLockedOut = false;
            trg.IsCustomerUser = true;

            DateTime dt = DateTime.Now;
            string pswd = dt.Ticks.ToString();

            trg.Password = pswd;
            trg.PasswordRepeat = pswd;

            return trg;
        }

        public static int GetCurrentMemberId()
        {
            MembershipUser user = GetCurrentUser();
            return (int)user.ProviderUserKey;
        }
        static MembershipUser GetCurrentUser()
        {
            return Membership.GetUser();
        }
    }

    public class OsobnaStrankaMemberRolesInfo
    {
        Hashtable htAdmin;
        Hashtable htCustomer;

        public bool IsAdmin(IMemberService service, IMember member)
        {
            return IsMemberInRole(service, member, OsobnaStrankaMemberRepository.OsobnaStrankaMemberAdminRole, ref htAdmin);
        }

        public bool IsCustomer(IMemberService service, IMember member)
        {
            return IsMemberInRole(service, member, OsobnaStrankaMemberRepository.OsobnaStrankaMemberCustomerRole, ref htCustomer);
        }

        bool IsMemberInRole(IMemberService service, IMember member, string roleName, ref Hashtable ht)
        {
            if (ht == null)
            {
                ht = LoadRolesInfo(service, roleName);
            }

            return ht.ContainsKey(member.Id);
        }

        Hashtable LoadRolesInfo(IMemberService service, string roleName)
        {
            Hashtable ht = new Hashtable();
            foreach (IMember member in service.GetMembersInRole(roleName))
            {
                ht.Add(member.Id, member);
            }

            return ht;
        }
    }
}