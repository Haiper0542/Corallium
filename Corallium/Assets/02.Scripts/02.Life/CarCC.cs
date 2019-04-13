using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCC : MonoBehaviour {

    Vector3 moveDirection;
    private float gravity = 50;

    CharacterController cc;

    public bool isRidden = false;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isRidden)
        {
            cc.center = new Vector3(0, 0.2f, 0);
        }

        moveDirection = Vector3.zero;
        moveDirection.y -= gravity * Time.deltaTime;
        cc.Move(moveDirection * Time.deltaTime);
    }
}
