using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public GameScenes sceneToLoad = GameScenes.none;
    public UnityEvent onEnter;
    private bool isReached = false;

    public enum GameScenes
    {
        none = -1,
        Tutorial = 0,
        ShipBuilding1To10 = 1,
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isReached && other.tag == "Player")
        {
            onEnter?.Invoke();
            if (sceneToLoad != GameScenes.none) SceneManager.LoadScene((int)sceneToLoad);
            StartCoroutine(VanishCoroutine());
        }
    }

    private IEnumerator VanishCoroutine()
    {
        isReached = true;
        float timer = 0;
        while(timer < 2f)
        {
            GetComponent<MeshRenderer>().material.SetFloat("_Intensity", 1f - timer/2f);
            yield return null;
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
