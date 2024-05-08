using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class waterSourceEmpty : MonoBehaviour
{

    private GameObject boxWater;
    // Start is called before the first frame update
    void Start()
    {
        boxWater = GameObject.Find("waterFiller");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private activateDroplets script;

    // this function determines to fill up an empty water source, such as our cup
    void OnTriggerStay(Collider collision)
    {
        // This checks to see if the cup is colliding with the water source
        if (collision.gameObject.name == "Throwable cup")
        {
            // assign variable to water subobject
            GameObject cupWater = collision.gameObject.transform.Find("waterScaler").gameObject;

            // scale water subobject up to 0.9f before stopping
            if(cupWater.transform.localScale.y < 0.9f)
            {
                // scale the water
                Vector3 cupScale = new Vector3(0, 0.01f, 0);
                cupWater.transform.localScale += cupScale;

                // call upon our activateDroplets script to change cup object logic
                script = collision.gameObject.GetComponent<activateDroplets>();
                script.addWater();

            }

        }
    }

    

}
