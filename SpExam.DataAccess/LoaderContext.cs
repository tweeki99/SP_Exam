namespace SpExam.DataAccess
{
    using SpExam.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class LoaderContext : DbContext
    {
        public LoaderContext()
            : base("name=LoaderContext")
        {
        }

        public DbSet<Report> Reports { get; set; }
    }
}