using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ShiverBot.Network
{
    internal class ConnectionManager
    {
        private Socket? sysSocket;
        internal bool IsSwitchConnected => sysSocket != null && sysSocket.Connected;

        internal string GetTitleId()
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return string.Empty;
            }

            string message = $"getTitleID\r\n";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);

            byte[] buffer = new byte[33];
            ReceiveBytes(buffer);
            return Encoding.ASCII.GetString(buffer).ToUpper();
        }

        internal string GetBuildId()
        {
            if (sysSocket == null || !IsSwitchConnected)
            {
                return string.Empty;
            }

            string message = $"getBuildID\r\n";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            sysSocket.Send(messageBytes);

            byte[] buffer = new byte[33];
            ReceiveBytes(buffer);
            return Encoding.ASCII.GetString(buffer).ToUpper();
        }

        internal bool TryConnect(string ipAddress, int port, out byte responseCode)
        {
            IPEndPoint endPoint;
            if (!IPAddress.TryParse(ipAddress, out IPAddress? ip))
            {
                responseCode = 0;
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
                    responseCode = 1;
                    return false;
                }

                responseCode = 2;
                return true;
            }

            sysSocket.Close();
            responseCode = 0;
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
        
        public void PokeAddress(string address, string data)
        {
            SendMessage($"poke 0x{address} 0x{data}\r\n");
        }

        public void PokeAddress(long address, string data)
        {
            PokeAddress($"{address:x8}", data);
        }

        private void FreezeAddress(string address, string data)
        {
            SendMessage($"freeze 0x{address} 0x{data}\r\n");
        }

        private void UnfreezeAddress(string address)
        {
            SendMessage($"unFreeze 0x{address}\r\n");
        }

        private void SendMessage(string message)
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
            while (buffer[bytesReceived - 1] != (byte)'\n')
            {
                bytesReceived += sysSocket.Receive(buffer, bytesReceived, 1, SocketFlags.None);
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
