
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
    
public partial class Category
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Category()
    {

        this.Tests = new HashSet<Test>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public Nullable<int> ImgId { get; set; }



    public virtual Image Image { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Test> Tests { get; set; }

}

}
