using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickMove : MonoBehaviour
{
    [Header("JoyStick")]
    public Transform JoyStick;
    public Transform JoyStickBack;
    Vector3 FirstPos;  // 조이스틱의 처음 위치
    Vector2 JoyVec;

    float Radius = 65;           // 조이스틱 배경의 반 지름
    int dead = 15;

    public bool leftHand = false;
    bool JoyOn = false;

    [Header("Player")]
    public Transform Player;
    public float Speed = 1;
    public float RotSpeed = 5;
    public float gravity = 20.0f;
    Quaternion newRot;
    Vector3 moveDirection;
    public Transform dronePos;

    public CharacterController cc;

    int JoyCount = 0;

    float JoyRange = 0.35f;

    [Header("Camera")]
    public Transform target;
    public float xSpeed = 220.0f;
    public float ySpeed = 110.0f;
    float x = 0.0f;
    float y = 0.0f;
    public float yMin = -80f;
    public float yMax = 80f;

    public float dist = 0;

    Quaternion rotation;
    Vector3 position;

    public Camera Maincam;

    public float nowDist = 10;
    public float aimDist = 10;

    public LayerMask layerMask;

    public bool camOn = true;
    public bool on = true;
    public bool isfixed = true;

    float accel = 0;
    float MaxSteer = 2;

    public PlayerAct2 playerAct2;
    public static ClickMove instance;
    public Animator animator;
    public Animator dAnimator;

    void Awake()
    {
        if (ClickMove.instance == null)
            ClickMove.instance = this;
        cc = Player.GetComponent<CharacterController>();
        
        target.transform.rotation = rotation;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void Update()
    {
        //UncoverView();
        JoyCount = 0;

        foreach (Touch touch in Input.touches)
        {
            if (on && !UIManager.instance.isPause && JoyCount == 0)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if ((!leftHand && touch.position.x < Screen.width * JoyRange) || (leftHand && touch.position.x > Screen.width * (1 - JoyRange)))
                        {
                            accel = 0;
                            FirstPos = touch.position;
                            JoyStickBack.position = FirstPos;
                            JoyStickBack.gameObject.SetActive(true);
                            JoyOn = true;
                        }
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        accel = 0;
                        JoyStick.position = Vector2.zero;
                        JoyVec = Vector3.zero;
                        JoyStickBack.gameObject.SetActive(false);
                        JoyOn = false;
                        animator.SetBool("isWalking", false);
                        dAnimator.SetBool("isWalking", false);
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (JoyOn)
                        {
                            Vector3 Pos = touch.position;
                            JoyVec = (Pos - FirstPos).normalized;
                            float Dist = Vector3.Distance(Pos, FirstPos);

                            if (Dist < Radius)
                                JoyStick.position = new Vector2(FirstPos.x, FirstPos.y) + JoyVec * Dist;
                            // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
                            else
                                JoyStick.position = new Vector2(FirstPos.x, FirstPos.y) + JoyVec * Radius;

                            newRot = Quaternion.Euler(new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0));

                            if (playerAct2.isRidden)
                                playerAct2.RideObj.transform.rotation = newRot;
                            else
                                Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, newRot, RotSpeed * Time.deltaTime);


                            if (Dist > dead)
                            {
                                dAnimator.SetBool("isWalking", true);
                                animator.SetBool("isWalking", true);
                                accel = Mathf.Clamp(accel + 0.01f, 0, MaxSteer);
                                moveDirection = newRot * Vector3.forward * Time.deltaTime * Speed;
                                if (playerAct2.isRidden)
                                {
                                    if (cc.isGrounded)
                                        playerAct2.RideObj.Move(moveDirection * accel);
                                    else
                                        playerAct2.RideObj.Move(moveDirection * accel * 0.2f);
                                }
                                else
                                {
                                    if (cc.isGrounded)
                                        cc.Move(moveDirection);
                                    else
                                        cc.Move(moveDirection * 0.2f);
                                }
                            }
                            else
                                accel = 0;
                        }
                        break;
                }
            }
            JoyCount++;
        }

        if(JoyCount == 0)
        {
            JoyStick.position = Vector2.zero;
            JoyVec = Vector3.zero;
            JoyStickBack.gameObject.SetActive(false);
        }
        if (on)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * Speed;
            if (playerAct2.isRidden)
            {
                if (cc.isGrounded)
                    playerAct2.RideObj.Move(moveDirection * accel);
                else
                    playerAct2.RideObj.Move(moveDirection * accel * 0.2f);
            }
            else
            {
                if (cc.isGrounded)
                    cc.Move(moveDirection);
                else
                    cc.Move(moveDirection * 0.2f);
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        if (playerAct2.isRidden)
            playerAct2.RideObj.Move(moveDirection * Time.deltaTime);
        else
            cc.Move(moveDirection * Time.deltaTime);
    }

    void LateUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            if (camOn && !UIManager.instance.isPause && touch.position.x >= Screen.width * JoyRange)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        x += touch.deltaPosition.x * xSpeed * 0.03f;
                        y -= touch.deltaPosition.y * ySpeed * 0.03f;

                        y = ClampAngle(y, yMin, yMax);

                        if (isfixed)
                            rotation = Quaternion.Euler(y, 0, 0);
                        else
                        //rotation = Quaternion.Euler(y, x, 0);

                        if (playerAct2.isRidden)
                            playerAct2.RideObj.transform.rotation = rotation;
                        else
                            target.transform.rotation = rotation;
                        //UncoverView();
                        break;
                }
            }
        }
        //position = rotation * new Vector3(0, 1.5f, -nowDist) + Player.position;
        if (isfixed)
        {
            Maincam.transform.position = Player.position + new Vector3(0, 8, -7);
            Maincam.transform.eulerAngles = new Vector3(40, 0, 0);
        }
    }

    public void FeedStart()
    {
        isfixed = false;
        animator.SetBool("isFeeding", true);
        StartCoroutine("FeedStartAni");
    }
    public void FeedEnd()
    {
        isfixed = true;
        animator.SetBool("isFeeding", false);
        StartCoroutine("FeedEndAni");
    }

    IEnumerator FeedStartAni()
    {
        for (int i = 0; i < 10; i++)
        {
            Maincam.transform.position += new Vector3(0.5f, -0.3f, 0.55f);
            Maincam.transform.eulerAngles += new Vector3(-0.5f, -5f, 0);
            //dronePos.position += new Vector3(-0.29f, 0, -0.2f);
            yield return new WaitForSeconds(0.002f);
        }
    }
    IEnumerator FeedEndAni()
    {
        for (int i = 0; i < 10; i++)
        {
            Maincam.transform.position -= new Vector3(0.5f, -0.3f, 0.55f);
            Maincam.transform.eulerAngles -= new Vector3(-0.5f, -5f, 0);
            //dronePos.position -= new Vector3(-0.29f, 0, -0.2f);
            yield return new WaitForSeconds(0.002f);
        }
    }

    void UncoverView()
    {
        RaycastHit hit;
        if (Physics.Raycast(Player.position + new Vector3(0, 1.5f, 0), -target.forward, out hit, 10, layerMask))
        {
            aimDist = Vector3.Distance(Player.position, hit.point);
            if (aimDist < 0.75f)
                aimDist = 0.75f;
        }
        else if (aimDist < 10)
            aimDist = 10;

        if (Mathf.Abs(nowDist - aimDist) > 0.5f)
        {
            if (nowDist > aimDist)
                nowDist = aimDist + 0.5f;
            else
                nowDist = aimDist - 0.5f;
        }
        if (nowDist > aimDist + 0.3f)
            nowDist -= 0.2f;
        else if (nowDist < aimDist - 0.3f)
            nowDist += 0.2f;
        else
            nowDist = aimDist;
    }
}
