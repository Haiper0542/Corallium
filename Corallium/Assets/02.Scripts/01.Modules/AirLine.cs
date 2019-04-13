using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLine : MonoBehaviour {

    public Transform AirTank;
    public int MaxAirRange = 2;
    Vector3 AirOrigin;
    LineRenderer line;

    public PlayerAct2 playerAct2;
    public Transform AirF;

    void Start () {
        line = GetComponent<LineRenderer>();
        InvokeRepeating("AirFound", 0, 0.01f);
    }

	void Update ()
    {
        transform.position = AirF.position;

        if (AirTank != null)
        {
            playerAct2.charging = true;
            line.SetPosition(0, AirF.position);
            line.SetPosition(1, AirTank.position);
            line.enabled = true;
        }
        else
        {
            playerAct2.charging = false;
            line.enabled = false;
        }
    }

    public void AirFound()
    {
        GameObject[] AirList_ = GameObject.FindGameObjectsWithTag("AirTank");
        List<GameObject> AirList = new List<GameObject>();
        for(int i = 0; i < AirList_.Length; i++)
        {
            if (AirList_[i].name == "AirTank" || AirList_[i].GetComponent<AirGen>().isGen)
            {
                AirList.Add(AirList_[i]);
            }
        }

        GameObject AirPos = AirList[0];
        float dist = Vector3.Distance(transform.position, AirList[0].transform.position);
        foreach (GameObject obj in AirList)
        {
            float dist_ = Vector3.Distance(transform.position, obj.transform.position);
            if (dist_ < MaxAirRange && dist > dist_)
            {
                dist = dist_;
                AirPos = obj;
            }
        }
        if (dist <= MaxAirRange)
            AirTank = AirPos.transform.GetChild(0);
        else
            AirTank = null;
    }
}
