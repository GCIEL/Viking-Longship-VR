using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class TestSelectFilter : MonoBehaviour, IXRSelectFilter
{
    public bool canProcess => true;

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        return interactable.transform.tag != "Test" && interactable.transform.tag != "Ship Piece";
    }
}
