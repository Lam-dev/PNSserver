using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSserver.Model
{
    public class UplinkMessage
    {
        public string DeduplicationId { get; set; }
        public DateTime Time { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public string DevAddr { get; set; }
        public bool Adr { get; set; }
        public int Dr { get; set; }
        public int FCnt { get; set; }
        public int FPort { get; set; }
        public bool Confirmed { get; set; }
        public string Data { get; set; }
        public List<RxInfo> RxInfo { get; set; }
        public TxInfo TxInfo { get; set; }
        public string RegionConfigId { get; set; }
    }

    public class DeviceInfo
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string DeviceProfileId { get; set; }
        public string DeviceProfileName { get; set; }
        public string DeviceName { get; set; }
        public string DevEui { get; set; }
        public string DeviceClassEnabled { get; set; }
        public Dictionary<string, string> Tags { get; set; }
    }

    public class RxInfo
    {
        public string GatewayId { get; set; }
        public long UplinkId { get; set; }
        public DateTime GwTime { get; set; }
        public DateTime NsTime { get; set; }
        public int Rssi { get; set; }
        public double Snr { get; set; }
        public int Channel { get; set; }
        public Location Location { get; set; }
        public string Context { get; set; }
        public string CrcStatus { get; set; }
    }

    public class Location
    {
        // Trường location rỗng trong JSON,
        // bạn có thể bổ sung thuộc tính (lat, lon, alt...) nếu ChirpStack có gửi.
    }

    public class TxInfo
    {
        public long Frequency { get; set; }
        public Modulation Modulation { get; set; }
    }

    public class Modulation
    {
        public Lora Lora { get; set; }
    }

    public class Lora
    {
        public int Bandwidth { get; set; }
        public int SpreadingFactor { get; set; }
        public string CodeRate { get; set; }
    }
}
