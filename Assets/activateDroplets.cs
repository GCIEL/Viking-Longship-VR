using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateDroplets : MonoBehaviour
{
    // the particle system we want to activate/deactivate
    private ParticleSystem droplets;

    // the water cylinder we want to shrink
    private GameObject water;

    // bool to determine whether the particle system is currently activated or not
    public bool isEmitting;
    
    // this bool determines whether the cup is empty or not
    public bool empty = false;

    // floats to collect the corrent rotational position of object
    float cupAngX;
    float cupAngZ;

    // Start is called before the first frame update
    void Start()
    {
        // assign droplets to the correct game object's particle system
        GameObject particleObject = GameObject.Find("droplets");
        droplets = particleObject.GetComponent<ParticleSystem>();

        water = GameObject.Find("waterScaler");

        // stop any emitting and initialize isEmitting
        droplets.Stop();
        isEmitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // collect the rotational position every frame
        cupAngX = transform.eulerAngles.x;
        cupAngZ = transform.eulerAngles.z;

        // this bool determines whether the object is facing sideways/downward
        bool shouldEmit = ((cupAngX > 90) && (cupAngX < 270)) || 
                           ((cupAngZ > 90) && (cupAngZ < 270));
        
        

        //Debug.Log("water is currently:" + water.transform.localScale.y);

        // it will emit if we are currently not emitting and we should
        // the reason we don't play every frame it should emit is for computational efficiency 
        if (shouldEmit && !empty)
        {
            if (!isEmitting)
            {
                droplets.Play();
                isEmitting = true;
            }

            if (water.transform.localScale.y >= 0.1f)
            {
                Vector3 scaleDec = new Vector3(0, 0.1f, 0);
                water.transform.localScale -= scaleDec;
            }
            else
            {
                Vector3 scaleDec = new Vector3(0, water.transform.localScale.y, 0);
                water.transform.localScale -= scaleDec;

                if (!empty)
                {
                    droplets.Stop();
                    empty = true;

                }
            }
        }
        // it will deactivate particles if we are emitting and need to stop
        else if (!shouldEmit) 
        {
            if (isEmitting) 
            {
                droplets.Stop();
                isEmitting = false;
            }

            // addWater();
        }

    }

    public void addWater() 
    {
        if (water.transform.localScale.y < 10f)
        {
            Vector3 scaleDec = new Vector3(0, 0.1f, 0);
            water.transform.localScale += scaleDec;
            if (empty)
            {
                empty = false;
            }
        }
    }
}
