using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateLiquid : MonoBehaviour
{
    private GameObject mLiquid;

    private int mSloshSpeed = 60;
    private int mRotateSpeed = 15;

    private int difference = 25;

    // Start is called before the first frame update
    void Start()
    {
        mLiquid = GameObject.Find("Liquid");
        
    }

    // Update is called once per frame
    void Update()
    {
        slosh();

        mLiquid.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime, Space.Self);
    }

    private void slosh() 
    {
        Quaternion inverseRotation = Quaternion.Inverse(transform.localRotation);

        Vector3 finalRotation = Quaternion.RotateTowards(mLiquid.transform.localRotation, inverseRotation, mSloshSpeed * Time.deltaTime).eulerAngles;

        finalRotation.x = ClampRotationValue(finalRotation.x, difference);
        finalRotation.z = ClampRotationValue(finalRotation.z, difference);

        mLiquid.transform.localEulerAngles = finalRotation;
    }

    private float ClampRotationValue(float value, float difference) 
    {
        float ret;

        if (value > 180) 
        {
            ret = Mathf.Clamp(value, 360 - difference, 360);
        }
        else
        {
            ret = Mathf.Clamp(value, 0, difference);
        }
        return ret;
        
    }
}
