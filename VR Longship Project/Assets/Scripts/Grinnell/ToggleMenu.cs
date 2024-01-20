using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    public InputActionReference toggleReference = null;

    private void Awake()
    {
        toggleReference.action.started += Toggle;
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }
}
