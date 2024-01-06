using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class ApplicabilityListDto
    {
        public static explicit operator ApplicabilityListDto(Applicability applicability)
        {
            return new ApplicabilityListDto();
        }
    }
}
