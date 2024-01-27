using AcrhiveModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class ApplicabilityDto: IIdentityModel
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public int OriginalId { get; set; }
        public static explicit operator ApplicabilityDto(Applicability applicability)
        {
            return new ApplicabilityDto()
            {
                Id = applicability.Id,
                Description = applicability.Description
            };
        }
    }
}
