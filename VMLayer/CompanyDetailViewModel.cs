using ServiceLayer;
using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Validation;
using System.ComponentModel.DataAnnotations;
using AcrhiveModels;

namespace VMLayer
{
    public class CompanyDetailViewModel: BasePopupViewModel
    {
        //Сервисы
        private readonly ICompanyService companyService;

        //Поля
        private int id;
        private string _name = string.Empty;
        private string? _description;
        private bool _isCreatingMode;

        //свойства
        public bool IsCreatingMode
        {
            get => _isCreatingMode;
            set => SetProperty(ref _isCreatingMode, value);
        }
        [Required]
        [MaxLength(ArchiveConstants.MAX_ORIGINAL_NAME_LENGTH)]
        [MinLength(1)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name,value,true);
        }
        [EmptyOrWithinRange(MinLength = 1, MaxLength = ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value, true);
        }

        //Конструктор
        public CompanyDetailViewModel(IDialogService dialogService, ICompanyService companyService) : base(dialogService)
        {
            this.companyService = companyService;

            ErrorExposer = new(this);
        }

        //Загрузка данный
        public async void LoadData(int id = 0)
        {
            if (id < 0)
            {
                var dto = await companyService.GetCompanyAsync(id);
                this.id = dto.Id;
                Name = dto.Name;
                Description = dto.Description;
                IsCreatingMode = false;
            }
            else
            {
                IsCreatingMode = true;
            }
        }

        //Возвращение значений
        internal override object? ReturnValue()
        {
            return new CompanyDto()
            {
                Id = id,
                Name = Name,
                Description = Description
            };
        }

        //Обработка события валидатора
        public ValidationErrorExposer ErrorExposer { get; }
    }
}
