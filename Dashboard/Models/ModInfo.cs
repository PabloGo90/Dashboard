using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ModInfo
    {
        public int Id { get; set; }
        public string TituloNombre { get; set; }
        public string descripcion { get; set; }        
        public ModMenu Menu { get; set; }
        public List<ModItem> Items { get; set; }
        public int Orden { get; set; }
        public List<ModParametros> Parametros { get; set; }
        public bool activo { get; set; }
        public string html_Form { get { return this.TituloNombre.Trim().Replace(" ","") + "Search"; } }
        public string html_SubMenu { get { return string.Format("{0}-menu-list", this.TituloNombre.Trim().Replace(" ", "")); } }

        public ModInfo()
        {
            this.Menu = new ModMenu();
            this.Items = new List<ModItem>();
            this.Parametros = new List<ModParametros>();
            this.TituloNombre = string.Empty;
        }
        public void mapModInfo(DataSet ds)
        {
            List<ModGDataset> _gDataset = new List<ModGDataset>();

            //Info
            this.Id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            this.TituloNombre = ds.Tables[0].Rows[0]["nombre"].ToString();
            this.Menu.Nombre = ds.Tables[0].Rows[0]["nombreMenu"].ToString();
            this.descripcion = ds.Tables[0].Rows[0]["descripcion"].ToString();
            this.activo = ds.Tables[0].Rows[0]["activo"].ToString() == "Y";

            //Dataset
            foreach (DataRow row in ds.Tables[2].Rows)
                _gDataset.Add(new ModGDataset(row));

            //Items
            foreach (DataRow row in ds.Tables[1].Rows)
                this.Items.Add(new ModItem(row, _gDataset));

            //Parametros
            foreach (DataRow row in ds.Tables[3].Rows)
                this.Parametros.Add(new ModParametros(row));
        }
    }
}