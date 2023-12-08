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


    void OnTriggerEnter(Collider collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Cylinder")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            //Debug.Log("Cylinder found!");

            GameObject cupWater = collision.gameObject.transform.Find("waterScaler").gameObject;

            //Debug.Log("111we have found the cupWater to be: " + cupWater.transform.localScale.y);


            if(cupWater.transform.localScale.y < 10f)
            {
                Vector3 cupScale = new Vector3(0, 1f, 0);
                cupWater.transform.localScale += cupScale;

                Vector3 boxWatScale = new Vector3(0, 0.01f, 0);
                boxWater.transform.localScale -= boxWatScale;

                script = collision.gameObject.GetComponent<activateDroplets>();

                script.addWater();

            }


            //Debug.Log("222we have found the cupWater to be: " + cupWater.transform.localScale.y);

            //

        }
    }

    

}
