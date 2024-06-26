using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
Separate code for setting the height of the model ship, intended for sliders
*/
public class ShipBuildingTable : MonoBehaviour
{
    public Vector3 minPos, maxPos;
    public void SetHeight(float lerp)
    {
        transform.position = Vector3.Lerp(minPos, maxPos, lerp);
    }

    public float GetLerpFromPosition()
    {
        return (transform.position.y - minPos.y) / (maxPos.y - minPos.y);
    }
}
