using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRow : MonoBehaviour
{
    GameObject lController, rController;
    

    // Start is called before the first frame update
    void Start()
    {
        lController = GameObject.Find("LeftHandAnchor");
        rController = GameObject.Find("RightHandAnchor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float[] GetDataRow()
    {
        Vector3 lPos = Camera.main.transform.InverseTransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
        Vector3 rPos = Camera.main.transform.InverseTransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
        //Vector3 lVel = Camera.main.transform.InverseTransformPoint(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch));
        //Vector3 rVel = Camera.main.transform.InverseTransformPoint(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch));

        return new float[] { lPos.x, lPos.y, lPos.z, rPos.x, rPos.y, rPos.z /*lVel.x, lVel.y, lVel.z, rVel.x, rVel.y, rVel.z*/ };
    }

    public string[] strRow()
    {
        List<string> s = new List<string>();
        foreach (float f in GetDataRow())
        {
            s.Add(f.ToString("R"));
        }
        return s.ToArray();
    }
}
