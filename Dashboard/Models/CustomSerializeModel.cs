using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public int SlpCode { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string DefaultView { get; set; }
        public string Ubicacion { get; set; }
        public string Tienda { get; set; }
    }
}