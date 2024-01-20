using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class PresentationScreen : MonoBehaviour
{
    public Texture defaultTex;
    private TextMeshPro label;

    void Start()
    {
        label = GetComponentInChildren<TextMeshPro>();
    }

    public void UpdateDescription(string newDesc)
    {
        label.text = newDesc;
    }

    public void PlayVideo()
    {
        label.enabled = false;
        GetComponent<VideoPlayer>().Play();
    }

    public void ShowDescription()
    {
        GetComponent<VideoPlayer>().Stop();
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", defaultTex);
        label.enabled = true;
    }

}
