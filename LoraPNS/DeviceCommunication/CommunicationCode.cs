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
        ReadTime = 2,
        ScheduleSet = 3,
        NotificationSet = 4,

    }
}
