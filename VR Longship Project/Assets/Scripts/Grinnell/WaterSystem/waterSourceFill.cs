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

    // this function determines whether the cup's particles is touching the source of water, and increments the size of the source of water
    // this script is unused because we have no reason to fill up a water source for this project
    private void OnParticleCollision(GameObject water)
    {

        if (water.name == "waterFill")
        {
            GameObject ourWater = GameObject.Find("waterFiller");

            Vector3 scaleDec = new Vector3(0, 0.5f, 0);
            ourWater.transform.localScale += scaleDec;
        }
    }
}
