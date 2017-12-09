using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace UnityStandardAssets.CrossPlatformInput {

    public class sendUDP : MonoBehaviour {
        // Use this for initialization
        Socket sock;
        IPEndPoint endPoint;
        void Start() {
            
            //Button btn = Button1.GetComponent<Button>();

            //btn.onClick.AddListener(TaskOnClick);
        }
        void TaskOnClick()
        {
            Debug.Log("You have clicked the button!");
           // SendUdp();
        }
        // Update is called once per frame
        void Update()
        {
        }
        /*
        public void SendUdp()
        {
            //EditText et = FindViewById<EditText>(Resource.Id.editText1);
           // int.TryParse(et.Text, out angle);
            string text = "a100";
            byte[] send_buffer = Encoding.ASCII.GetBytes(text);
            sock.SendTo(send_buffer, endPoint);
        }*/

        public void initConnection()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPHostEntry ipHost;

            try
            {
                ipHost = Dns.GetHostEntry("esp8266");
            }
            catch (System.Net.Sockets.SocketException e)
            {
                try
                {
                    ipHost = Dns.GetHostEntry("192.168.100.7");
                }
                catch (SocketException E)
                {
                    return;
                }
            }

            IPAddress serverAddr = ipHost.AddressList[0];

            endPoint = new IPEndPoint(serverAddr, 4321);
        }
        public void SendUdp(float x, float y)
        {

            //EditText et = FindViewById<EditText>(Resource.Id.editText1);
            //int.TryParse(et.Text, out angle);
            string text = "1"; //(axesToUse == AxisOption.OnlyVertical) ? ("y" + y) : ("x" + x);
            byte[] send_buffer = Encoding.ASCII.GetBytes(text);
            try
            {
                sock.SendTo(send_buffer, endPoint);
            }
            catch (System.Exception e)
            {
                initConnection();
            }
        }
    }
}