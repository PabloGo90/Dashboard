using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class UsuarioSistema
    {
        public int idUser { get; set; }
        public string nombreCompleto { get; set; }
        public string pass { get; set; }
        public string rut { get; set; }
        public bool activo { get; set; } = true;
        public string loginUsuario { get; set; }
        //public string fechaUltimoIngreso { get; set; }
        public string correo { get; set; }
        public string fono { get; set; }
        //public string TCS { get; set; }
        public string ip_usuario { get; set; }
        public string caducidad { get; set; }
        //public string intentos_fallidos { get; set; }
        //public string FechaActivacion { get; set; }
        //public string FechaExpiracion { get; set; }
        public string conectado { get; set; }
        public bool isAdmin { get; set; }


        public int salida { get; set; }
        public string pass2 { get; set; }
        public bool flagCambioPwd { get { return !string.IsNullOrEmpty(pass); } }


    }
}