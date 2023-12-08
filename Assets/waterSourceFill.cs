using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSourceFill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject water)
    {

        if (water.name == "waterFill")
        {
            GameObject ourWater = GameObject.Find("waterFiller");

            Vector3 scaleDec = new Vector3(0, 0.5f, 0);
            ourWater.transform.localScale += scaleDec;

            Debug.Log("Success!");
        }
    }

    /*
    private void OnParticleCollision(GameObject collider)
    {
        if (collider.name != "waterFill") 
        {
            Debug.Log(collider.name);
            return;
        }


        Debug.Log("successfully reaching");
        
        
    }
    */
}
