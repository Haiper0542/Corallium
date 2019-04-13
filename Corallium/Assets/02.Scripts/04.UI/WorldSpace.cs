using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpace : MonoBehaviour {

    Transform tr;
    Transform mainCam;

	void Start () {
        tr = transform;
        mainCam = GameObject.Find("Main Camera").transform;
    }

	void LateUpdate () {
        tr.LookAt(mainCam.position);
        tr.Rotate(Vector3.up * 180);
	}
}
