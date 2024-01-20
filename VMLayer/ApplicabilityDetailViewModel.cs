﻿using AcrhiveModels.DTOs;
using AcrhiveModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Validation;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;

namespace VMLayer
{
    public class ApplicabilityDetailViewModel: BasePopupViewModel
    {
        //Сервисы
        private readonly IApplicabilityService applicabilityService;

        //приватыне поля
        private string _description = string.Empty;
        private int id;
        private bool _isCreatingMode;
        private int originalId;
        private ApplicabilityListDto? _selectedDto;

        //Свойсва
        public bool IsCreatingMode
        { 
            get => _isCreatingMode;
            set => SetProperty(ref _isCreatingMode,value);
        }
        [CustomValidation(typeof(ApplicabilityDetailViewModel), nameof(ValidateApplicabilityDescription))]
        [EmptyOrWithinRange(MinLength = 1, MaxLength = ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
        public string Description
        {
            get => _description;
            set
            {
                SelectedDto = null;
                SetProperty(ref _description, value, true);
            }
        }
        public ApplicabilityListDto? SelectedDto
        {
            get => _selectedDto;
            set
            {
                SetProperty(ref _selectedDto, value);
                ValidateProperty(Description, nameof(Description));
            }
        }
        public ObservableCollection<ApplicabilityListDto> DtoList { get; set; } = [];

        //Конструктор
        public ApplicabilityDetailViewModel(IDialogService dialogService, IApplicabilityService applicabilityService): base(dialogService)
        { 
            this.applicabilityService = applicabilityService;

            ErrorExposer = new(this);
        }

        //Загрузка данных
        public async void LoadData(int original, object? paprams = null)
        {
            originalId = original;
            
            if (paprams != null && paprams is ApplicabilityListDto dto)
            {
                IsCreatingMode = false;
                Description = dto.Description;
                id = dto.Id;
            }
            else
            {
                IsCreatingMode = true;
                var list = await applicabilityService.GetFreeApplicabilities(originalId);
                list.ForEach(DtoList.Add);
            }
        }

        //Возврат данных из диалогового окна
        internal override object? ReturnValue()
        {
            if (SelectedDto != null)
            {
                return SelectedDto;
            }
            return new ApplicabilityListDto()
            {
                Description = Description,
                Id = id,
                OriginalId = originalId
            };
        }
        
        //Обработка события валидатора
        public ValidationErrorExposer ErrorExposer { get; }
        //Дополниетльный атрибут для валидации
        public static ValidationResult ValidateApplicabilityDescription(string description,ValidationContext context)
        {
            ApplicabilityDetailViewModel instance = (ApplicabilityDetailViewModel)context.ObjectInstance;
            if (instance.SelectedDto != null && description.IsNullOrEmpty() == false)
            {
                return new("Нельзя выбрать и то и то.");
            }
            else if (instance.SelectedDto == null && description.IsNullOrEmpty())
            {
                return new("Нельзя создать пустую запись.");
            }
            else
            {
                return ValidationResult.Success!;
            }
        }
    }
}