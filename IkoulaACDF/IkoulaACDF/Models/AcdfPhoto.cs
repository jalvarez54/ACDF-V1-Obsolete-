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
    
    public partial class AcdfPhoto
    {
        public int PhotoId { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string SchoolPeriod { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public string ThumbPath { get; set; }
        public string Place { get; set; }
    
        public virtual AcdfSubCategory AcdfSubCategory { get; set; }
    }
}
