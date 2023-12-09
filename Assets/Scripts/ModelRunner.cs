using System;
using TMPro;
using UnityEngine;
using SocketIOClient;
using System.Collections.Generic;

public class ModelRunner : MonoBehaviour
{
    GameObject centerEye, lController, rController;
    public TMP_Text detectedText,recordingText;

    Boolean recording = false;

    Uri uri = new Uri("http://localhost:3000");
    SocketIOUnity socket;

    // Start is called before the first frame update
    void Start()
    {
        centerEye = GameObject.Find("CenterEyeAnchor");
        lController = GameObject.Find("LeftControllerAnchor");
        rController = GameObject.Find("RightControllerAnchor");

        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
        {
            {"token", "UNITY" }
        }
    ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.Connect();
        Debug.Log("socket: " + socket);
        
    }

    // Update is called once per frame
    void Update()
    {
        float[] pos = new float[] {centerEye.transform.position.x, centerEye.transform.position.y, centerEye.transform.position.z,
                       lController.transform.position.x, lController.transform.position.y, lController.transform.position.z,
                      rController.transform.position.x, rController.transform.position.y, rController.transform.position.z};

        socket.Emit("loc_update", pos);
    }
}
