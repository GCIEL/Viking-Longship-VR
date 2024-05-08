using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;

public class VRUISlider : MonoBehaviour
{
    public UnityEvent<float> onValueChanged;
    private RectTransform fill;
    private float maxWidth;
    void Awake()
    {
        fill = transform.GetChild(0).GetComponent<RectTransform>();
        maxWidth = GetComponent<RectTransform>().rect.width;
    }

    public void SetValue(float value)
    {
        fill.sizeDelta = new Vector2(maxWidth*value, fill.rect.height);
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.tag != "Index Fingertip") return;

        Vector3 dir = transform.worldToLocalMatrix * (collider.transform.position - transform.position);
        float fillValue = Mathf.Clamp(dir.x + maxWidth/2f, 0, maxWidth);
        fill.sizeDelta = new Vector2(fillValue, fill.rect.height);
        onValueChanged?.Invoke(fillValue/maxWidth);
    }
}
