using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
This class describes a slot that a placeable object fits into
You need only to give it a reference to the placeable that will fit into it (before the game start)
And set a Unity event to trigger when the placeable reaches this slot
*/
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
