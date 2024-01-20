using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceableSlot : MonoBehaviour
{
    public Placeable placeable;
    public UnityEvent onPlace;

    void Start()
    {
        placeable.onPlace.AddListener(Place);
    }

    public void Place()
    {
        GetComponent<MeshRenderer>().enabled = true;
        onPlace?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Placeable>() != null && other.GetComponent<Placeable>() == placeable)
        {
            //placeable.TranslateToFinalTarget(transform, 1f);
        }
    }
}
