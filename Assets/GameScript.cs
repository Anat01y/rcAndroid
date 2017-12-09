using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets
{

    public class GameScript : MonoBehaviour
    {

        public UnityStandardAssets.CrossPlatformInput.Joystick stickX;
        public UnityStandardAssets.CrossPlatformInput.Joystick stickY;
        public UnityEngine.UI.Text statusText;
        public UnityEngine.UI.Button updateButton;
        public UnityEngine.UI.Button calibrateButton;
        public UnityEngine.UI.Text accTextX;
        public UnityEngine.UI.Text accTextY;
        public UnityEngine.UI.Text accTextZ;
        public UnityEngine.UI.Dropdown dropDown;

        /*
        public GameObject greenPrefab;
        public GameObject redPrefab;

        public GameObject curState;
        public Vector3 statePos;*/

        public float startPosX;
        public float startPosY;
        public Vector3 posVectorX;
        public Vector3 posVectorY;

        public ConnectionController connection;
        public AccelerometrController ac;

        // Use this for initialization
        void Start()
        {
            connection = new ConnectionController(4320, 4321);
            ac = new AccelerometrController();
            startPosX = stickX.transform.position.x;
            startPosY = stickY.transform.position.y;
            posVectorX = stickX.transform.position;
            posVectorY = stickY.transform.position;
            calibrateButton.onClick.AddListener(ac.CalibrateAcceleration);
            updateButton.onClick.AddListener(connection.initConnection);
            dropDown.onValueChanged.AddListener(OnDropDownChangedListener);
            //statePos = new Vector3(156, 390, 0);
    }
        
        // Update is called once per frame
        void Update()
        {
            //print((byte)((startPosX - stickX.transform.position.x) * -1 + 100));
            //print((startPosX - stickX.transform.position.x) * -1);
            if (dropDown.value == 1)
            {
                posVectorY.y = Map((ac.GetVector3().y) * 100, -100, 100, startPosY - 100, startPosY + 100);
                stickY.transform.position = posVectorY;
                posVectorX.x = Map((ac.GetVector3().x) * 100, -100, 100, startPosX - 100, startPosX + 100);
                stickX.transform.position = posVectorX;
            }
            /*
            accTextX.text = ac.GetVector3().x.ToString();
            accTextY.text = ac.GetVector3().y.ToString();
            accTextZ.text = ac.GetVector3().z.ToString();*/
            statusText.text = connection.ToString();
            

        }

        private void FixedUpdate()
        {
            connection.SendUdp((byte)((startPosX - stickX.transform.position.x) * -1 + 100), (byte)(((startPosY - stickY.transform.position.y) * -1) + 100));
        }

        void OnDropDownChangedListener(int newValue)
        {
            if (newValue == 1)
            {
                ac.CalibrateAcceleration();
                calibrateButton.gameObject.SetActive(true);
            }
            else
            {
                calibrateButton.gameObject.SetActive(false);
                posVectorY.y = startPosY;
                stickY.transform.position = posVectorY;
                posVectorX.x = startPosX;
                stickX.transform.position = posVectorX;
            }

        }

        float Map(float value, float minIn, float maxIn, float minOut, float maxOut)
        {
            return (value - minIn) * (maxOut - minOut) / (maxIn - minIn) + minOut;
        }
        
    }
}