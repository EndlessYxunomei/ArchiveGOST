using AcrhiveModels.DTOs;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMLayer.Messages
{
    public class OriginalUpdatedMessage(OriginalListDto value) : ValueChangedMessage<OriginalListDto>(value)
    {
    }
}
