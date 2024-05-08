using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerDominantHandSelector : MonoBehaviour
{
    public enum DominantHand
    {
        None,
        Right,
        Left
    }

    [SerializeField] private Transform leftController, rightController;
    private DominantHand dominantHand = DominantHand.None;

    public void SetDominantHand(TMPro.TMP_Dropdown tMP_Dropdown)
    {
        DominantHand dominantHand = (DominantHand) tMP_Dropdown.value;
        if(dominantHand == DominantHand.Right)
        {
            leftController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;
            rightController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = false;

            leftController.GetComponentInChildren<XRInteractorLineVisual>().enabled = false;
            leftController.GetComponentInChildren<XRRayInteractor>().interactionLayers = InteractionLayerMask.NameToLayer("Default");
            rightController.GetComponentInChildren<XRInteractorLineVisual>().enabled = true;
        }
        else if(dominantHand == DominantHand.Left)
        {
            leftController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = false;
            rightController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;
            
            leftController.GetComponentInChildren<XRInteractorLineVisual>().enabled = true;
            rightController.GetComponentInChildren<XRInteractorLineVisual>().enabled = false;
        }
        else
        {
            leftController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;
            rightController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;

            leftController.GetComponentInChildren<XRInteractorLineVisual>().enabled = true;
            rightController.GetComponentInChildren<XRInteractorLineVisual>().enabled = true;
        }
    }
}
