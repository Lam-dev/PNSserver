using LoraPNS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS.DeviceCommunication
{
    internal class FindSaparateFrame
    {

        public event Action<ElectricalCharacteristicsModel> DeviceStatus;
        byte[] header = UTF8Encoding.UTF8.GetBytes("PNS");
        byte[] allData = new byte[] { };

        public bool cardPresent = false;
        public bool isReader = false;  // cờ để check có phải thiết bị đọc ecotek ko
        public string readerVersion = "";
       
        public FindSaparateFrame()
        {

        }

        public void ClearCurrentData()
        {
            allData = new byte[] { };
        }

        public void ClearListReciptedFrame()
        {
      
        }


        public void ReciptedBytes(byte[] data)
        {
            ////Console.WriteLine($"data: {BitConverter.ToString(data)}");
           
            allData = BytesUtil.CombineMany(allData, data);
            var indexOfHeader = __IndexOfHeader(allData, header);
            if (indexOfHeader != -1)
            {
                allData = allData.Skip(indexOfHeader).ToArray();
                while (true)
                {
                    try
                    {
                        var frame = new DeviceCommunicationFrameBase();
                        (var result, int numberByteOfFrame) = frame.TryParseFrame(allData);
                        //Console.WriteLine($"parse frame: {result},{numberByteOfFrame}, {frame.code}");
                        allData = allData.Skip(numberByteOfFrame).ToArray();
                        if (result)
                        {
                            if (frame.code == CommunicationCode.Status)
                            {
                                var deviceStatus = ParseFrameData.ParseDeviceStatusMessage(frame.data);
                                DeviceStatus?.Invoke(deviceStatus);
                            }
                            if (frame.code == CommunicationCode.OnOffRightNow)
                            {
                                
                            }
                            else if (frame.code == CommunicationCode.SetTime)
                            {
    
                            }
                            else if (frame.code == CommunicationCode.ReadTime)
                            {
                              
                            }
                            else if (frame.code == CommunicationCode.ScheduleSet)
                            {
                               
                            }
                            else if (frame.code == CommunicationCode.NotificationSet)
                            {
                                
                            }
                      
                        }
                        else
                        {
                            if (numberByteOfFrame == 0) // nếu ko cắt được frame. Do chưa nhận đủ dữ liệu. Thì thoát luôn vòng lặp.
                                break;
                        }
                    }
                    catch (CheckSumNotMatchException ex)
                    {
                      
                       
                    }
                }
                
            }
        }

        int __IndexOfHeader(byte[] haystack, byte[] needle)
        {
            if (haystack == null || needle == null)
                throw new ArgumentNullException();

            if (needle.Length == 0 || haystack.Length < needle.Length)
                return -1;

            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    return i;
            }
            return -1; // không tìm thấy
        }
    }
}

