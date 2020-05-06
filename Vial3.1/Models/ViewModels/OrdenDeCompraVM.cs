using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vial3._1.Models.ViewModels
{
    public class OrdenDeCompraVM
    {
        [Required]
        [Display(Name ="Orden de Compra")]
        public int IdOrdenDeCompra { get; set; }

        [Required]
        public int Afiliado { get; set; }

        [Display(Name = "Afiliado")]
        public string DatosAfiliado { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Emision")]
        public DateTime FechaEmision { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Vencimiento")]
        public DateTime FechaVencimiento { get; set; }


        [Required]
        [Range(500, 10000,ErrorMessage = "El valor de {0} debe estar entre {1} y {2}.")]
        public double Importe { get; set; }

        [Required]
        [Display(Name = "Comercio")]
        public int Comercio { get; set; }

        [Required]
        [Display(Name = "Estado Ord. Compra")]
        public int EstadoOC { get; set; }

        [Display(Name = "Estado Ord. Compra")]
        public string EstadoDescripcion { get; set; }

        [Display(Name = "Comercio")]
        public string RazonSocial { get; set; }


        public virtual vial_afiliados Afiliados_lst { get; set; }
        public virtual vial_comercio Comercio_lst { get; set; }
        public virtual vial_ordencomprapresentada Ordencomprapresentada_lst { get; set; }
        
    }
}