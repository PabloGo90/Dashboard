using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Dashboard.CustomAuthentication
{
    public class CustomRole : RoleProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            if (HttpContext.Current.User != null &&
             ((CustomPrincipal)HttpContext.Current.User).Funciones != null &&
             ((CustomPrincipal)HttpContext.Current.User).Funciones.Count() > 0)
            {
                return ((CustomPrincipal)HttpContext.Current.User).Funciones.ToArray();
            }
            else
            {
                var userRoles = new string[] { };
                /*
                using (AuthenticationDB dbContext = new AuthenticationDB())
                {
                    var selectedUser = (from us in dbContext.Users.Include("Roles")
                                        where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                        select us).FirstOrDefault();


                    if(selectedUser != null)
                    {
                        userRoles = new[] { selectedUser.Roles.Select(r=>r.RoleName).ToString() };
                    }

                    return userRoles.ToArray();
                }*/

                return userRoles.ToArray();

            }


        }



        #region Overrides of Role Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            string[] roles = { };
            if (HttpContext.Current.User != null &&
                ((CustomPrincipal)HttpContext.Current.User).Funciones != null &&
                ((CustomPrincipal)HttpContext.Current.User).Funciones.Count() > 0)
            {
                roles = ((CustomPrincipal)HttpContext.Current.User).Funciones.ToArray();
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}