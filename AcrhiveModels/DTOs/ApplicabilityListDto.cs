using AcrhiveModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class ApplicabilityListDto: IIdentityModel
    {
        public required int Id { get; set; }
        public static explicit operator ApplicabilityListDto(Applicability applicability)
        {
            return new ApplicabilityListDto()
            {
                Id = applicability.Id,
            };
        }
    }
}
