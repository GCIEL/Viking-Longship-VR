using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;


/*
This class makes the player able to inspect objects
Inspection happens when the player is hovering an object with either right or left ray interactors and pressing the trigger
This class is meant to be attacthed to the player
*/
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
        TextAsset document = Resources.Load<TextAsset>("MD/MDFiles/" + goal.name);
        if(document == null) return;

        infoPanel.GetComponent<LazyFollow>().target = goal;
        //infoPanel.transform.parent = goal;
        infoPanel.transform.position = goal.position + Vector3.up*0.5f;
        infoPanel.GetComponent<CanvasGroup>().alpha = 1;
        infoPanel.GetComponent<CanvasGroup>().interactable = true;
        infoPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        infoPanel.GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;

        infoPanel.GetComponentInChildren<FormattedDocumentDisplay>().DisplayDocument(document);
    }
}
