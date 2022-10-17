using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShiverBot.Network
{
    internal class ConnectionManager
    {
        private Socket? sysSocket;
        private List<string>? frozenAddresses;
        internal bool IsSwitchConnected => sysSocket != null && sysSocket.Connected;
        private long heapBase;

        internal bool TryConnect(string ipAddress, int port, out string error)
        {
            IPEndPoint endPoint;
            if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
            {
                error = "Not a valid IP address.";
                return false;
            }

            sysSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sysSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            endPoint = new IPEndPoint(ip, port);
            IAsyncResult result = sysSocket.BeginConnect(endPoint, null, null);

            bool connectionSuccess = result.AsyncWaitHandle.WaitOne(3000, true);
            if (connectionSuccess)
            {
                try
                {
                    sysSocket.EndConnect(result);
                }
                catch (Exception ex)
                {
                    // TODO Program.GetLoggingInstance().LogException(ex, "(TryConnect)");
                    sysSocket.Close();
                    error = ex.ToString();
                    return false;
                }

                error = string.Empty;
                frozenAddresses = new();
                heapBase = Convert.ToInt64(SendCommandAsIs("getHeapBase", 33)[..16], 16);
                return true;
            }

            sysSocket.Close();
            error = "Timed out. There was nothing at the specified IP address.";
            return false;
        }

        public byte[]? PeekAddress(string address, int size)
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return null;
            }

            string message = $"peek 0x{address} {size}\r\n";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);

            byte[] buffer = new byte[(size * 2) + 1];
            ReceiveBytes(buffer);
            return DecoderUtil.ConvertHexByteStringToBytes(buffer);
        }

        public byte[]? PeekAddress(long address, int size)
        {
            return PeekAddress($"{address:x8}", size);
        }

        public byte[]? PeekMainAddress(string address, int size)
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return null;
            }

            string message = $"peekMain 0x{address} {size}\r\n";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);

            byte[] buffer = new byte[(size * 2) + 1];
            ReceiveBytes(buffer);
            return DecoderUtil.ConvertHexByteStringToBytes(buffer);
        }

        public byte[]? PeekMainAddress(long address, int size)
        {
            return PeekMainAddress($"{address:x8}", size);
        }

        public byte[]? PeekAbsoluteAddress(string address, int size)
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return null;
            }

            string message = $"peekAbsolute 0x{address} {size}\r\n";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);

            byte[] buffer = new byte[(size * 2) + 1];
            ReceiveBytes(buffer);
            return DecoderUtil.ConvertHexByteStringToBytes(buffer);
        }

        public byte[]? PeekAbsoluteAddress(long address, int size)
        {
            return PeekAbsoluteAddress($"{address:x8}", size);
        }

        public void PokeAddress(string address, string data)
        {
            SendMessage($"poke 0x{address} 0x{data}\r\n");
        }

        public void PokeAddress(long address, string data)
        {
            PokeAddress($"{address:x8}", data);
        }

        public void PokeAbsoluteAddress(string address, string data)
        {
            SendMessage($"pokeAbsolute 0x{address} 0x{data}\r\n");
        }

        public void PokeAbsoluteAddress(long address, string data)
        {
            PokeAbsoluteAddress($"{address:x16}", data);
        }

        public void FreezeAddress(string address, string data)
        {
            SendMessage($"freeze 0x{address} 0x{data}\r\n");
            if (frozenAddresses != null && !frozenAddresses.Contains(address))
            {
                frozenAddresses.Add(address);
            }
        }

        public void FreezeAddress(long address, string data)
        {
            if (address > 0xFFFFFFFF)
            {
                FreezeAddress($"{address - heapBase:x8}", data);
            }
            else
            {
                FreezeAddress($"{address:x8}", data);
            }
        }

        public void UnfreezeAddress(string address)
        {
            SendMessage($"unFreeze 0x{address}\r\n");
            if (frozenAddresses != null && frozenAddresses.Contains(address))
            {
                frozenAddresses.Remove(address);
            }
        }

        public void UnfreezeAddress(long address)
        {
            if (address > 0xFFFFFFFF)
            {
                UnfreezeAddress($"{address - heapBase:x8}");
            }
            else
            {
                UnfreezeAddress("${address:x8}");
            }
        }

        public void UnfreezeAllAddresses()
        {
            if (frozenAddresses != null)
            {
                lock (frozenAddresses)
                {
                    foreach (string address in frozenAddresses)
                    {
                        long address64 = Convert.ToInt64(address, 16);
                        if (address64 > 0xFFFFFFFF)
                        {
                            UnfreezeAddress($"{address64 - heapBase:x8}");
                        }
                        else
                        {
                            UnfreezeAddress(address);
                        }
                    }
                }
            }
        }

        public long TrackStepsAddress(string address, List<MemoryStep> steps, byte mode)
        {
            long rf;

            byte[]? baseData;
            switch (mode)
            {
                case 0:
                default:
                    baseData = PeekAddress(address, 8);
                    break;

                case 1:
                    baseData = PeekMainAddress(address, 8);
                    break;

                case 2:
                    baseData = PeekAbsoluteAddress(address, 8);
                    break;
            }

            rf = BitConverter.ToInt64(baseData);

            foreach (MemoryStep step in steps)
            {
                switch (step.Action)
                {
                    case '+':
                        rf += step.Step;
                        break;

                    case 'L':
                        byte[]? data = PeekAbsoluteAddress(rf + step.Step, 8);
                        rf = BitConverter.ToInt64(data);
                        break;
                }
            }

            return rf;
        }

        internal string SendCommandAsIs(string command, int bufferSize)
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return string.Empty;
            }

            string message = $"{command}\r\n";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);

            byte[] buffer = new byte[bufferSize];
            ReceiveBytes(buffer);
            return Encoding.ASCII.GetString(buffer).ToUpper();
        }

        public void SendMessage(string message)
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return;
            }

            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);
        }

        private void ReceiveBytes(byte[] buffer)
        {
            if (sysSocket == null)
            {
                return;
            }

            int bytesReceived = sysSocket.Receive(buffer, 0, 1, SocketFlags.None);
            try
            {
                while (buffer[bytesReceived - 1] != (byte)'\n')
                {
                    bytesReceived += sysSocket.Receive(buffer, bytesReceived, 1, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error while receiving bytes:\r\n{ex}", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void TryDisconnect()
        {
            if (sysSocket == null)
            {
                return;
            }

            try
            {
                UnfreezeAllAddresses();

                IAsyncResult result = sysSocket.BeginDisconnect(true, null, null);

                bool disconnectionSuccess = result.AsyncWaitHandle.WaitOne(3000, true);
                if (disconnectionSuccess)
                {
                    sysSocket.EndDisconnect(result);
                }

                sysSocket.Close();
            }
            catch (Exception ex)
            {
                // TODO Program.GetLoggingInstance().LogException(ex, "(TryDisconnect)");
            }
        }
    }
}
