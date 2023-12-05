using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModMenuChild
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        public bool linkActive { get; set; }
        public string Parent { get; set; }

        public string IconoHtml { get { return $"#{this.Icono}"; } }
        public string AClassHtml { get { return $"nav-link {(this.linkActive ? "active" : "")} d-flex align-items-center gap-2"; } }
        public string AriaCurrentHtml { get { if (this.linkActive) return "aria-current=page"; else return ""; } }
    }
}