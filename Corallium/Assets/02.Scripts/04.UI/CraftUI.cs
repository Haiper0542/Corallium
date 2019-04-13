using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [Header("[Craft]")]
    public MatInfo[] MatInfos = new MatInfo[1];
    public Transform MatList;

    public Image[] MatImage = new Image[3];
    public Text[] MatText = new Text[3];

    public GameObject CraftBtns;
    public float CraftTime;
    public Image CraftBtn;
    public int Craftid;

    public bool SIyunMode = false;

    public Inventory inven;

    private void Awake()
    {
        CraftBtns.SetActive(false);
        inven = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void CraftButton()
    {
        bool flag = false;
        CraftBtns.SetActive(true);
        CraftBtn.gameObject.SetActive(true);
        UIManager.instance.nowClicked = GetComponent<CraftUI>();
        switch (MatInfos.Length)
        {
            case 1:
                for(int i = 0; i < 3; i++)
                {
                    MatImage[i].gameObject.SetActive(false);
                    MatText[i].gameObject.SetActive(false);
                }
                MatImage[0].gameObject.SetActive(true);
                MatText[0].gameObject.SetActive(true);
                MatImage[0].transform.localPosition = new Vector3(85, 15, 0);
                MatText[0].transform.localPosition = new Vector3(85, -45, 0);
                MatImage[0].transform.localScale = new Vector3(1.3f, 1.3f, 1);

                MatImage[0].sprite = inven.itemInfo[MatInfos[0].MatId].ItemImg;
                MatText[0].text = MatInfos[0].MatCount + " / " + inven.ItemCount(MatInfos[0].MatId);

                if (MatInfos[0].MatCount > inven.ItemCount(MatInfos[0].MatId))
                    flag = true;
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    MatImage[i].gameObject.SetActive(true);
                    MatText[i].gameObject.SetActive(true);
                }
                MatImage[2].gameObject.SetActive(false);
                MatText[2].gameObject.SetActive(false);
                MatImage[0].transform.localPosition = new Vector3(35, 15, 0);
                MatText[0].transform.localPosition = new Vector3(35, -40, 0);
                MatImage[1].transform.localPosition = new Vector3(145, 15, 0);
                MatText[1].transform.localPosition = new Vector3(145, -40, 0);
                MatImage[0].transform.localScale = new Vector3(1.2f, 1.2f, 1);
                MatImage[1].transform.localScale = new Vector3(1.2f, 1.2f, 1);

                MatImage[0].sprite = inven.itemInfo[MatInfos[0].MatId].ItemImg;
                MatText[0].text = MatInfos[0].MatCount + " / " + inven.ItemCount(MatInfos[0].MatId);
                MatImage[1].sprite = inven.itemInfo[MatInfos[1].MatId].ItemImg;
                MatText[1].text = MatInfos[1].MatCount + " / " + inven.ItemCount(MatInfos[1].MatId);

                if (MatInfos[0].MatCount > inven.ItemCount(MatInfos[0].MatId))
                    flag = true;
                else if (MatInfos[1].MatCount > inven.ItemCount(MatInfos[1].MatId))
                    flag = true;
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {
                    MatImage[i].gameObject.SetActive(true);
                    MatText[i].gameObject.SetActive(true);
                    MatImage[i].transform.localPosition = new Vector3(5 + 85 * i, 15, 0);
                    MatText[i].transform.localPosition = new Vector3(5 + 85 * i, -40, 0);
                    MatImage[i].transform.localScale = new Vector3(1.2f, 1.2f, 1);

                    MatImage[i].sprite = inven.itemInfo[MatInfos[i].MatId].ItemImg;

                    MatText[i].text = MatInfos[i].MatCount + " / " + inven.ItemCount(MatInfos[i].MatId);
                    if (MatInfos[i].MatCount > inven.ItemCount(MatInfos[i].MatId))
                        flag = true;
                }
                break;
        }
        if (!SIyunMode)
        {
            if (flag)
            {
                CraftBtn.color = Color.red;
                CraftBtn.raycastTarget = false;
            }
            else
            {
                CraftBtn.color = Color.white;
                CraftBtn.raycastTarget = true;
            }
        }
        else
        {
            CraftBtn.color = Color.white;
            CraftBtn.raycastTarget = true;
        }
    }

    public void MatReset()
    {
        for (int i = 0; i < 3; i++)
        {
            MatImage[i].gameObject.SetActive(false);
            MatText[i].gameObject.SetActive(false);
            CraftBtn.gameObject.SetActive(false);
        }
        CraftBtns.SetActive(false);
    }
}

[Serializable]
public struct MatInfo
{
    public int MatId;
    public int MatCount;
}
