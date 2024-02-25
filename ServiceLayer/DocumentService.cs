using AcrhiveModels;
using AcrhiveModels.DTOs;
using ArchiveGOST_DbLibrary;
using DataBaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceLayer
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepo documentRepo;
        public DocumentService(IDocumentRepo documentRepo)
        {
            this.documentRepo = documentRepo;
        }
        public DocumentService(ArchiveDbContext dbContext)
        {
            documentRepo = new DocumentRepo(dbContext);
        }

        public async Task<int> UpsertDocument(DocumentDetailDto document)
        {
            Document newDocument = new()
            {
                Name = document.Name,
                Id = document.Id,
                Description = document.Description,
                Date = new(document.Date.Year, document.Date.Month, document.Date.Day),
                DocumentType = document.DocumentType,
                CompanyId = document.Company!.Id
            };
            return await documentRepo.UpsertDocument(newDocument);
        }
        public async Task<bool> CheckDocument(string name, DateOnly date)
        {
            return await documentRepo.CheckDocument(name, date);
        }
        public async Task<DocumentDetailDto> GetDocumentDetail(int id)
        {
            Document document = await documentRepo.GetDocumentAsync(id);
            return (DocumentDetailDto)document;
        }
        public async Task<List<DocumentListDto>> GetDocumentListAsync()
        {
            var nelist = await documentRepo.GetDocumentList();
            List<DocumentListDto> result = [];
            foreach (var document in nelist)
            {
                DocumentListDto dto = (DocumentListDto)document;
                result.Add(dto);
            }
            return result;
        }
        public async Task<List<DocumentListDto>> GetDocumentList(DocumentType type)
        {
            var nelist = await documentRepo.GetDocumentList(type);
            List<DocumentListDto> result = [];
            foreach (var document in nelist)
            {
                DocumentListDto dto = (DocumentListDto)document;
                result.Add(dto);
            }
            return result;
        }
        public async Task<List<DocumentListDto>> GetDocumentsByCompany(int companyId)
        {
            var nelist = await documentRepo.GetDocumentsByCompany(companyId);
            List<DocumentListDto> result = [];
            result.AddRange(nelist.Select(document => (DocumentListDto)document));
            return result;
        }
        public async Task DeleteDocument(int id) => await documentRepo.DeleteDocument(id);
        public async Task<DocumentListDto> GetDocumentAsync(int id)
        {
            Document document = await documentRepo.GetDocumentAsync(id);
            return (DocumentListDto)document;
        }
    }
}
