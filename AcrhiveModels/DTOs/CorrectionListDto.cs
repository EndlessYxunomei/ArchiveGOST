using AcrhiveModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class CorrectionListDto: IIdentityModel
    {
        public required int Id {  get; set; }
        public required int Number {  get; set; }
        public DateTime Date { get; set; }
        public static explicit operator CorrectionListDto(Correction correction)
        {
            return new CorrectionListDto()
            {
                Id = correction.Id,
                Number = correction.CorrectionNumber,
                Date = correction.CreatedDate
            };
        }
    }
}
