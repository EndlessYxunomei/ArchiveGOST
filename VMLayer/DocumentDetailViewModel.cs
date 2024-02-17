using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMLayer.Navigation;

namespace VMLayer
{
    public class DocumentDetailViewModel : ObservableValidator, INavigationParameterReceiver
    {
        public Task OnNavigatedTo(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
