using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vial3._1.Models.ViewModels
{
    public class AfiliadoVM
    {
        public int IdAfiliados { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        [Required]
        public int Documento { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public List<OrdenDeCompraVM> OrdenDeCompra {get;set ;}

        //public virtual list<vial_ordendecompra> ordendecompra { get; set; }

      


    }


}