using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceableSlot : MonoBehaviour
{
    public Placeable placeable;
    private Material placeableMaterial;
    public UnityEvent onPlace;

    void Awake()
    {
        placeableMaterial = placeable.GetComponent<MeshRenderer>().material;
        placeable.onPlace.AddListener(Place);
    }

    public void Place()
    {
        GetComponent<MeshRenderer>().material = placeableMaterial;
        GetComponent<MeshRenderer>().enabled = true;
        onPlace?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Placeable>() != null && other.GetComponent<Placeable>() == placeable)
        {
            placeable.TranslateToFinalTarget(transform, 1f);
        }
    }
}
