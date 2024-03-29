﻿using AcrhiveModels;
using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IDocumentService
    {
        Task<List<DocumentListDto>> GetDocumentListAsync();
        Task<List<DocumentListDto>> GetDocumentList(DocumentType type);
        Task<List<DocumentListDto>> GetDocumentsByCompany(int companyId);
        Task<int> UpsertDocument (DocumentDetailDto document);
        Task<DocumentDetailDto> GetDocumentDetail(int id);
        Task<DocumentListDto> GetDocumentAsync(int id);
        Task DeleteDocument (int id);
        Task<bool> CheckDocument(string name, DateOnly date);
    }
}
