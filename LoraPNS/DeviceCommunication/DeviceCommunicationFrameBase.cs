using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS.DeviceCommunication
{
    internal class DeviceCommunicationFrameBase
    {
        int __headerLength = 3;
        int __codeLength = 1;
        int __dataLengthLength = 2;
        int __checkSumLength = 1;
        byte[] key;
        public string header { get; set; } = "PNS";
        public CommunicationCode code { get; set; }
        public byte[] data { get; set; } = new byte[] { };
        public int checksum { get; set; }
        public (bool, int) TryParseFrame(byte[] dataRecipted)
        {
            if (dataRecipted.Length < 6)
            {
                return (false, 0);
            }
            var dataLengthBytes = dataRecipted.Skip(4).Take(2).ToArray();
            var __dataLength = dataLengthBytes[0] + dataLengthBytes[1] * 256;
            if (__headerLength + __codeLength + __dataLengthLength + __checkSumLength + __dataLength > dataRecipted.Length) // tổng tất cả thành phần nhỏ hơn số byte đang có.=> ko đủ để cắt
                return (false, 0);
            else
            {
                var __header = dataRecipted.Take(3).ToArray();
                var __code = dataRecipted[3];
                var __dataOfFrame = dataRecipted.Skip(6).Take(__dataLength).ToArray();

                var __checkSum = dataRecipted[__headerLength + __codeLength + __dataLengthLength + __dataLength];
                byte[] __combineOfCodeLengthData = BytesUtil.CombineMany(new byte[] { __code }, dataLengthBytes, __dataOfFrame);

                var __checkSumCalculated = CalculateCheckSum(__combineOfCodeLengthData);
                if (__checkSum == __checkSumCalculated)
                {
                    header = Encoding.UTF8.GetString(__header);
                    code = (CommunicationCode)__code;
                    data = __dataOfFrame;
                    checksum = __checkSum;
                    var numberByteOfFrame = header.Length + __dataLength + 4;  // 4 byte là 1 code, 2 độ dài, 1 checksum.
                    return (true, numberByteOfFrame);
                }
                else
                {
                    throw new Exception("Checksum not match");
                }
            }
        }

        /// <summary>
        /// lấy frame dạng byte[] từ các thông tin trên để gửi cho thiết bi. 
        /// </summary>
        /// <returns></returns>
        public byte[] BuildFrame()
        {
            byte[] frame;
            var dataToSend = data;
            var dataLengthBytes = new byte[] { (byte)(dataToSend.Length % 256), (byte)(dataToSend.Length / 256) };
            var combineOfCodeLengthData = BytesUtil.CombineMany(new byte[] { (byte)code }, dataLengthBytes, dataToSend);

            frame = BytesUtil.CombineMany(Encoding.UTF8.GetBytes(header), combineOfCodeLengthData, new byte[] { CalculateCheckSum(combineOfCodeLengthData) });
            return frame;

        }
        static byte CalculateCheckSum(byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Mảng không được rỗng.");

            byte result = data[0]; // khởi tạo với phần tử đầu tiên

            for (int i = 1; i < data.Length; i++)
            {
                result ^= data[i];
            }

            return result;
        }
    }
}

