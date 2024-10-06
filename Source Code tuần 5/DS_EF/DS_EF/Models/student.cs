namespace DS_EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("student")]
    public partial class student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentID { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public decimal? AverageScore { get; set; }

        public int? FacultyID { get; set; }

        public virtual faculty Faculty { get; set; }
    }
}
