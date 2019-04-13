using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTilt : MonoBehaviour
{
    private Gyroscope gyroinfo;
    public GameObject bubbleParticle;

    void Start()
    {
        Input.gyro.enabled = true;
        InvokeRepeating("gyroupdate", 0, 0.05f);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //bubbleParticle.SetActive(true);
        }
        else
        {
            //bubbleParticle.SetActive(false);
        }
    }
    
    void gyroupdate()
    {
        Quaternion transquat = Quaternion.identity;

        gyroinfo = Input.gyro;

        transquat.w = gyroinfo.attitude.w;

        transquat.x = -gyroinfo.attitude.x;
        transquat.y = -gyroinfo.attitude.y;
        transquat.z = gyroinfo.attitude.z;

        transform.rotation = Quaternion.Euler(90, 0, 0) * transquat;
    }
}
