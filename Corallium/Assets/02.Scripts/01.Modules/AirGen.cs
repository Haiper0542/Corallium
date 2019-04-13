using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirGen : MonoBehaviour {

    public LineRenderer line;
    public Light lightPre;
    public Transform AirSource;
    Vector3 AirOrigin;

    public bool isGen;

    private void Start()
    {
        lightPre = transform.GetChild(0).GetComponent<Light>();
        line = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(AirSource != null && (AirSource.name == "AirTank" || AirSource.GetComponent<AirGen>().isGen))
        {
            lightPre.enabled = true;
            if(AirSource.name == "AirTank")
            {
                AirOrigin = AirSource.position - lightPre.transform.position;
                line.SetPosition(1, AirOrigin);
            }
            else
            {
                AirOrigin = AirSource.GetChild(1).position - lightPre.transform.position;
                line.SetPosition(1, AirOrigin);
            }
            isGen = true;
        }
        else
        {
            lightPre.enabled = false;
            AirOrigin = Vector3.zero;
            line.SetPosition(1, AirOrigin);
            isGen = false;
        }
    }
}
