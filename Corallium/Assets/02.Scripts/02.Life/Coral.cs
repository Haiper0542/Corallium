using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral : MonoBehaviour {

    public int index;
    public GameObject[] CoralPrefab = new GameObject[3];
    public float GrowSpeed = 1;
    public int[] next = new int[2];
    public bool isGrow = true;

	void Start () {
        transform.Rotate(0, Random.Range(0, 360), 0);
        if (isGrow)
        {
            for (int i = 0; i < 2; i++)
            {
                CoralPrefab[i] = transform.GetChild(i).gameObject;
            }
            StartCoroutine("NextCoral");
        }
	}

    IEnumerator NextCoral()
    {
        yield return new WaitForSeconds(next[index] * GrowSpeed);
        CoralPrefab[index].SetActive(false);
        index++;
        if(CoralPrefab[index] != null)
            CoralPrefab[index].SetActive(true);

        if (index > 1)
            yield return null;
        else
            StartCoroutine("NextCoral");
    }
}
