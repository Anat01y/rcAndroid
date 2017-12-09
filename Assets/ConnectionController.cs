using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Assets
{
    public class ConnectionController
    {
        private Socket sock;
        private IPEndPoint endPoint;
        private int listenPort, outputPort;
        private bool connected;
        public ConnectionController(int listenPort, int outputPort)
        {
            this.listenPort = listenPort;
            this.outputPort = outputPort;
            initConnection();
        }
        public void initConnection()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UdpClient receivingUdpClient = new UdpClient(listenPort);
            endPoint = null;
            //int i = 0;
            try
            {
                // while ((endPoint == null) && (i<1000))
                // {
                //   i++;
                for (int i = 0; (i < 250) && (endPoint == null); i++)
                    if (receivingUdpClient.Available > 0)
                    {
                        byte[] receiveBytes = receivingUdpClient.Receive(ref endPoint);
                        endPoint.Port = outputPort;
                        connected = true;
                        //print("recieved" + receiveBytes[0] + " from " + endPoint.ToString());
                    }
                // }
                receivingUdpClient.Close();
            }
            catch (Exception ex)
            {
                //print(ex.ToString() + "\n  " + ex.Message);
            }
        }
        public void SendUdp(byte x, byte y)
        {

            byte[] send_buffer = { x, y };

            try
            {
                sock.SendTo(send_buffer, endPoint);
            }
            catch (Exception e)
            {
                //print(e);
                connected = false;
                initConnection();
            }
        }
        public bool IsConnected()
        {
            return connected;
        }

        public override string ToString()
        {
            if (connected)
                return endPoint.Address.ToString();
            else
                return "Подключение...";
        }
    }
}
