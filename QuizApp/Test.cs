
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
    
public partial class Test
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Test()
    {

        this.Attempts = new HashSet<Attempt>();

        this.TestQuestions = new HashSet<TestQuestion>();

    }


    public int Id { get; set; }

    public int Amount { get; set; }

    public string Difficulty { get; set; }

    public int CategoryId { get; set; }

    public string Type { get; set; }

    public Nullable<int> ImgId { get; set; }

    public int OwnerId { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Attempt> Attempts { get; set; }

    public virtual Category Category { get; set; }

    public virtual Image Image { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TestQuestion> TestQuestions { get; set; }

    public virtual User User { get; set; }

}

}
