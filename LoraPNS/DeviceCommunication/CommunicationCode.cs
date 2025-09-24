using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS.DeviceCommunication
{
    internal enum CommunicationCode
    {
        Status = 0,
        OnOffRightNow = 1,
        SetTime = 3,
        ReadTime = 4,
        ScheduleSet = 5,
        NotificationSet = 6,

    }
}
