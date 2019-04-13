using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    public int maxResource = 4;
    public int resources = 4;
    public bool canMine = true;

    public float nextGenerateTime = 0.0f;
    public float generateTerm = 20.0f;

    public GameObject[] gems = new GameObject[4];

    private void Update()
    {
        if(resources < maxResource)
        {
            nextGenerateTime += Time.deltaTime;
            if(nextGenerateTime > generateTerm)
            {
                resources++;
                canMine = true;
                gems[resources - 1].SetActive(true);
                nextGenerateTime = 0;
            }
        }
    }

    public void GemActiveFalse()
    {
        for(int i = resources; i < maxResource; i++)
        {
            gems[i].SetActive(false);
        }
        if (resources <= 0)
            canMine = false;
    }
}
