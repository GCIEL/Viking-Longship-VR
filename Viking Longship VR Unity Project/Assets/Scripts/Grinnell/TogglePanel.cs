using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class TogglePanel : MonoBehaviour
{
    public UnityEvent[] toggleEvents;
    private HoverButton[] toggleButtons;
    // Start is called before the first frame update
    void Awake()
    {
        toggleButtons = GetComponentsInChildren<HoverButton>();
        for (int i = 0; i < toggleButtons.Length; i++)
        {
            int tempIndex = i;
            toggleButtons[i].onButtonDown.AddListener(delegate{Toggle(tempIndex);});
        }
    }

    public void Toggle(int toggleIndex)
    {
        toggleButtons[toggleIndex].Lock();
        for(int i = 0; i < toggleButtons.Length; i++)
        {
            if (i == toggleIndex) continue;
            toggleButtons[i].locked = false;
        }
        toggleEvents[toggleIndex].Invoke();
    }

    public void Lock(int toggleIndex)
    {
        toggleButtons[toggleIndex].Lock();
        for(int i = 0; i < toggleButtons.Length; i++)
        {
            if (i == toggleIndex) continue;
            toggleButtons[i].locked = false;
        }
    }
}
