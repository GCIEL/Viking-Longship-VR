using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using Valve.VR.InteractionSystem.Sample;

public class VRButton : MonoBehaviour
{
    public UnityEvent onPress;
    private bool canPress = true;
    private bool inPlayerHand = false;
    private Transform sphere;
    private float idleScale = 1;
    private float selectedScale => idleScale * 1.25f;
    private float pressedScale => idleScale * 0.75f;

    void Start()
    {
        sphere = transform.GetChild(0);
        idleScale = sphere.localScale.x;
    }

    public void Press()
    {
        if(inPlayerHand)
        {
            sphere.localScale = Vector3.one * pressedScale;
            if(canPress) StartCoroutine(PressCoroutine());
        }
    }

    public void Release()
    {
        sphere.localScale = Vector3.one * idleScale;
    }

    private IEnumerator PressCoroutine()
    {
        canPress = false;
        if (onPress != null) onPress.Invoke();
        yield return new WaitForSeconds(0.5f);
        canPress = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hand Palm")
        {
            if(canPress) sphere.localScale = Vector3.one * selectedScale;
            inPlayerHand = true;
        } 
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand Palm")
        {
            if(canPress) sphere.localScale = Vector3.one * idleScale;
            inPlayerHand = false;
        }
    }
}
