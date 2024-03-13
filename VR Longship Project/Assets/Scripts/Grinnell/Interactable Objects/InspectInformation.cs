using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inspect Information")]
public class InspectInformation : ScriptableObject
{
    public AudioClip audio;
    public Image image;
    public string text;
}
