using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class InfoTrazabilidad
    {
        public string idCaso { get; set; }
        public string Fecha_Inicio_Actividad { get; set; }
        public string idResponsableActividad { get; set; }
        public string ResponsableActividad { get; set; }
        public string idActOrigen { get; set; }
        public string DesActOrigen { get; set; }
        public string idActSiguiente { get; set; }
        public string DesActSiguiente { get; set; }
        public string DesTipoEntrega { get; set; }
        public string DesTipoProceso { get; set; }
        public string DesRecepcionDesde { get; set; }
        public string QuienTrajoDoc { get; set; }
        public string Fabrica { get; set; }
        public string cantidadUnidades { get; set; }
        public string ManifiestoRecepcionSobre { get; set; }
        public string idSucursal { get; set; }
        public string DesSucursal { get; set; }
        public string Sobre { get; set; }
        public string idEstadoSobre { get; set; }
        public string EstadoSobre { get; set; }
        public string Solicitud { get; set; }
        public string Mutuo { get; set; }
        public string NumOperacion { get; set; }
        public string NumOperacionNueva { get; set; }
        public string idEstadoOperacion { get; set; }
        public string EstadoOperacion { get; set; }
        public string ManifiestoSobre { get; set; }
        public string ManifiestoSalida { get; set; }
        public string esSeguimiento { get; set; }
        public string idEstadoDevolucion { get; set; }
        public string EstadoDevolucion { get; set; }
        public string Destinatario { get; set; }
        public string NumCertificado { get; set; }
        public string FechaMarcaDevolucion { get; set; }
        public string FechaDevolucion { get; set; }
        public string UsuarioDevolucion { get; set; }
        public string EsContingencia { get; set; }
        public string NumManSalEXP { get; set; }
        public string idEstadoAprobacion { get; set; }
        public string DesEstadoAprobacio { get; set; }


    }
}