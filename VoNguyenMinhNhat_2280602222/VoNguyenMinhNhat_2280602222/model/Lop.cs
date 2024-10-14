namespace VoNguyenMinhNhat_2280602222.model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lop")]
    public partial class Lop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDLop { get; set; }

        [Required]
        [StringLength(50)]
        public string TenLop { get; set; }

    }
}
