using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCtrl : MonoBehaviour
{
    public CharacterController cc;

    public float moveSpeed = 5f;
    public float turnSpeed = 540f;

    bool isMoveState = false;
    public Camera MainCam;

    Vector3 hitPosition;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isMoveState)
        {
            Vector3 dir = hitPosition - transform.position;
            Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
            if (dirXZ != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dirXZ);
                Quaternion frameRot = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);

                transform.rotation = frameRot;
            }

            Vector3 targetPos = transform.position + dirXZ;

            Vector3 framePos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            Vector3 moveDir = (framePos - transform.position) + Physics.gravity;

            cc.Move(moveDir);

            if (framePos == targetPos)
            {
                isMoveState = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                hitPosition = hitInfo.point;
                isMoveState = true;
            }
        }

    }
}