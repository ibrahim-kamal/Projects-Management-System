﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database1Entities : DbContext
    {
        public Database1Entities()
            : base("name=Database1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<feedback> feedbacks { get; set; }
        public virtual DbSet<JD> JDs { get; set; }
        public virtual DbSet<PM> PMS { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<request> requests { get; set; }
        public virtual DbSet<team> teams { get; set; }
        public virtual DbSet<TL> TLs { get; set; }
        public virtual DbSet<type> types { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
