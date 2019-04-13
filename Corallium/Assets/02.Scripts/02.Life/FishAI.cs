using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour {

    public float speed = 0.8f;
    public float middleY = 3;
    public float ocha = 0.5f;
    public float rotSpeed = 10.0f;

    [Space]
    [Space]
    public float closeness = 0;
    float maxCloseness = 100;

    [Space]
    [Space]
    public Vector3 goalPos;
    Vector3 tempPos;

    public bool isTamed = false;
    public int[] favor = new int[2];

    public GameObject tamedParticle;
    public GameObject feedParticle;
    public GameObject niceParticle;

    public Transform player;

    public LayerMask layerMask;

    void Start ()
    {
        player = GameObject.Find("Player").transform;
        if (speed > 0.1f)
        {
            goalPos = transform.position + new Vector3(Random.Range(10, 20), 0, Random.Range(10, 20));
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + middleY, transform.position.z);
            }
        }
        favor[0] = Random.Range(1, 7);
        favor[1] = Random.Range(1, 7);
    }

    void Update()
    {
        if (isTamed)
        {
            //goalPos = Quaternion.LookRotation(player.position) * Vector3.forward;
            //goalPos = goalPos.normalized;
            goalPos = player.position;
            closeness -= Time.deltaTime * 0.5f;
            if (closeness <= 0)
            {
                closeness -= 0;
                isTamed = false;
            }
        }

        if (speed > 0.1f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.distance > middleY + ocha)
                    transform.Rotate(0.6f, 0, 0);
                else if (hit.distance < middleY - ocha)
                    transform.Rotate(-0.6f, 0, 0);
                else if (hit.distance > middleY)
                    transform.Rotate(0.1f, 0, 0);
                else if (hit.distance < middleY)
                    transform.Rotate(-0.1f, 0, 0);
            }
            if(Vector3.Distance(transform.position,goalPos) > 2.5f)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, layerMask))
                goalPos = transform.position + new Vector3(Random.Range(10, 20), 0, Random.Range(10, 20));
        }
        transform.LookAt(goalPos);
    }

    public void Feeding(int gift)
    {
        if (gift == favor[0] || gift == favor[1])
        {
            closeness += 50;
            GameObject newPart = Instantiate(niceParticle, transform.position, Quaternion.identity);
            Destroy(newPart, 3);
        }
        else
        {
            closeness += 25;
            //GameObject newPart = Instantiate(feedParticle, transform.position, Quaternion.identity);
            //Destroy(newPart, 3);
        }

        if (closeness >= maxCloseness)
        {
            isTamed = true;
            QuestImage.instance.TameFish++;
            GameObject newPart = Instantiate(tamedParticle, transform.position, Quaternion.identity);
            Destroy(newPart, 3);
        }
    }
}
