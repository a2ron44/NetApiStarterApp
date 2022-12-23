using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net6StarterApp.Models
{
    public class BaseObject : IBaseObject
    {
        
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [MaxLength(30)]
        [Column("created_by")]
        public string? CreatedBy { get; set; }

        [Column("last_mod_date")]
        public DateTime LastModifiedDate { get; set; }

        [MaxLength(30)]
        [Column("last_mod_by")]
        public string? LastModifiedBy { get; set; }

        [MaxLength(15)]
        [Column("modify_source")]
        public string? ModifiedSource { get; set; } = "app";
    }
}

