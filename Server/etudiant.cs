//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server
{
    using System;
    using System.Collections.Generic;
    
    public partial class etudiant
    {
        public int numeroCarte { get; set; }
        public int idUser { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string specialite { get; set; }
        public string niveau { get; set; }
    
        public virtual user user { get; set; }
    }
}