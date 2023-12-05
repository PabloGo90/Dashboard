using System;
using Dashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Dashboard.CustomAuthentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string Name { get; set; }
        public string DefaultView { get; set; }
        public ICollection<Role> Funciones { get; set; }

        #endregion

        public CustomMembershipUser(UsuarioSitio user):base("CustomMembership", user.Username, user.UserId, string.Empty, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.UserId;
            Name = user.Username;
            Funciones = user.Roles;
            DefaultView = user.DefaultView;
        }
    }
}