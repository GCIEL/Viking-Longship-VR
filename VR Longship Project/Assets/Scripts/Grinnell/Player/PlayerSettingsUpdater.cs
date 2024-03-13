using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

using Longship;

public class PlayerSettingsUpdater : MonoBehaviour
{
    [SerializeField] private PlayerSettings playerSettings;

    void Start()
    {
        ApplyAllSettings();
    }

    public void ChangeDominantHand()
    {
        playerSettings.leftHandedControls = !playerSettings.leftHandedControls;
        ApplyDominantHandSetting();
    }

    private void ApplyAllSettings()
    {
        ApplyDominantHandSetting();
    }

    private void ApplyDominantHandSetting()
    {
        bool leftHanded = playerSettings.leftHandedControls;

        Transform leftController = transform.Find("Camera Offset").Find("Left Controller").transform;
        Transform rightController = transform.Find("Camera Offset").Find("Right Controller").transform;

        leftController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = !leftHanded;
        rightController.GetComponent<ActionBasedControllerManager>().smoothMotionEnabled = leftHanded;

        leftController.GetComponentInChildren<XRInteractorLineVisual>().enabled = leftHanded;
        leftController.GetComponentInChildren<XRRayInteractor>().interactionLayers = leftHanded ? 1 << InteractionLayerMask.NameToLayer("RayInteractor") : 0;
        leftController.GetComponentInChildren<XRRayInteractor>().enableUIInteraction = leftHanded;

        rightController.GetComponentInChildren<XRInteractorLineVisual>().enabled = !leftHanded;
        rightController.GetComponentInChildren<XRRayInteractor>().interactionLayers = !leftHanded ? 1 << InteractionLayerMask.NameToLayer("RayInteractor") : 0;
        rightController.GetComponentInChildren<XRRayInteractor>().enableUIInteraction = !leftHanded;

        Transform leftUI = leftController.Find("Left Controller").Find("Joystick Affordances Left");
        Transform rightUI = rightController.Find("Right Controller").Find("Joystick Affordances Right");

        leftUI.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(leftHanded);
        leftUI.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(!leftHanded);

        rightUI.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(!leftHanded);
        rightUI.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(leftHanded);
    }
}
