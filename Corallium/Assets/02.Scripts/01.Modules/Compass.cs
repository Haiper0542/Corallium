using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

    public Transform target;
    Transform tr;

    private void Awake()
    {
        tr = transform;
    }

    void Update ()
    {
        tr.LookAt(new Vector3(target.position.x, tr.position.y, target.position.z));
    }
}
