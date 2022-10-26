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
