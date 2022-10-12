using ShiverBot.Network;

namespace ShiverBot.IO
{
    internal static class ChonkReader
    {
        internal static void GetData(string addressStr, int bytesToRead, bool main, ConnectionManager cM, string fileName)
        {
            using FileStream fS = new(fileName, FileMode.Create, FileAccess.Write);
            long address = Convert.ToInt64(addressStr, 16);
            int chunks = (int)Math.Ceiling(bytesToRead / 4194304.0);

            for (int i = 0; i < chunks; i++)
            {
                int chunkSize = 4194304;
                if (i == chunks - 1)
                {
                    chunkSize = bytesToRead % 4194304;
                }

                byte[]? data;
                if (main)
                {
                    data = cM.PeekMainAddress(address, chunkSize);
                }
                else
                {
                    data = cM.PeekAddress(address, chunkSize);
                }

                fS.Write(data);
                address += chunkSize;
            }
        }
    }
}
