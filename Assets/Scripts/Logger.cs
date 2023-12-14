using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Runner : MonoBehaviour
{

    GameObject centerEye, lController, rController;
    bool recording, leftButton;
    StringBuilder buff;
    public TMP_Text hudText,classInd;
    public int classClass = 0;
    public int classEx = 0;
    private string folderName = "C:\\Users\\hlele\\Unity\\Record-" + DateTime.Now.ToString("MM-dd") + "_" + new System.Random().Next(1000) + "\\";

    // Start is called before the first frame update
    void Start()
    {
        centerEye = GameObject.Find("CenterEyeAnchor");
        lController = GameObject.Find("LeftControllerAnchor");
        rController = GameObject.Find("RightControllerAnchor");
        buff = new StringBuilder();
        Directory.CreateDirectory(folderName);

       
    }

    void IncrementClass()
    {
        classClass += 1;
        classEx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.Space))
        {
            if (hudText)
            {
                hudText.text = "Recording";
                hudText.color = Color.red;
            }

            if (!recording)
            {
                recording = true;
                buff.AppendLine("hd_x,hd_y,hd_z,l_x,l_y,l_z,r_x,r_y,r_z");
            }

            Vector3 hPos = centerEye.transform.position;
            Vector3 lPos = lController.transform.position;
            Vector3 rPos = rController.transform.position;

            

            buff.AppendLine($"{hPos.x.ToString("R")},{hPos.y.ToString("R")},{hPos.z.ToString("R")},{lPos.x.ToString("R")},{lPos.y.ToString("R")},{lPos.z.ToString("R")},{rPos.x.ToString("R")},{rPos.y.ToString("R")},{rPos.z.ToString("R")}");
        }
        else
        {
            if (hudText)
            {
                hudText.color = Color.gray;
                hudText.text = "Press and hold right index trigger to record. Press left trigger to start new class.";
                
            }
            
            if (recording)
            {
                recording = false;
                classEx += 1;
                writeLog();
            }
        }
        
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            if (!recording && !leftButton)
            {
                IncrementClass();
                leftButton = true;
            }
        }
        else
        {
            if (leftButton)
            {
                leftButton = false;
            }
        }


    }

    private void FixedUpdate()
    {
        classInd.text = string.Format("Class {0} ({1} Examples)", classClass.ToString(), classEx.ToString());
    }

    private void writeLog()
    {
        Directory.CreateDirectory(Path.Combine(folderName, classClass.ToString()));
       
        File.WriteAllText(Path.Combine(folderName,classClass.ToString(),classEx.ToString() + ".csv"), buff.ToString());
        buff.Clear();
    }
}
