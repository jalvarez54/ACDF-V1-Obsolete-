//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IkoulaACDF.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AcdfSubCategory
    {
        public AcdfSubCategory()
        {
            this.AcdfPhotoes = new HashSet<AcdfPhoto>();
        }
    
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsEnable { get; set; }
        public Nullable<int> CategoryId { get; set; }
    
        public virtual ICollection<AcdfPhoto> AcdfPhotoes { get; set; }
        public virtual AcdfCategory AcdfCategory { get; set; }
    }
}
