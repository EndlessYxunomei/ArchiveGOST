using AcrhiveModels.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcrhiveModels
{
    public class FullAuditableModel : IAuditeModel, IIdentityModel, ISoftDeletable
    {
        //public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string? LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsDeletable { get; set; }
    }
}