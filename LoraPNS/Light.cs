using Chirpstack.Api;
using LoraPNS.DeviceCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS
{
    internal class Light : LoraNode
    {

    
        Random rnd = new Random();
        FindSaparateFrame __saparateFrame = new FindSaparateFrame();
        public Light(DeviceListItem deviceInfo, string applicationID, string token) : base(deviceInfo, applicationID, token)
        {

        }

        void __ParseReciptedFromDevice(byte[] reciptedData)
        {
            __saparateFrame.ReciptedBytes(reciptedData);
          
        }

        public void OrderOnOffRightNow(int dimPercentage)
        {
            var frame = new DeviceCommunicationFrameBase() { code = CommunicationCode.OnOffRightNow, data = new byte[] { (byte)dimPercentage } }.BuildFrame();
            AddToDownlink(frame);
        }

        public void RequestReadTime()
        {
            AddToDownlink();
        }

        public void SetTimeForDevice(byte[] time)
        {
            var frame = new DeviceCommunicationFrameBase() { code = CommunicationCode.SetTime, data = time }.BuildFrame();
            AddToDownlink(frame);
        }

    }
}
