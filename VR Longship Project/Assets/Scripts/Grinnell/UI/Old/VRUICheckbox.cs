using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VRUICheckbox : MonoBehaviour
{
    public UnityEvent onCheck, onUncheck;
    public bool startState = false;

    private Color idle, clicked;
    private bool isClicking = false;
    private Image background;
    private GameObject checkmark;
    private bool state = false;
    private bool uiEnabled = true;

    public void UIToggle(bool state) { uiEnabled = state; }

    void Awake()
    {
        checkmark = transform.GetChild(0).gameObject;
        background = GetComponent<Image>();
        idle = background.color;
        clicked = background.color/2f; clicked.a = 1f;
    }

    void Start() { Toggle(startState); }

    void OnTriggerEnter(Collider collider)
    {
        if(isClicking || !uiEnabled) return;
        if(collider.tag == "Index Fingertip") StartCoroutine(ToggleCoroutine());
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Index Fingertip") isClicking = false;
    }

    public void Toggle(bool state)
    {
        //Debug.Log(state);
        checkmark.SetActive(state);
        this.state = state;
        if(state) onCheck?.Invoke();
        else onUncheck?.Invoke();
    }

    private IEnumerator ToggleCoroutine()
    {
        isClicking = true;
        background.color = clicked;
        yield return new WaitForSeconds(0.25f);
        while(isClicking && uiEnabled) yield return null;
        background.color = idle;
        Toggle(!state);
    }
}
