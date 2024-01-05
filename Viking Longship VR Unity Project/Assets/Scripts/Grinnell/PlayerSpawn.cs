using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player[] players = (Player[]) GameObject.FindObjectsOfType(typeof(Player), true);
        for(int i = 1; i < players.Length; i++) players[i].gameObject.SetActive(false);
        GameObject player = players[0].gameObject;
        if(player != null) player.GetComponent<PlayerController>().Teleport(transform);
        player.GetComponentInChildren<CurrentSceneUIHookup>().Hookup();
    }
}
