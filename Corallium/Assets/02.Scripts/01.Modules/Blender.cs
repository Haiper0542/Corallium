using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blender : MonoBehaviour {

    public int soilCount = 0;
    public int SuccesId = 0;
    public int MatId = 0;
    public Image Timer;
    public Text TimeText;
    private float BlendTime = 0;
    public float SuccessTime = 25;

    public bool isBlending = false;
    private bool set = false;

    public Transform cover;
    public Transform cube;
    public Transform edge;

    public GameObject PannelOff;
    public GameObject PannelOn;

    private UIManager uiManager;

    private void Start()
    {
        StartCoroutine("CoverOpenAni");
    }

    void Update () {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if(isBlending && BlendTime < SuccessTime)
        {
            PannelOff.SetActive(false);
            PannelOn.SetActive(true);
            edge.Rotate(0, 0, 5);
            BlendTime += Time.deltaTime;
            Timer.fillAmount = BlendTime / SuccessTime;
            TimeText.text = (BlendTime / SuccessTime * 100).ToString("F0") + "%";
        }
        else if(set)
        {
            StartCoroutine("CoverOpenAni");

            set = false;
            SuccesId = Random.Range(52, 54);
            uiManager.inventory.InvenId[22] = SuccesId;
            uiManager.inventory.InvenCount[22] = 1;
            uiManager.inventory.SlotUpdate(22);
        }
	}
    public void BlendStart()
    {
        if (!isBlending)
        {
            StartCoroutine("CoverCloseAni");

            isBlending = true;
            set = true;

            MatId = 0;
            soilCount--;
            uiManager.inventory.InvenCount[21] = soilCount;
            if (soilCount == 0)
                uiManager.inventory.InvenId[21] = 0;

            uiManager.inventory.InvenId[20] = 0;
            uiManager.inventory.InvenCount[20] = 0;
            uiManager.inventory.SlotUpdate(20);
            uiManager.inventory.SlotUpdate(21);

            BlendTime = 0;
        }
    }

    public void BlendReset()
    {
        isBlending = false;
        Timer.fillAmount = 0;
        TimeText.text = "";
        SuccesId = 0;
    }

    IEnumerator CoverOpenAni()
    {
        PannelOff.SetActive(true);
        PannelOn.SetActive(false);
        for (int i = 0; i < 15; i++)
        {
            cover.Translate(0, 0.04f, 0);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 7; i++)
        {
            cover.Rotate(0, 6, 0);
            cube.Rotate(0, 6, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator CoverCloseAni()
    {
        for (int i = 0; i < 7; i++)
        {
            cover.Rotate(0, -6, 0);
            cube.Rotate(0, -6, 0);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 15; i++)
        {
            cover.Translate(0, -0.04f, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
