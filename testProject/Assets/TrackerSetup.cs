using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackerSetup : MonoBehaviour
{

    void Awake()
    {
        SetDeviceNum();
    }
    void Update()
    {
       
    }

    private void SetDeviceNum()
    {
        uint ind = 0;
        var err = ETrackedPropertyError.TrackedProp_Success;
        for (uint i = 0; i < 16; i++)
        {
            var result = new System.Text.StringBuilder((int) 64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result,
                64, ref err);
            if (result.ToString().Contains("tracker"))
            {
                ind = i;
                break;
            }
        }
        //set TrackedObject index
        GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex) ind;
    }
    
}