using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRUIForm : MonoBehaviour
{
    public UIGroup formUIGroup;
    public TextMeshProUGUI ageTMP, ageErrorMessage, genderErrorMessage;
    public string age, gender;

    public void Submit()
    {
        bool error = false;
        if(age.Length == 0) { ageErrorMessage.gameObject.SetActive(true); error = true; }
        else ageErrorMessage.gameObject.SetActive(false);
        if(gender.Length == 0) { genderErrorMessage.gameObject.SetActive(true); error = true; }
        else genderErrorMessage.gameObject.SetActive(false);
        if(error) return;

        BroadcastMessage("UIToggle", false, SendMessageOptions.DontRequireReceiver);
        StartCoroutine(formUIGroup.FadeOut(2f));
    }

    public void AgeAddDigit(int digit) 
    { 
        if (age.Length == 3 || (age.Length == 0 && digit == 0)) return;
        age += digit;
        ageTMP.text = "Age: " + age; 
    }
    public void AgeDeleteDigit() 
    { 
        if (age.Length == 0) return; 
        age = age.Remove(age.Length - 1);
        ageTMP.text = "Age: " + age; 
    }

    public void SetGender(string newGenter) { gender = newGenter; }
}
