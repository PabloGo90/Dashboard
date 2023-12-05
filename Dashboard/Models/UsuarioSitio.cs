using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class UsuarioSitio
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DefaultView { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public UsuarioSitio()
        {

        }
        public UsuarioSitio(UsuarioSistema usr)
        {
            this.UserId = usr.idUser;
            this.Username = usr.loginUsuario;
            this.FirstName = usr.nombreCompleto;
            this.Roles = new List<Role>();
            this.Roles.Add(new Role() { RoleName = "Home" });
            this.Roles.Add(new Role() { RoleName = "Dashboard" });
            this.Roles.Add(new Role() { RoleName = "ChangePwd" });
            if (usr.isAdmin)
                this.Roles.Add(new Role() { RoleName = "Admin" });

            this.DefaultView = "Home";
        }
        public UsuarioSitio(bool test)
        {
            this.UserId = 1;
            this.Username = "user1";
            this.FirstName = "Usuario test";
            this.Roles = new List<Role>();
            this.Roles.Add(new Role() { RoleName = "Home" });
            this.Roles.Add(new Role() { RoleName = "Dashboard" });
            this.Roles.Add(new Role() { RoleName = "Admin" });
            this.DefaultView = "Home";
        }
        public UsuarioSitio(string userPortalSic)
        {
            this.UserId = 1;
            this.Username = userPortalSic;
            this.FirstName = userPortalSic;
            this.Roles = new List<Role>();
            this.Roles.Add(new Role() { RoleName = "Home" });
            this.Roles.Add(new Role() { RoleName = "Dashboard" });
            this.Roles.Add(new Role() { RoleName = "PortalSic" });
            
            this.DefaultView = "Home";
        }
    }
}