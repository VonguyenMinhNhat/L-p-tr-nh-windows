using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace VoNguyenMinhNhat_2280602222.model
{
    public partial class Model1 : DbContext
    {
        public Model1()
            :base("name=Model1")
        {

        }

        public virtual DbSet<Lop> Lops { get; set; }
        public virtual DbSet<SV> SVs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
