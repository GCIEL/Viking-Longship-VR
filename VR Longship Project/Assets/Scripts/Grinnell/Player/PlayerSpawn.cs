using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This used to make sure only one player was in the scene, but I havent changed it from using steamVR yet
*/
public class PlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        Player[] players = (Player[]) GameObject.FindObjectsOfType(typeof(Player), true);
        for(int i = 1; i < players.Length; i++) players[i].gameObject.SetActive(false);
        GameObject player = players[0].gameObject;
        if(player != null) player.GetComponent<PlayerController>().Teleport(transform);
        player.GetComponentInChildren<CurrentSceneUIHookup>().Hookup();*/
    }
}
