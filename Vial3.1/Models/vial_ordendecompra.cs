//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vial3._1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vial_ordendecompra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vial_ordendecompra()
        {
            this.vial_ordencomprapresentada = new HashSet<vial_ordencomprapresentada>();
        }
    
        public int idOrdenDeCompra { get; set; }
        public int Afiliado { get; set; }
        public double importe { get; set; }
        public int comercio { get; set; }
        public Nullable<System.DateTime> fecha_emision { get; set; }
        public Nullable<System.DateTime> fecha_vencimiento { get; set; }
        public int estado { get; set; }
    
        public virtual vial_afiliados vial_afiliados { get; set; }
        public virtual vial_comercio vial_comercio { get; set; }
        public virtual vial_estadoordencompra vial_estadoordencompra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vial_ordencomprapresentada> vial_ordencomprapresentada { get; set; }
    }
}
