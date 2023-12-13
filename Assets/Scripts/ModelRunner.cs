using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using Meta.WitAi.Json;

public class ModelRunner : MonoBehaviour
{
    GameObject centerEye, lController, rController;
    public TMP_Text detectedText;

    Boolean recording = false;

    Uri uri = new Uri("http://localhost:3000");

    List<string[]> buffer = new List<string[]>();
    int size = 0;

    Dictionary<string, string> labels = new Dictionary<string, string>() { { "0", "idle" }, { "1", "clap 2x" }, { "2", "T-pose" }, { "3", "raise right hand" }, { "4", "black panther pose" }, { "5", "yes" }, { "6", "no" } } ;

    // Start is called before the first frame update
    async void Start()
    {
        centerEye = GameObject.Find("CenterEyeAnchor");
        lController = GameObject.Find("LeftControllerAnchor");
        rController = GameObject.Find("RightControllerAnchor");
        
    }



    // Update is called once per frame
    void Update()
    {
        
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            if (!recording)
            {
                recording = true;
                detectedText.text = "Recording...";
                detectedText.color = Color.red;
            }

            buffer.Add(new string[] { centerEye.transform.position.x.ToString("R"), centerEye.transform.position.y.ToString("R"), centerEye.transform.position.z.ToString("R"), lController.transform.position.x.ToString("R"), lController.transform.position.y.ToString("R"), lController.transform.position.z.ToString("R"), rController.transform.position.x.ToString("R"), rController.transform.position.y.ToString("R"), rController.transform.position.z.ToString("R") });
        }
        else
        {
            if (recording)
            {
                recording = false;
            }

            if (buffer.Count > 0)
            {
                Debug.Log("Sending rq");
                Request();
                
            }
        }

       
    }

    public void Request()
    {
        try
        {
            string url = "http://localhost:3000/";

            var request = UnityWebRequest.Post(url, $@"{JsonConvert.SerializeObject(buffer.ToArray())}", "application/text");
            request.SetRequestHeader("Accept", "*/*");
            StartCoroutine(onResponse(request));
            buffer.Clear();
        }
        catch (Exception e) { Debug.Log("ERROR : " + e.Message); }
    }
    private IEnumerator onResponse(UnityWebRequest req)
    {

        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success) {
            ClassificationResult result;
            Debug.Log(req.downloadHandler.text);
            result = JsonUtility.FromJson<ClassificationResult>(req.downloadHandler.text);
            
            if(result.result != null)
            {
                detectedText.text = "Detected Class: " + labels[result.result];
                detectedText.color = Color.green;
            }
        }
        else
            Debug.Log("Error " + req.error);
        
        // Some code after success

    }


    [Serializable]
    public class ClassificationResult
    {
        public string result;
    }

}
