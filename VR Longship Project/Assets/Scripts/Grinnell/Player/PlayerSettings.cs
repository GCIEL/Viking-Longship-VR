using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Intented to be a singleton object that stores the settings the player chooses
[CreateAssetMenu(menuName = "Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public bool leftHandedControls;

}
