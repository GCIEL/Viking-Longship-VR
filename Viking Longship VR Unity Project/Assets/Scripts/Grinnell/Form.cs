using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Form : MonoBehaviour
{
    private TogglePanel genderButtons;
    private bool canEdit = true;
    private TextMeshPro tmp;
    private int age = 16;
    private Gender gender = Gender.Male;
    private bool hasSubmitted = false;
    public UnityEvent onSubmit;
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public void SetGenderMale() { gender = Gender.Male; UpdateText(); }
    public void SetGenderFemale() { gender = Gender.Female; UpdateText(); }
    public void SetGenderOther() { gender = Gender.Other; UpdateText(); }
    public void Submit()
    {
        if (!hasSubmitted) StartCoroutine(DescendCoroutine());
        hasSubmitted = true;
    }
    private IEnumerator DescendCoroutine()
    {
        Vector3 startPos = transform.position, endPos = transform.position - Vector3.up * 2.5f;
        float timer = 0;
        while(timer < 3f)
        {
            transform.position = Vector3.Lerp(startPos, endPos,timer / 3f);
            yield return null;
            timer += Time.deltaTime;
        }
        onSubmit?.Invoke();
    }
    public void IncrementAge()
    {
        if(canEdit)
        {
            age++;
            age = Mathf.Clamp(age, 1, 99);
            UpdateText();
            StartCoroutine(FormUpdateDelay());
        }
    }

    public void DecrementAge()
    {
        if(canEdit)
        {
            age--;
            age = Mathf.Clamp(age, 1, 99);
            UpdateText();
            StartCoroutine(FormUpdateDelay());
        }
    }

    private IEnumerator FormUpdateDelay()
    {
        canEdit = false;
        yield return new WaitForSeconds(0.5f);
        canEdit = true;
    }

    private void UpdateText()
    {
        tmp.text = "Age: " + age + "\n\n" +
                   "Gender: " + gender;
    }

    public void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        UpdateText();
        genderButtons = GetComponentInChildren<TogglePanel>();
        genderButtons.Lock(0);
    }
}
