using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public void UpdateValue() { GetComponent<TextMeshProUGUI>().text = transform.parent.GetComponent<Slider>().value.ToString(); }
}
