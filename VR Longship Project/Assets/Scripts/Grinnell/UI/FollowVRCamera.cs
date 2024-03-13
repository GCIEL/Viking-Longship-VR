using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVRCamera : MonoBehaviour
{
    public Transform vrCamera;

    void Update()
    {
        transform.position = vrCamera.position;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, vrCamera.rotation.eulerAngles.y, vrCamera.rotation.eulerAngles.z);
    }
}
