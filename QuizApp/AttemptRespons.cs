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
    
    public partial class AttemptRespons
    {
        public int Id { get; set; }
        public int AttemptId { get; set; }
        public int TestQuestionId { get; set; }
        public string Response { get; set; }
        public short IsCorrect { get; set; }
    
        public virtual Attempt Attempt { get; set; }
        public virtual TestQuestion TestQuestion { get; set; }
    }
}