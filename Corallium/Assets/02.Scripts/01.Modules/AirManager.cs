using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirManager : MonoBehaviour {

    public Transform AirOrigin;
    public List<AirGen> AirGenList = new List<AirGen>();
    public float MaxAirRange;

    public static AirManager instance;

    private void Awake()
    {
        instance = GetComponent<AirManager>();
        InvokeRepeating("AirGenUpdate", 0, 0.02f);
    }
    
    public void AirGen_Add(AirGen newAirGen)
    {
        AirGenList.Add(newAirGen);
        AirGen_Update();
    }

    public void AirGen_Update()
    {
        AirGenList.Sort(delegate (AirGen t1, AirGen t2)
        {
            return Vector3.Distance(t1.transform.position, AirOrigin.position).CompareTo(Vector3.Distance(t2.transform.position, AirOrigin.position));
        });
    }

    public void AirGenUpdate()
    {
        AirGen_Update();

        foreach (AirGen air1 in AirGenList)
        {
            if (air1.isGen)
            {
                if(air1.AirSource != null && Vector3.Distance(air1.transform.position, air1.AirSource.position) < MaxAirRange)
                    continue;
            }

            if (Vector3.Distance(air1.transform.position, AirOrigin.position) < MaxAirRange)
            {
                air1.AirSource = AirOrigin;
                continue;
            }

            bool flag = false;
            foreach (AirGen air2 in AirGenList)
            {
                if (air1 != air2)
                {
                    if (air2.isGen && Vector3.Distance(air1.transform.position, air2.transform.position) < MaxAirRange)
                    {
                        air1.AirSource = air2.transform;
                        flag = true;
                    }
                }
            }
            if (!flag)
                air1.AirSource = null;
        }
    }
}