using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Placeable : MonoBehaviour
{
    public bool onPlayerHand = false;
    public UnityEvent onPlace;
    private Coroutine translateCoroutine = null;
    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        Throwable throwable = GetComponent<Throwable>();
        throwable.onPickUp.AddListener(OnPlayerPickUp);
        throwable.onDetachFromHand.AddListener(OnPlayerRelease);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") TranslateToStart(2f);
    }

    public void OnPlayerPickUp() { onPlayerHand = true; }
    public void OnPlayerRelease() { onPlayerHand = false; }

    public void TranslateToFinalTarget(Transform target, float translateTime)
    {
        if(translateCoroutine == null) translateCoroutine = StartCoroutine(TranslateToTargetCoroutine(target.position, target.rotation, translateTime, true));
    }

    public void TranslateToStart(float translateTime)
    {
        if(translateCoroutine == null) translateCoroutine = StartCoroutine(TranslateToTargetCoroutine(originalPos, originalRot, translateTime, false));
    }

    private IEnumerator TranslateToTargetCoroutine(Vector3 targetPos, Quaternion targetRot, float translateTime, bool targetIsFinal)
    {
        GetComponent<Interactable>().highlightOnHover = false;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        GetComponent<Rigidbody>().isKinematic = true;
        float time = 0;
        while(time < translateTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / translateTime);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, time / translateTime);
            yield return null;
            time += Time.deltaTime;
            if(onPlayerHand)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Interactable>().highlightOnHover = true;
                translateCoroutine = null;
                yield break;
            }
        }


        
        if (targetIsFinal)
        {
            onPlace?.Invoke();
            Destroy(gameObject);
        }

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Interactable>().highlightOnHover = true;
        translateCoroutine = null;
    }
}
