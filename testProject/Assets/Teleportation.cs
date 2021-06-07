using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleportation : MonoBehaviour
{
    public Transform teleport1;
    public Transform teleport2;
    private bool teleportSpot = false;

    public SteamVR_Action_Boolean my_Action;
    private SteamVR_Behaviour_Pose my_Pose = null;

    void Awake()
    {
        my_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }
    void Update()
    {
        if (SerialCommunication.buttonPressed == 1 && SerialCommunication.prevButtonPressed == 0) //btn has just started being pressed
        {
            if (my_Action.GetStateUp(my_Pose.inputSource))
            {
                TeleportPlayer();
                teleportSpot = !teleportSpot;
            }
            
        }
    }
    public static void TeleportPlayer()
    {
        Debug.Log("im teleporting");
        //Transform cameraRig = SteamVR_Render.Top().origin;

      
          /*  Debug.Log("teleporty");   
            Debug.Log("camera rig");
            Vector3 headPos = SteamVR_Render.Top().head.position;
            Debug.Log("head pos");
            Vector3 headPosToGnd = new Vector3(headPos.x, cameraRig.position.y, headPos.z);
            Vector3 translatedVect;
            if (spot)
            {
                translatedVect = pos1 - headPosToGnd;
            }
            else
            {
                translatedVect = pos2 - headPosToGnd;
            }

            cameraRig.position += translatedVect;*/
    }

    private IEnumerator MoveRig(Transform camRig, Vector3 translation)
    {
        yield return null;
    }

    
}
