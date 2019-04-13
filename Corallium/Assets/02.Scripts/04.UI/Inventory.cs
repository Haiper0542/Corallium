using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public Transform Player;
    [Header("[Inventory]")]
    public Transform inventory;
    public Transform hotbar;
    public Transform blender;

    public Image[] InvenSlot;
    public Text[] InvenText;
    public int[] InvenId;
    public int[] InvenCount;
    public static int maxInventory = 12;

    public Transform mouseImgTr;
    public Image mouseImg;

    public GameObject[] ExtraSlot;

    [Header("[Item Instruction]")]
    public GameObject itemInstruction;
    public Text itemName;
    public Image itemImg;
    public Text itemExplain;

    [Header("[Selected Item]")]
    public int SelectedSlot = 0;
    public int SelectedHot = 0;
    public Image SelectedCheck;
    public Image SelectedHotCheck;

    [Header("[Item Information]")]
    public ItemInfo[] itemInfo = new ItemInfo[30];

    [Header("[Draging Item")]
    public int DragingId;
    public int DragingCount;
    public int OriginSlot;
    public bool isDrag = false;

    [Header("Icon")]
    public Image LUseIconImg;
    public Image RUseIconImg;

    public GameObject LUseIcon;
    public GameObject RUseIcon;

    public PlayerAct2 playerAct2;

    public LayerMask layerMask;

    void Start ()
    {
        for (int i = 0; i < 12; i++)
        {
            InvenSlot[i] = inventory.GetChild(i + 1).GetChild(1).GetComponent<Image>();
            InvenText[i] = inventory.GetChild(i + 1).GetChild(2).GetComponent<Text>();
        }
        for (int i = 12; i < 14; i++)
        {
            InvenSlot[i] = inventory.GetChild(i + 1).GetChild(1).GetComponent<Image>();
            InvenText[i] = inventory.GetChild(i + 1).GetChild(2).GetComponent<Text>();
        }
        for (int i = 0; i < 4; i++)
        {
            InvenSlot[i + 14] = hotbar.GetChild(i).GetChild(1).GetComponent<Image>();
            InvenText[i + 14] = hotbar.GetChild(i).GetChild(2).GetComponent<Text>();
        }
        for (int i = 20; i < 23; i++)
        {
            InvenSlot[i] = blender.GetChild(i - 18).GetChild(0).GetComponent<Image>();
            InvenText[i] = blender.GetChild(i - 18).GetChild(1).GetComponent<Text>();
        }
        for (int i = 0; i < 21; i++)
        {
            itemInfo[i].maxCount = 9;
        }
        for (int i = 21; i < 32; i++)
        {
            itemInfo[i].maxCount = 1;
        }
        for (int i = 32; i < 65; i++)
        {
            itemInfo[i].maxCount = 9;
        }
    }

    public void Update()
    {
        SelectedHotCheck.transform.position = InvenSlot[SelectedHot + 14].transform.position;
        playerAct2.handId = InvenId[SelectedHot + 14];

        if(InvenId[12] != 0)
        {
            LUseIcon.SetActive(true);
            RUseIcon.SetActive(true);
            LUseIconImg.sprite = itemInfo[InvenId[12]].ItemImg;
            RUseIconImg.sprite = itemInfo[InvenId[12]].ItemImg;
        }
        else
        {
            LUseIcon.SetActive(false);
            RUseIcon.SetActive(false);
        }

        if (DragingId != 0)
            mouseImgTr.position = Input.mousePosition;
        mouseImg.sprite = itemInfo[DragingId].ItemImg;
    }

    public int ItemCount(int id)
    {
        int count = 0;
        for (int i = 0; i < maxInventory; i++)
        {
            if (InvenId[i] == id)
                count+=InvenCount[i];
        }
        for (int i = 14; i < 18; i++)
        {
            if (InvenId[i] == id)
                count += InvenCount[i];
        }
        return count;
    }

    public void SlotUpgrade()
    {
        maxInventory++;
        ExtraSlot[maxInventory - 13].SetActive(true);
    }

    public bool AddSlot(int id, int count)
    {
        bool flag = false;
        for(int i = 14; i < 18; i++)
        {
            if (InvenId[i] == id && InvenCount[i] < itemInfo[id].maxCount)
            {
                if(InvenCount[i] + count <= itemInfo[id].maxCount)
                {
                    InvenCount[i] += count;
                    flag = true;
                    SlotUpdate(i);
                    break;
                }
                else
                {
                    InvenCount[i] = itemInfo[id].maxCount;
                    count = count - (itemInfo[id].maxCount - InvenCount[i]);
                    SlotUpdate(i);
                    break;
                }
            }
        }
        if (!flag)
        {
            for (int i = 0; i < maxInventory; i++)
            {
                if (InvenId[i] == id && InvenCount[i] < itemInfo[id].maxCount)
                {
                    if (InvenCount[i] + count <= itemInfo[id].maxCount)
                    {
                        InvenCount[i] += count;
                        flag = true;
                        SlotUpdate(i);
                        break;
                    }
                    else
                    {
                        InvenCount[i] = itemInfo[id].maxCount;
                        count = count - (itemInfo[id].maxCount - InvenCount[i]);
                        SlotUpdate(i);
                        break;
                    }
                }
            }
        }
        if (!flag)
        {
            for (int i = 14; i < 18; i++)
            {
                if (InvenId[i] == 0)
                {
                    InvenId[i] = id;
                    InvenCount[i] = count;
                    flag = true;
                    SlotUpdate(i);
                    break;
                }
            }
        }
        if (!flag)
        {
            for (int i = 0; i < maxInventory; i++)
            {
                if (InvenId[i] == 0)
                {
                    InvenId[i] = id;
                    InvenCount[i] = count;
                    flag = true;
                    SlotUpdate(i);
                    break;
                }
            }
        }

        if (!flag)
        {
            //for (int i = 0; i < count; i++)
                //Instantiate(itemInfo[id].Item, playerAct2.transform.position, Quaternion.identity);
            UIManager.instance.Alarm("꽉 참!");
            return false;
        }
        else
            return true;
    }

    public void DropItem()
    {
        if(InvenId[SelectedSlot] != 0)
        {
            InvenCount[SelectedSlot]--;
            //Instantiate(itemInfo[InvenId[SelectedSlot]].Item, playerAct2.transform.position, Quaternion.identity);
            SlotUpdate(SelectedSlot);
        }
    }

    public bool RemoveSlot(int id, int count)
    {
        bool flag = false;

        if (ItemCount(id) < count)
            return flag;
        else
            flag = true;

        for (int i = 0; i < 18; i++)
        {
            if (InvenId[i] == id)
            {
                if (InvenCount[i] >= count)
                {
                    InvenCount[i] -= count;
                    SlotUpdate(i);
                    break;
                }
                else
                {
                    count = count - InvenCount[i];
                    InvenCount[i] = 0;
                    SlotUpdate(i);
                }
            }
        }
        return flag;
    }

    public void SelectUpdate() //슬롯 선택 업데이트
    {
        if (SelectedSlot != -1 && InvenId[SelectedSlot] != 0) //선택한 슬롯의 아이템이 있을때
        {
            itemInstruction.SetActive(true);
            itemName.text = itemInfo[InvenId[SelectedSlot]].ItemName;
            itemImg.sprite = itemInfo[InvenId[SelectedSlot]].ItemImg;
            itemExplain.text = itemInfo[InvenId[SelectedSlot]].ItemExp;
        }
        else
        {
            SelectedCheck.gameObject.SetActive(false);
            itemInstruction.SetActive(false);
            itemName.text = "";
            itemImg.sprite = itemInfo[0].ItemImg;
            itemExplain.text = "";
        }
    }

    public void SelectSlot(int slot) // 슬롯 선택
    {
        if (SelectedSlot == slot) //이미 선택한걸 다시 선택했을때
        {
            SelectedSlot = -1;
            SelectedCheck.gameObject.SetActive(false);
            itemInstruction.SetActive(false);
            itemName.text = "";
            itemImg.sprite = itemInfo[0].ItemImg;
            itemExplain.text = "";
        }
        else if (InvenId[slot] != 0) //선택
        {
            SelectedCheck.gameObject.SetActive(true);
            SelectedCheck.transform.position = InvenSlot[slot].transform.position;

            SelectedSlot = slot;
            itemInstruction.SetActive(true);
            itemName.text = itemInfo[InvenId[slot]].ItemName;
            itemImg.sprite = itemInfo[InvenId[slot]].ItemImg;
            itemExplain.text = itemInfo[InvenId[slot]].ItemExp;
        }
        else //비어있는창 선택했을때
        {
            SelectedSlot = slot;
            SelectedCheck.gameObject.SetActive(true);
            SelectedCheck.transform.position = InvenSlot[slot].transform.position;

            itemInstruction.SetActive(false);
            itemName.text = "";
            itemImg.sprite = itemInfo[0].ItemImg;
            itemExplain.text = "";
        }
    }

    public void SelectHotbar(int slot) // 핫바 선택
    {
        SelectedHot = slot;
        if (SelectedSlot == slot + 14) //이미 선택한걸 다시 선택했을때
        {
            SelectedSlot = -1;
            SelectedCheck.gameObject.SetActive(false);
            itemInstruction.SetActive(false);
            itemName.text = "";
            itemImg.sprite = itemInfo[0].ItemImg;
            itemExplain.text = "";
        }
        else if (UIManager.instance.screenState == UIManager.ScreenState.Inventory)
        {
            slot += 14;
            SelectedCheck.gameObject.SetActive(true);
            SelectedCheck.transform.position = InvenSlot[slot].transform.position;

            if (InvenId[slot] != 0)
            {
                SelectedSlot = slot;
                itemInstruction.SetActive(true);
                itemName.text = itemInfo[InvenId[slot]].ItemName;
                itemImg.sprite = itemInfo[InvenId[slot]].ItemImg;
                itemExplain.text = itemInfo[InvenId[slot]].ItemExp;
            }
            else
            {
                SelectedSlot = slot;
                itemInstruction.SetActive(false);
                itemName.text = "";
                itemImg.sprite = itemInfo[0].ItemImg;
                itemExplain.text = "";
            }
        }
        else
        {
            SelectedSlot = slot + 14;
            UIManager.instance.NowItemAlarm(itemInfo[InvenId[SelectedSlot]].ItemName);
        }
    }

    public void DragBeginSlot(int slot)
    {
        if(slot >= 20 && slot <=22) //블렌더
        {
            if (slot == 20)//식물창
            {
                DragingId = InvenId[slot];
                DragingCount = 1;

                //식물 존재 여부 = false
                playerAct2.nowObj.GetComponent<Blender>().MatId = 0;
            }
            else if (slot == 21)//흙창
            {
                DragingId = InvenId[slot];
                DragingCount = playerAct2.nowObj.GetComponent<Blender>().soilCount;

                //흙 존재 여부 = false
                playerAct2.nowObj.GetComponent<Blender>().soilCount = 0;
            }
            else //배출창
            {
                DragingId = InvenId[slot];
                DragingCount = 1;

                playerAct2.nowObj.GetComponent<Blender>().BlendReset();
            }
            OriginSlot = slot;

            InvenId[slot] = 0;
            InvenCount[slot] = 0;
            SlotUpdate(slot);

            isDrag = true;

            SelectedSlot = -1;
            SelectUpdate();
        }
        else if (InvenId[slot] != 0) //모든 슬롯  단, 선택한게 있을때만
        {
            DragingId = InvenId[slot];
            DragingCount = InvenCount[slot];
            OriginSlot = slot;

            InvenId[slot] = 0;
            InvenCount[slot] = 0;
            SlotUpdate(slot);

            isDrag = true;

            SelectedSlot = -1;
            SelectUpdate();
        }
    }

    public void DragEndSlot(int slot)
    {
        if (isDrag)
        {
            InvenId[slot] = DragingId;
            InvenCount[slot] = DragingCount;
            SlotUpdate(slot);
            DragingId = 0;
            DragingCount = 0;
            OriginSlot = -1;
            isDrag = false;
        }
    }

    public void DropSlot(int slot)
    {
        if (isDrag)
        {
            if (slot <= 11 || (slot >= 14 && slot <= 17)) //인벤토리 || 핫바
            {
                if (OriginSlot <= 18) //원래 일반슬롯에 있었을때
                {
                    //만약 같은 아이템이면 합치기
                    if (InvenId[slot] == DragingId && InvenCount[slot] < itemInfo[InvenId[slot]].maxCount)
                    {
                        if (DragingCount + InvenCount[slot] > itemInfo[InvenId[slot]].maxCount)
                        {
                            InvenCount[OriginSlot] = itemInfo[InvenId[slot]].maxCount - InvenCount[slot];
                            InvenCount[slot] = itemInfo[InvenId[slot]].maxCount;
                        }
                        else
                            InvenCount[slot] = DragingCount + InvenCount[slot];
                    }
                    else if (InvenId[slot] == 0) //해당 슬롯이 비어있었을때
                    {
                        InvenId[OriginSlot] = 0;
                        InvenCount[OriginSlot] = 0;
                        InvenId[slot] = DragingId;
                        InvenCount[slot] = DragingCount;
                    }
                    else //서로 다른아이템이면 교체
                    {
                        InvenId[OriginSlot] = InvenId[slot];
                        InvenCount[OriginSlot] = InvenCount[slot];
                        InvenId[slot] = DragingId;
                        InvenCount[slot] = DragingCount;
                    }
                }
                else //원래 블렌더에 있었을때
                {
                    if (InvenId[slot] == 0) //해당 슬롯이 비어있었을때
                    {
                        InvenId[OriginSlot] = 0;
                        InvenCount[OriginSlot] = 0;
                        InvenId[slot] = DragingId;
                        InvenCount[slot] = DragingCount;
                    }
                    else if (InvenId[slot] == DragingId && InvenCount[slot] < itemInfo[InvenId[slot]].maxCount)//같은 아이템이고 최대량보다 적었을때
                        InvenCount[slot]++;
                    else
                    {
                        InvenId[OriginSlot] = DragingId;
                        InvenCount[OriginSlot] = DragingCount;
                    }
                }
                SlotUpdate(OriginSlot);
                SlotUpdate(slot);

                DragingId = 0;
                DragingCount = 0;
                OriginSlot = -1;
            }
            else if (slot == 12) //도구 창
            {
                //21 : 유도표식, 25 : 드릴, 26: 삽
                if ((DragingId == 21 || DragingId == 25 || DragingId == 26) && InvenId[12] == 0)
                {
                    InvenId[12] = DragingId;
                    InvenCount[12] = 1;

                    InvenId[OriginSlot] = 0;
                    InvenCount[OriginSlot] = 0;

                    SlotUpdate(OriginSlot);
                    SlotUpdate(12);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;

                }
                else
                {
                    InvenId[OriginSlot] = DragingId;
                    InvenCount[OriginSlot] = 1;
                    SlotUpdate(OriginSlot);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;
                }
            }
            else if (slot == 13) //장비 창
            {
                //22 : 추가 산소통, 23 : 제트팩
                if ((DragingId == 22 || DragingId == 23) && InvenId[13] == 0)
                {
                    InvenId[13] = DragingId;
                    InvenCount[13] = 1;

                    InvenId[OriginSlot] = 0;
                    InvenCount[OriginSlot] = 0;

                    SlotUpdate(OriginSlot);
                    SlotUpdate(13);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;
                }
                else
                {
                    InvenId[OriginSlot] = DragingId;
                    InvenCount[OriginSlot] = DragingCount;
                    SlotUpdate(OriginSlot);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;
                }
            }
            else if (slot == 20 && DragingId > 0 && DragingId <= 20 && InvenId[20] == 0) //블렌더 식물창
            {
                playerAct2.nowObj.GetComponent<Blender>().MatId = DragingId;
                InvenId[20] = DragingId;
                InvenCount[20] = 1;
                InvenSlot[20].sprite = itemInfo[DragingId].ItemImg;

                if (DragingCount-- == 0)
                {
                    DragingId = 0;
                    OriginSlot = -1;
                }
                else
                {
                    InvenId[OriginSlot] = DragingId;
                    InvenCount[OriginSlot] = DragingCount;

                    SlotUpdate(OriginSlot);
                    SlotUpdate(slot);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;
                }
            }
            else if (slot == 21 && DragingId == 51 && InvenCount[21] <= 9) //블렌더 흙창
            {
                if (DragingCount + playerAct2.nowObj.GetComponent<Blender>().soilCount > 9)
                {
                    DragingCount = DragingCount + playerAct2.nowObj.GetComponent<Blender>().soilCount - 9;
                    playerAct2.nowObj.GetComponent<Blender>().soilCount = 9;
                }
                else
                {
                    playerAct2.nowObj.GetComponent<Blender>().soilCount = DragingCount;
                    DragingCount = 0;
                }

                InvenId[slot] = 51;
                InvenCount[slot] = playerAct2.nowObj.GetComponent<Blender>().soilCount;
                SlotUpdate(slot);

                if (DragingCount != 0)
                {
                    InvenId[OriginSlot] = DragingId;
                    InvenCount[OriginSlot] = DragingCount;

                    SlotUpdate(OriginSlot);
                    SlotUpdate(slot);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;
                }
                else
                {
                    InvenId[OriginSlot] = DragingId;
                    InvenCount[OriginSlot] = DragingCount;
                    SlotUpdate(OriginSlot);

                    DragingId = 0;
                    DragingCount = 0;
                    OriginSlot = -1;
                }
            }
            else
            {
                InvenId[OriginSlot] = DragingId;
                InvenCount[OriginSlot] = DragingCount;
                SlotUpdate(OriginSlot);

                DragingId = 0;
                DragingCount = 0;
                OriginSlot = -1;
            }
        }
        isDrag = false;
    }

    public void SlotUpdate(int slot) //슬롯 창 업데이트
    {
        if (slot < 20 && InvenCount[slot] == 0)
        {
            InvenText[slot].text = "";
            InvenId[slot] = 0;
        }
        Debug.Log(slot);
        InvenText[slot].text = InvenCount[slot].ToString();
        InvenSlot[slot].sprite = itemInfo[InvenId[slot]].ItemImg;
    }
}

[System.Serializable]
public struct ItemInfo
{
    public Sprite ItemImg;
    public string ItemTxt;
    public string ItemName;
    [TextArea]
    public string ItemExp;

    public GameObject Block;
    public GameObject Item;
    public float pickTime;

    public int maxCount;
}