using AcrhiveModels.DTOs;
using AcrhiveModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;
using ServiceLayer;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VMLayer.Validation;
using System.Diagnostics.CodeAnalysis;

namespace VMLayer
{
    public class CorrectionDetailViewModel: ObservableValidator, INavigationParameterReceiver
    {
        //Сервисы
        private readonly INavigationService navigationService;
        private readonly ICorrectionService correctionService;
        private readonly IDocumentService documentService;
        private readonly IDialogService dialogService;

        //Приватные поля
        private DocumentListDto? _document;
        private string _description = string.Empty;
        private int _correctionNumber;
        private int id;
        private int originalId;
        private int oldCorrectionNumber;
        private string _originalName = string.Empty;
        private string _originalCaption = string.Empty;

        //Свойства
        [Required]
        [Range(1, int.MaxValue)]
        public int CorrectionNumber
        {
            get => _correctionNumber;
            set => SetProperty(ref _correctionNumber, value, true);
        }
        [Required]
        [MaxLength(ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
        [MinLength(1)]
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, true);
        }
        public string OriginalName
        {
            get => _originalName;
            private set => SetProperty(ref _originalName,value);
        }
        public string OriginalCaption
        {
            get => _originalCaption;
            private set => SetProperty(ref _originalCaption, value);
        }
        [Required]
        public DocumentListDto? Document
        {
            get => _document;
            set => SetProperty(ref _document, value,true);
        }
        public ObservableCollection<DocumentListDto> DocumentList { get; set; } = [];

        //Кнопки
        public IAsyncRelayCommand AddDocumentCommand { get; }
        private async Task AddDocument()
        {
            await Task.Delay(10);//ЗАглушка
        }

        public IAsyncRelayCommand AcseptCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }
        private async Task SaveCorrection()
        {
            if(CorrectionNumber == oldCorrectionNumber || await correctionService.CheckCorrectionNumber(originalId,CorrectionNumber))
            {
                CorrectionDetailDto newCor = new()
                {
                    Id = id,
                    Description = Description,
                    CorrectionNumber = CorrectionNumber,
                    OriginalId = originalId,
                    Document = Document
                };
                int newId = await correctionService.UpsertCorrection(newCor);
                CorrectionListDto dto = await correctionService.GetCorrectionAsync(newId);
                
                ErrorsChanged -= CorrectionDetailViewModel_ErrorsChanged;
                await navigationService.GoBackAndReturn(new Dictionary<string, object>() { { NavParamConstants.CorrectionList, dto } });
            }
            else
            {
                await dialogService.Notify("Ошибка сохранения", "Данный номер изменения уже занят");
            }
        }
        private async Task CancelCorrection()
        {
            ErrorsChanged -= CorrectionDetailViewModel_ErrorsChanged;
            await navigationService.GoBack();
        }

        //Конструктор
        public CorrectionDetailViewModel(INavigationService navigationService, ICorrectionService correctionService, IDocumentService documentService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.correctionService = correctionService;
            this.documentService = documentService;
            this.dialogService = dialogService;

            ErrorExposer = new(this);

            AddDocumentCommand = new AsyncRelayCommand(AddDocument);

            AcseptCommand = new AsyncRelayCommand(SaveCorrection, () => !HasErrors);
            CancelCommand = new AsyncRelayCommand(CancelCorrection);

            ErrorsChanged += CorrectionDetailViewModel_ErrorsChanged;

            ValidateAllProperties();
        }

        //Обработка навигации и загрузка данных
        public async Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            List<DocumentListDto> documentList = await documentService.GetDocumentList(DocumentType.AddCorrection);
            foreach (DocumentListDto doc in documentList)
            {
                UtilityService.UpdateList(DocumentList, doc);
            }
            

            if (parameters.TryGetValue(NavParamConstants.DocumentList, out object? value_doc) && value_doc is DocumentListDto document)
            {
                UtilityService.UpdateList(DocumentList, document);
            }
            if (parameters.TryGetValue(NavParamConstants.OriginalList, out object? orig_det) && orig_det is OriginalListDto original)
            {
                originalId = original.Id;
                OriginalName = original.OriginalName;
                OriginalCaption = original.OriginalCaption;
            }

            if (parameters.TryGetValue(NavParamConstants.CorrectionDeatil, out object? cor_det) && cor_det is int corId && corId > 0)
            {
                var dto = await correctionService.GetCorrectionDetailAsync(corId);
                id = dto.Id;
                CorrectionNumber = dto.CorrectionNumber;
                oldCorrectionNumber = dto.CorrectionNumber;
                Description = dto.Description;

                Document = DocumentList.First(x => x.Id == dto.Document!.Id);
            }
            else
            {
                CorrectionNumber = await correctionService.GetFreeCorrectionNumber(originalId);
                oldCorrectionNumber = CorrectionNumber;
            }
        }

        //Обработка валидатора
        private void CorrectionDetailViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e) => AcseptCommand.NotifyCanExecuteChanged();
        public ValidationErrorExposer ErrorExposer { get; }
    }
}
