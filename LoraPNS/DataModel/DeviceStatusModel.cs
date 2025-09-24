using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS.DataModel
{
    internal class ElectricalCharacteristicsModel
    {
        public float U { get; set; }
        public float I { get; set; }
        public float P { get; set; }
        public float cosphi { get; set; }
        public float totalEnergy { get; set; }

        public byte dimPercentage { get; set; }
        public byte dimOrderPlacer { get; set; }
        public byte notificationP { get; set; }
    }
}
