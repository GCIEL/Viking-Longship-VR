using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Placeable : MonoBehaviour
{
    public bool onPlayerHand = false;
    public UnityEvent onPlace;
    private Coroutine translateCoroutine = null;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private XRGrabInteractable xrInteractable;
    private UnityAction<SelectEnterEventArgs> onPickUpAction;
    private UnityAction<SelectExitEventArgs> onReleaseAction;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        xrInteractable = GetComponent<XRGrabInteractable>();
        
        onPickUpAction += OnPlayerPickUp;
        xrInteractable.selectEntered.AddListener(onPickUpAction);

        onReleaseAction += OnPlayerRelease;
        xrInteractable.selectExited.AddListener(onReleaseAction);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") TranslateToStart(2f);
    }

    public void OnPlayerPickUp(SelectEnterEventArgs args) { onPlayerHand = true; }
    public void OnPlayerRelease(SelectExitEventArgs args) { onPlayerHand = false; }

    public void TranslateToFinalTarget(Transform target, float translateTime)
    {
        if(translateCoroutine == null && !onPlayerHand) translateCoroutine = StartCoroutine(TranslateToTargetCoroutine(target.position, target.rotation, translateTime, true));
    }

    public void TranslateToStart(float translateTime)
    {
        if(translateCoroutine == null && !onPlayerHand) translateCoroutine = StartCoroutine(TranslateToTargetCoroutine(originalPos, originalRot, translateTime, false));
    }

    private IEnumerator TranslateToTargetCoroutine(Vector3 targetPos, Quaternion targetRot, float translateTime, bool targetIsFinal)
    {
        xrInteractable.enabled = false;
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
        }
        
        if (targetIsFinal)
        {
            onPlace?.Invoke();
            Destroy(gameObject);
        }

        GetComponent<Rigidbody>().isKinematic = false;
        xrInteractable.enabled = true;
        translateCoroutine = null;
    }
}
