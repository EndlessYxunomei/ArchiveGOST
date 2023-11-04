using AcrhiveModels.Interfaces;

namespace AcrhiveModels
{
    public class FullAuditableModel : IAuditeModel, IIdentityModel, ISoftDeletable
    {
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Id { get; set; }
        public bool IsDeletable { get; set; }
    }
}