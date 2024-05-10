
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleMenu : MonoBehaviour
{
    // Reference to the action you created
    public InputActionReference toggleReference = null;

    private void Awake()
    {
        // Adding the toggle function as a listener for this action
        // So the toggle function is called whevener the action happens
        toggleReference.action.started += Toggle;
    }

    // Any function you want to hook up to your input should have an 
    // input like this (InputAction.CallbackContext ctx)
    private void Toggle(InputAction.CallbackContext ctx)
    {
        bool isEnabled = transform.GetChild(0).gameObject.activeSelf;
        transform.GetChild(0).gameObject.SetActive(!isEnabled);
    }
}

