using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralManager : MonoBehaviour {

    public LayerMask layerMask;
    public GameObject[] CoralPre = new GameObject[10];
    public int CoralPer = 5;

    public GameObject[] OrePre = new GameObject[10];
    public int OrePer = 2;

    public int tankSize = 5;

    private void Start()
    {
        RaycastHit hit;
        for(int i = 0; i < tankSize; i++)
        {
            for(int j = 0; j < tankSize; j++)
            {
                int r = Random.Range(0, 100);
                if (r < CoralPer)
                {
                    if (Physics.Raycast(new Vector3(i,5,j), Vector3.down, out hit, Mathf.Infinity, layerMask))
                    {
                        GameObject newCoral = Instantiate(CoralPre[Random.Range(0, 7)], hit.point, Quaternion.LookRotation(hit.normal));
                        newCoral.transform.Rotate(Vector3.right * 90);
                    }
                }
                else if(r < CoralPer + OrePer)
                {
                    if (Physics.Raycast(new Vector3(i, 5, j), Vector3.down, out hit, Mathf.Infinity, layerMask))
                    {
                        GameObject newCoral = Instantiate(OrePre[Random.Range(0, 7)], hit.point, Quaternion.LookRotation(hit.normal));
                        newCoral.transform.Rotate(Vector3.right * 90);
                    }
                }
            }
        }
    }
}
