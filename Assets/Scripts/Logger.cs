using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{

    GameObject centerEye, lController, rController;
    bool recording;
    StringBuilder buff;
    public TMP_Text hudText; 

    // Start is called before the first frame update
    void Start()
    {
        centerEye = GameObject.Find("CenterEyeAnchor");
        lController = GameObject.Find("LeftControllerAnchor");
        rController = GameObject.Find("RightControllerAnchor");
        buff = new StringBuilder();
        buff.AppendLine("hd_x,hd_y,hd_z,l_x,l_y,l_z,r_x,r_y,r_z");
      
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            if (hudText)
            {
                hudText.text = "Recording";
                hudText.color = Color.red;
            }

            if (!recording)
            {
                recording = true;
            }

            string hPos = Regex.Replace(centerEye.transform.position.ToString(),@"\(|\)|\s","");
            string lPos = Regex.Replace(lController.transform.position.ToString(), @"\(|\)|\s", "");
            string rPos = Regex.Replace(lController.transform.position.ToString(), @"\(|\)|\s", "");

            buff.AppendLine($"{hPos},{lPos},{rPos}");
        }
        else
        {
            if (hudText)
            {
                hudText.color = Color.gray;
                hudText.text = "Press and hold right index trigger to record";
                
            }
            
            if (recording)
            {
                recording = false;
                writeLog();
            }
        }
            
    }

    private void writeLog()
    {
        File.WriteAllText("C:\\Users\\hlele\\Unity\\Record-" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss")+ ".csv", buff.ToString());
        buff.Clear();
    }
}
