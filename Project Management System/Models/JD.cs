//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_Management_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JD()
        {
            this.feedbacks = new HashSet<feedback>();
        }
    
        public int id { get; set; }
        public int user_id { get; set; }
        public int coding_skills { get; set; }
        public int system_design { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<feedback> feedbacks { get; set; }
        public virtual user user { get; set; }
    }
}