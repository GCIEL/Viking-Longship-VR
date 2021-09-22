using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class TeleportationManager : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset actionAsset;

    private InputAction _thumbstick;

    [SerializeField]
    private XRRayInteractor rayInteractor;

    [SerializeField]
    TeleportationProvider teleportationProvider;

    [SerializeField]
    ContinuousMoveProviderBase continuousMoveProvider;

    bool _isActive;
    bool toggleVal;
    UnityEngine.XR.InputDevice controller;
    // Start is called before the first frame update
    void Start()
    {
        rayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;

        _thumbstick = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
        _thumbstick.Enable();

       
    }

    // Update is called once per frame
    void Update()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        controller = inputDevices[1]; // 1 represents left hand

        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out toggleVal) && toggleVal)
        {
            teleportationProvider.enabled = false;
            continuousMoveProvider.enabled = true;
            rayInteractor.enabled = false;
        }
        else
        {
            teleportationProvider.enabled = !false;
            continuousMoveProvider.enabled = !true;
        }
        
        if (!_isActive)
        {
            return;
        }

        if (_thumbstick.triggered)
            return;

        teleportDestination destination = CheckLocation();
        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = destination.location,
        };

        if (destination.validDestination)
        {
            teleportationProvider.QueueTeleportRequest(request);
            _isActive = false;
            rayInteractor.enabled = false;
        }
        else
        {
            rayInteractor.enabled = false;
            return;
        }
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }

    struct teleportDestination
    {
        public Vector3 location;
        public bool validDestination;
  
    }

    private teleportDestination CheckLocation()
    {
        teleportDestination destination = new teleportDestination();
        destination.validDestination = false;
        
        if (!(rayInteractor.enabled))
        {
  
            return destination;
        }
        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        { 
            return destination;
        }

        // Teleportation Anchor
        TeleportationAnchor anchor = hit.transform.GetComponent<TeleportationAnchor>();
        if (anchor)
        {
            destination.validDestination = true;
            destination.location = anchor.teleportAnchorTransform.position;
        }
        // Teleportation Area  
        else if (hit.transform.GetComponent<TeleportationArea>())
        {
            destination.validDestination = true;
            destination.location = hit.point;
        }
        else
        {
            destination.validDestination = false;
            return destination;
        }

        return destination;
    }
}
