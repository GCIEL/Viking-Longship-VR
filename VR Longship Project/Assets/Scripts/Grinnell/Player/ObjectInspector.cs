using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class ObjectInspector : MonoBehaviour
{
    [SerializeField] private InputActionReference leftInput = null, rightInput = null;
    [SerializeField] private Transform infoPanel;
    [SerializeField] private XRRayInteractor left, right;
    
    void Start()
    {
        leftInput.action.started += Inspect;
        rightInput.action.started += Inspect;
    }

    void Inspect(InputAction.CallbackContext ctx)
    {
        if(left.interactablesHovered.Count > 0) SendInfoPanel(left.interactablesHovered[0].transform);
        if(right.interactablesHovered.Count > 0) SendInfoPanel(right.interactablesHovered[0].transform);
    }

    private void SendInfoPanel(Transform goal)
    {
        infoPanel.transform.position = goal.position + Vector3.up*0.3f;
        infoPanel.GetComponent<CanvasGroup>().alpha = 1;
        infoPanel.GetComponent<CanvasGroup>().interactable = true;
        //infoPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
