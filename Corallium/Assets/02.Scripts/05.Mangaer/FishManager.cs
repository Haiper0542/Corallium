using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

    public GameObject[] fishPre = new GameObject[7];
    public int[] count = new int[7];
    public static int tankSize = 20;

    int numFish;
    int index;
    public GameObject[] allFish;
    
    public static Vector3 goalPos = Vector3.zero;

    public static FishManager instance;

    public void Awake()
    {
        instance = GetComponent<FishManager>();
    }

    private void Start()
    {
        foreach (int i in count)
            numFish += i;

        allFish = new GameObject[numFish];

        for (int i = 0; i <8; i++)
        {
            for (int j = 0; j < count[i]; j++)
            {
                Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize),
                    Random.Range(0, tankSize),
                    Random.Range(-tankSize, tankSize));
                allFish[index++] = Instantiate(fishPre[i], pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
    }

    private void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-tankSize, tankSize),
                Random.Range(0, tankSize),
                Random.Range(-tankSize, tankSize));
        }
    }
}
