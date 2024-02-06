using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<XRGrabInteractable>().hoverEntered.AddListener(Test);
    }

    void Test(HoverEnterEventArgs arg)
    {
        //arg.interactorObject.
        //Debug.Log("123");
    }
}
