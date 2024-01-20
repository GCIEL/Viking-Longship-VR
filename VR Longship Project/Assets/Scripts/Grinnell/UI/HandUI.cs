using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandUI : MonoBehaviour
{
    public UIGroup wristUI;
    public Transform vrCamera;
    private ActionBasedController hand;

    void Start()
    {
        hand = GetComponent<ActionBasedController>();
    }

    public void ChangeMessage(string message)
    {
        //palmUI.SetText(message, 0);
    }

    void Update()
    {
        //if(Input.GetButton("XRI_Left_MenuButton")) Debug.Log("123");
        /*
        if(hand.interactablesSelected.Count > 0)
        {
            //palmUI.BroadcastMessage("UIToggle", false, SendMessageOptions.DontRequireReceiver);
            wristUI.BroadcastMessage("UIToggle", false, SendMessageOptions.DontRequireReceiver);
            //palmUI.LerpAlpha(0);
            wristUI.LerpAlpha(0);
            return;
        } 

        float palmAngleDiff = Vector3.Angle(transform.right, Vector3.up);
        float wristAngleDiff = Vector3.Angle(transform.right, vrCamera.forward);

        if(palmAngleDiff < 45)
        {
            palmUI.BroadcastMessage("UIToggle", true, SendMessageOptions.DontRequireReceiver);
            palmUI.LerpAlpha((45f - palmAngleDiff)/45f);
        }
        else
        {
            palmUI.BroadcastMessage("UIToggle", false, SendMessageOptions.DontRequireReceiver);
            palmUI.LerpAlpha(0);
        }

        if(wristAngleDiff < 45)
        {
            wristUI.BroadcastMessage("UIToggle", true, SendMessageOptions.DontRequireReceiver);
            wristUI.LerpAlpha((45f - wristAngleDiff)/45f);
        }
        else
        {
            wristUI.BroadcastMessage("UIToggle", false, SendMessageOptions.DontRequireReceiver);
            wristUI.LerpAlpha(0);
        }*/
    }
}
