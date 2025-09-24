using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraPNS
{
    internal class BytesUtil
    {
        public static byte[] CombineMany(params byte[][] arrays)
        {
            int totalLength = 0;

            // Tính tổng độ dài
            foreach (var arr in arrays)
            {
                if (arr != null)
                    totalLength += arr.Length;
            }

            byte[] result = new byte[totalLength];
            int offset = 0;

            // Chép từng mảng vào mảng kết quả
            foreach (var arr in arrays)
            {
                if (arr == null) continue;

                Buffer.BlockCopy(arr, 0, result, offset, arr.Length);
                offset += arr.Length;
            }

            return result;
        }

        public static List<byte[]> SplitBytes(byte[] source, int chunkSize = 2048)
        {
            var result = new List<byte[]>();
            int offset = 0;

            while (offset < source.Length)
            {
                int remaining = source.Length - offset;
                int size = Math.Min(chunkSize, remaining);

                byte[] chunk = new byte[size];
                Array.Copy(source, offset, chunk, 0, size);
                result.Add(chunk);

                offset += size;
            }

            return result;
        }

        public static List<byte[]> SplitBytesEnoughChuck(byte[] source, int chunkSize = 2048)
        {
            var result = new List<byte[]>();
            int offset = 0;

            while (offset < source.Length)
            {
                int remaining = source.Length - offset;
                int size = Math.Min(chunkSize, remaining);
                byte[] chunk = new byte[chunkSize];
                Array.Copy(source, offset, chunk, 0, size);
                result.Add(chunk);
                offset += size;
            }

            return result;
        }
    }
}
