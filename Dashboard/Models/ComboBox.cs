using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class ComboBox
    {
        public string name { get; set; }
        public string value { get; set; }
        public string parent { get; set; }
        public bool selected { get; set; }

        public ComboBox()
        {

        }
        public ComboBox(DataRow row, int nivel)
        {
            switch (nivel)
            {
                case 1:
                    this.name = row["nameNivel1"].ToString();
                    this.value = row["value"].ToString();
                    this.parent = row["nameNivel2"].ToString();
                    break;
                case 2:
                    this.name = row["nameNivel2"].ToString();
                    this.value = row["nameNivel2"].ToString();
                    this.parent = row["nameNivel3"].ToString();
                    break;
                case 3:
                    this.name = row["nameNivel3"].ToString();
                    this.value = row["nameNivel3"].ToString();
                    this.parent = string.Empty;
                    break;
                default:
                    this.name = "Err";
                    this.value = "Err";
                    this.parent = string.Empty;
                    break;
            }
        }
    }
}