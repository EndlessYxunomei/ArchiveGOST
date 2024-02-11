using AcrhiveModels;
using AcrhiveModels.DTOs;
using ArchiveGOST_DbLibrary;
using DataBaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ApplicabilityService : IApplicabilityService
    {
        private readonly IApplicabilityRepo applicabilityRepo;
        private readonly IOriginalRepo originalRepo;
        
        public ApplicabilityService(IApplicabilityRepo applicabilityRepo, IOriginalRepo originalRepo)
        {
            this.applicabilityRepo = applicabilityRepo;
            this.originalRepo = originalRepo;
        }
        public ApplicabilityService(ArchiveDbContext context)
        {
            applicabilityRepo = new ApplicabilityRepo(context);
            originalRepo = new OriginalRepo(context);
        }

        public async Task<List<ApplicabilityDto>> GetApplicabilityListAsync()
        {
            var list = await applicabilityRepo.GetApplicabilityList();
            List<ApplicabilityDto> dtolist = [];
            dtolist.AddRange(list.Select(item => (ApplicabilityDto)item));
            return dtolist;
        }
        public async Task<List<ApplicabilityDto>> GetApplicabilityListByOriginalAsync(int id)
        {
            var list = await applicabilityRepo.GetApplicabilityListByOriginal(id);
            List<ApplicabilityDto> dtolist = [];
            dtolist.AddRange(list.Select(item => (ApplicabilityDto)item));
            return dtolist;
        }
        public async Task<List<ApplicabilityDto>> GetFreeApplicabilities(int originalId)
        {
            var list = await applicabilityRepo.GetFreeApplicabilityList(originalId);
            List<ApplicabilityDto> dtolist = [];
            dtolist.AddRange(list.Select(item => (ApplicabilityDto)item));
            return dtolist;
        }

        public async Task<int> UpsertApplicability(ApplicabilityDto applicability)
        {
            //Создаем новый лист для оригиналов, которые имеют данную применимость
            //List<Original> originalList = [];

            //Узанем новая ли это будет применимость или нет
            /*var existAppl = await applicabilityRepo.GetApplicabilityAsync(applicability.Id);
            if (existAppl != null)
            {
                //Если применимость уже существует, то берем её список оригиналов
                originalList = existAppl.Originals;
            }
            //Ищем наш оригинал в списке и если не находим, то добавляем
            if (originalList.Any(x => x.Id == applicability.OriginalId) == false) 
            {
                var newOrig = await originalRepo.GetOriginalAsync(applicability.OriginalId);
                originalList.Add(newOrig);
            }*/

            /*if (applicability.OriginalId > 0)
            {

            }*/

            //Создаем применимость на базе нашей дтошки и списка оригиналов
            Applicability newapp = new()
            {
                Description = applicability.Description,
                //Originals = originalList,
                Id = applicability.Id
            };
            //Сохраняем изменнения
            return await applicabilityRepo.UpsertApplicability(newapp);
        }
        public async Task DeleteApplicability(int id) => await applicabilityRepo.DeleteApplicability(id);
        public async Task DeleteOriginalFromApplicability(int id, int originalId)
        {
            var existAppl = await applicabilityRepo.GetApplicabilityAsync(id);
            if (existAppl != null)
            {
                existAppl.Originals.RemoveAll(x => x.Id == originalId);
                await applicabilityRepo.UpsertApplicability(existAppl);
            }
        }
        public async Task AddOriginalToApplicability(int id, int originalId)
        {
            var existAppl = await applicabilityRepo.GetApplicabilityAsync(id);
            if (existAppl != null && existAppl.Originals.Any(x => x.Id == originalId) == false)
            {
                var newOriginal = await originalRepo.GetOriginalAsync(originalId);
                if (newOriginal != null)
                {
                    existAppl.Originals.Add(newOriginal);
                    await applicabilityRepo.UpsertApplicability(existAppl);
                }
            }
        }

        public async Task<bool> CheckApplicability(string description)
        {
            return await applicabilityRepo.CheckApplicability(description);
        }

        public async Task<ApplicabilityDto?> GetApplicabilityAsync(int id)
        {
            var result = await applicabilityRepo.GetApplicabilityAsync(id);
            return result == null ? null : (ApplicabilityDto)result;
        }
    }
}
