using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public float speed = 0.001f;
    float rotSpeed = 5.0f;
    public bool isJelly = false;

    public float minSpeed = 0.2f;
    public float maxSpeed = 0.8f;

    Vector3 averHeading;
    Vector3 averPos;

    float neighborDist = 3.0f;
    public Vector3 newGoalPos;

    public bool turning = false;

    public float turningTime = 0.0f;

    Animator animator;

    void Start () {
        speed = Random.Range(minSpeed, maxSpeed);
        animator = GetComponent<Animator>();
        if(!isJelly)
            animator.speed = speed;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (!turning)
            {
                newGoalPos = transform.position - other.gameObject.transform.position;
            }
            turning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            turning = false;
        }
    }

    void Update()
    {
        if (turning)
        {
            turningTime += Time.deltaTime;
            if (turningTime > 2.0f)
            {
                transform.LookAt(new Vector3(Random.Range(-5, 5), 5, Random.Range(-5, 5)));
                int size = 5;
                //위치 재설정
                newGoalPos = new Vector3(Random.Range(-size, size),
            Random.Range(0, size * 2),
            Random.Range(-size, size));
            }

            //중심부를 향하게
            Vector3 dir = newGoalPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
                rotSpeed * Time.deltaTime);
            speed = Random.Range(minSpeed, maxSpeed);
            if (!isJelly)
                animator.speed = speed;
        }
        else
        {
            turningTime = 0;
            if (Random.Range(0, 10) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, speed * Time.deltaTime);

        if(transform.position.y < -20 || transform.position.y > 40
            || transform.position.x > 40 || transform.position.x < -40
            || transform.position.z > 40 || transform.position.z < -40
            )
        {
            transform.LookAt(Vector3.up * 5);
        }
	}

    void ApplyRules()
    {
        GameObject[] gos;
        gos = FishManager.instance.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = FishManager.goalPos;

        float dist;

        int groupSize = 0;
        foreach(GameObject go in gos)
        {
            if(go != gameObject) //자신 제외
            {
                dist = Vector3.Distance(go.transform.position, transform.position);
                if(dist <= neighborDist)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(dist < 2.0f)
                    {
                        vavoid = vavoid + (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            //중심 계산
            vcentre = vcentre / groupSize + (goalPos - transform.position);
            //평균 속도 계산 -> 속도 맞추기
            speed = gSpeed / groupSize;
            if (!isJelly)
                animator.speed = speed;

            Vector3 dir = (vcentre + vavoid) - transform.position;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }
    }
}
