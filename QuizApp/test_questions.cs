//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class test_questions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public test_questions()
        {
            this.attempt_responses = new HashSet<attempt_responses>();
        }
    
        public int id { get; set; }
        public int test_id { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public string wrong_answers { get; set; }
        public Nullable<int> img_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<attempt_responses> attempt_responses { get; set; }
        public virtual image image { get; set; }
        public virtual test test { get; set; }
    }
}
