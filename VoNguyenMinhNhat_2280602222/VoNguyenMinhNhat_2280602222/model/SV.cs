namespace VoNguyenMinhNhat_2280602222.model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SV")]
    public partial class SV
    {
        [Key]
        [StringLength(50)]
        public string MaSV { get; set; }

        [Required]
        [StringLength(50)]
        public string TenSV { get; set; }

        public DateTime NgaySinh { get; set; }
        public virtual Lop Lop { get; set; }
    }
}
