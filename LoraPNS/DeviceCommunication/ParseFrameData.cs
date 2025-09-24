using LoraPNS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS.DeviceCommunication
{
    internal class ParseFrameData
    {
        public static ElectricalCharacteristicsModel ParseDeviceStatusMessage(byte[] data)
        {
            var rnd = new Random();
            return new ElectricalCharacteristicsModel()
            {
                U = (float)(rnd.NextDouble() * (260 - 150) + 150),
                I = (float)(rnd.NextDouble() * (4 - 0)),
                cosphi = (float)(rnd.NextDouble() * (1 - 0)),
                P = 200,
                totalEnergy = 100,
            };
        }
    }
}
