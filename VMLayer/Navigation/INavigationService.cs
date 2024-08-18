using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLayer.Navigation
{
    public interface INavigationService
    {
        Task GoBack();
        Task GoBackAndReturn(Dictionary<string, object> parameters);
        Task GoToCreateOriginal();
        Task GoToOriginalDetails(int id);
        Task GoToCorrectionDetails(OriginalListDto original, int id = 0);
        Task GoToCreateDocument();
        Task GoToDocumentDetails(int id);
        Task GoToCreatePerson();
		Task GoToPersonDetails(int id);
	}
}
