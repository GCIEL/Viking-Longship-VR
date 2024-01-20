using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialUI : MonoBehaviour
{
    private RawImage textBg;
    private TextMeshProUGUI tmp;
    void Start()
    {
        textBg = GetComponent<RawImage>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        //Hide();
    }

    public void ShowMessage(string message)
    {
        textBg.color = new Color(0,0,0,0.6f);
        tmp.text = message;
    }

    public void Hide()
    {
        textBg.color = new Color(0,0,0,0);
        tmp.text = "";
    }
}
