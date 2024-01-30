using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

            // leftController.GetChild(2).gameObject.SetActive(false);
            // rightController.GetChild(2).gameObject.SetActive(true);
        }
        else if(dominantHand == DominantHand.Left)
        {
            leftController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = false;
            rightController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;
            
            // leftController.GetChild(2).gameObject.SetActive(true);
            // rightController.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            leftController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;
            rightController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = true;

            // leftController.GetChild(2).gameObject.SetActive(true);
            // rightController.GetChild(2).gameObject.SetActive(true);
        }
    }
}
