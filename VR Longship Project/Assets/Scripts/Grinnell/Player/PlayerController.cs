using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR.Features.Interactions;

public class PlayerController : MonoBehaviour
{
    /*
    public float speed = 5;
    public Transform head;
    private CharacterController controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Teleport(Transform to)
    {
        controller.enabled = false;
        transform.position = to.position;
        transform.rotation = to.rotation;
        controller.enabled = true;
    }
    public void Move(){
        Vector2 input = GetComponent<SteamVR_Behaviour_Vector2>().vector2Action.axis;
        float headYrot = head.transform.rotation.eulerAngles.y;
        Quaternion quat =  Quaternion.Euler(0,headYrot,0);
        Vector3 forward = quat * Vector3.forward;
        Vector3 right = quat * Vector3.right;
        controller.Move((forward * input.y + right * input.x) * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        Move();
        float distanceFromFloor = Vector3.Dot(head.localPosition, Vector3.up);
        controller.height = Mathf.Max(controller.radius, distanceFromFloor);
        transform.position = new Vector3(transform.position.x, (head.localPosition - 0.5f * distanceFromFloor * Vector3.up).y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
