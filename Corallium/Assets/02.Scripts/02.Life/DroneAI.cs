using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour {
    
    public Transform PlayerTarget;
    public Transform target;
    public float Speed = 2.0f;
    public float rotSpeed = 120f;

    public float minDist = 1.5f;
    public float startDist = 1.5f;
    bool isMoving = true;

    public static DroneAI instance;

    private void Awake()
    {
        instance = GetComponent<DroneAI>();
    }

    private void Update()
    {
        float dis = Vector3.Distance(transform.position, target.position);
        if (dis < 0.01f)
            isMoving = false;
        else if (dis < minDist)
        {
            isMoving = true;
            Speed = 6.0f;
        }
        else if (dis > startDist){
            isMoving = true;
            Speed = 1.5f;
        }

        if (isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.position, target.position, Time.deltaTime * Speed);
            transform.localRotation = target.rotation;
        }
    }
}
