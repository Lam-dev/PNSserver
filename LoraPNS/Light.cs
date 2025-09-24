using Chirpstack.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS
{
    internal class Light:LoraNode
    {
       
        float U, I, P, cosphi, totalEnergy;
        byte dimPercentage, orderPlacer, notification;
        Random rnd = new Random();
        public Light(DeviceListItem deviceInfo, string applicationID, string token):base(deviceInfo, applicationID, token)
        {
            
        }

        void __ParseReciptedFromDevice(byte[] reciptedData)
        {
            
            U = (float)(rnd.NextDouble() * (260 - 150) + 150);
            I = (float)(rnd.NextDouble() * (4 - 0));
            cosphi = (float)(rnd.NextDouble() * (1 - 0));
            P = U * I * cosphi;
            totalEnergy = 100;
        }

        public void OrderOnOffRightNow(int dimPercentage)
        {
            AddToDownlink(new byte[dimPercentage]);
        }

        public void RequestReadTime()
        {
            AddToDownlink();
        }

    }
}
