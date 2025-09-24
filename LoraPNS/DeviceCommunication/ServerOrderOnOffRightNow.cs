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
            if (dimPercentage > 100)
            {
                throw new Exception("dimPecentage > 100");
            }
            data = new byte[] { (byte)dimPercentage };
            return BuildFrame();
        }

    }
}
