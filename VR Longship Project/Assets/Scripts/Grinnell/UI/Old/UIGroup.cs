using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIGroup : MonoBehaviour
{
    private Image[] images;
    private TextMeshProUGUI[] tmps;
    private float[] imgsMaxAlphas;

    public void SetText(string text, int tmpInd) { tmps[tmpInd].text = text; }

    void Start()
    {
        images = GetComponentsInChildren<Image>(true);
        tmps = GetComponentsInChildren<TextMeshProUGUI>(true);
        imgsMaxAlphas = new float[images.Length];
        for(int i = 0; i < images.Length; i++) imgsMaxAlphas[i] = images[i].color.a;
    }

    public void LerpAlpha(float amount)
    {
        amount = Mathf.Clamp(amount, 0f, 1f);
        foreach(TextMeshProUGUI tmp in tmps) tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, amount);
        for(int i = 0; i < images.Length; i++){
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, amount * imgsMaxAlphas[i]);
        } 
    }

    public IEnumerator FadeOut(float time)
    {
        float timer = 0;
        while(timer < time)
        {
            LerpAlpha(1 - timer/time);
            yield return null;
            timer += Time.deltaTime;
        }
        LerpAlpha(0);
    }
}
