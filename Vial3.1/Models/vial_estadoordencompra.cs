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
    
    public partial class vial_estadoordencompra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vial_estadoordencompra()
        {
            this.vial_ordendecompra = new HashSet<vial_ordendecompra>();
        }
    
        public int idestadoordencompra { get; set; }
        public string estadoordencompra { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vial_ordendecompra> vial_ordendecompra { get; set; }
    }
}
