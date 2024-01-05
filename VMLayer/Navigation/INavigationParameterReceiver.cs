using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLayer.Navigation
{
    public interface INavigationParameterReceiver
    {
        Task OnNavigatedTo(Dictionary<string, object> parameters);
    }
}
