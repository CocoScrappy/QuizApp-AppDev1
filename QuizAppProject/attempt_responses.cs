//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizAppProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class attempt_responses
    {
        public int id { get; set; }
        public int attempt_id { get; set; }
        public int test_question_id { get; set; }
        public string response { get; set; }
        public short is_correct { get; set; }
    
        public virtual attempt attempt { get; set; }
        public virtual test_questions test_questions { get; set; }
    }
}
