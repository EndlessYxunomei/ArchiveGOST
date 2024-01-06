using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLayer.Validation
{
    public class ValidationErrorExposer : INotifyPropertyChanged, IDisposable
    {
        readonly ObservableValidator validator;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ValidationErrorExposer(ObservableValidator observableValidator)
        {
            validator = observableValidator;
            validator.ErrorsChanged += ObservableValidator_ErrorsChanged;
        }

        private void ObservableValidator_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Item[{e.PropertyName}]"));

        public void Dispose()
        {
            validator.ErrorsChanged -= ObservableValidator_ErrorsChanged;
            GC.SuppressFinalize(this);
        }

        public List<ValidationResult> this[string property]
            => validator.GetErrors(property).ToList();
    }
}
