using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCircle : MonoBehaviour {

    public Transform[] Cubes = new Transform[9];

	void Awake () {
        for(int i = 0; i < 9; i++)
            Cubes[i] = transform.GetChild(i);
    }

    public void Resize(Vector3 center, float Scale)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < 9; i++)
        {
            Cubes[i].position = center;
            Cubes[i].Translate(Vector3.left * Scale * 0.7f);
            Cubes[i].localScale = new Vector3(0.1f, 0.1f, 0.3f * Scale);
        }
    }
}
