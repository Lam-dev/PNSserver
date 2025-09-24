using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS.DeviceCommunication
{
    internal class ServerOrderOnOffRightNow:DeviceCommunicationFrameBase
    {
        public ServerOrderOnOffRightNow()
        {
            code = CommunicationCode.OnOffRightNow;

        }

        public byte[] GetFrame(int dimPercentage)
        {
            data = new byte[] { (byte)dimPercentage };
            return BuildFrame();
        }

    }
}
