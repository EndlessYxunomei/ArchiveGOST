using AcrhiveModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UtilityService
    {
        public static void UpdateList<T>(ObservableCollection<T> list, T updateItem) where T : IIdentityModel
        {
            T? exist = list.FirstOrDefault(x => x?.Id == updateItem.Id);
            if (exist != null)
            {
                list[list.IndexOf(exist)] = updateItem;
            }
            else
            {
                list.Add(updateItem);
            }
        }
    }
}
