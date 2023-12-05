using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModMenu
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        public bool linkActive { get; set; }
        public bool collapse { get; set; }
        public string Parent { get; set; }
        public List<ModMenuChild> ChildMenus { get; set; }
        public bool isParent { get { return string.IsNullOrEmpty(this.Parent) && ChildMenus != null; } }

        public string IconoHtml { get { return $"#{this.Icono}"; } }
        public string AClassHtml { get { return $"nav-link {(this.linkActive ? "active" : "")} d-flex align-items-center gap-2"; } }
        public string AriaCurrentHtml { get { if (this.linkActive) return "aria-current=page"; else return ""; } }
        public string ParentID { get { if (this.isParent) return $"{this.Nombre.Replace(" ","")}-collapse"; else return ""; } }
        public string ParentIDTag { get { if (this.isParent) return $"#{this.Nombre.Replace(" ", "")}-collapse"; else return ""; } }
        public string DivChildCollapse { get { return $"collapse {(this.collapse ? "show" : "")} ps-3"; } }

        public ModMenu()
        {
            this.ChildMenus = new List<ModMenuChild>();
        }
        public ModMenu(ModMenuChild menu, int _orden)
        {
            this.Icono = menu.Icono;
            this.Id = menu.Id;
            this.Nombre = menu.Nombre;
            this.Orden = _orden;
            this.ChildMenus = new List<ModMenuChild>();
        }
        public ModMenu(List<ModMenuChild> childs, string _parent, int _orden)
        {
            this.Icono = "caret-down-fill";
            this.Nombre = _parent;
            this.Orden = _orden;
            this.ChildMenus = childs.Where(z => z.Parent == _parent).ToList();
        }
    }
}