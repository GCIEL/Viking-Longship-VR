using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class ReticleSetter : MonoBehaviour
{

    [SerializeField]
    TeleportationArea teleportationArea;

    [SerializeField]
    XRRig xrRig;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       teleportationArea.customReticle.transform.rotation = xrRig.transform.rotation;
        Debug.Log(teleportationArea.customReticle.transform.rotation);
    }
}
