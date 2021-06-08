using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleportation : MonoBehaviour
{

    void Awake()
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
    void Update()
    {
        if (SerialCommunication.buttonPressed == 1 && SerialCommunication.prevButtonPressed == 0) //btn has just started being pressed
        {
            TeleportPlayer();
        }
    }
    public static void TeleportPlayer()
    {
        Debug.Log("im teleporting");
        
    }

  
}
