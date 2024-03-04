using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VRUIButton : MonoBehaviour
{
    public UnityEvent onClick;
    public Color idle;
    public Color deselected;
    private Color clicked;
    private Image image;
    private bool isClicking = false;
    private bool uiEnabled = true;

    public void UIToggle(bool state) { uiEnabled = state; }

    void Awake()
    {
        image = GetComponent<Image>();
        clicked = idle + new Color(0.2f, 0.2f, 0.2f, 0f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(isClicking || !uiEnabled) return;
        if(collider.tag == "Index Fingertip") StartCoroutine(Click());
    }

    private IEnumerator Click()
    {
        isClicking = true;
        image.color = clicked;
        yield return new WaitForSeconds(0.25f);
        image.color = idle;
        isClicking = false;
        onClick?.Invoke();
    }

    public void Deselect() { image.color = deselected; }
}
