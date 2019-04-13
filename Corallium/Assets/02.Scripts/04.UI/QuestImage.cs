using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestImage : MonoBehaviour {

    public GameObject QuestList;
    public bool isOpen = false;

    public Image[] questImg = new Image[3];
    public Text[] questText = new Text[3];
    public int[] nowQuestCase = new int[3];
    public int[] nowQuestCount = new int[3];

    List<int> alreadyQuest = new List<int>();

    public int clearCount = 0;
    public int maxCount = 3;

    public int[] GetSeaWeed = new int[5];
    public int TameFish = 0;
    public int[] GetGem = new int[7];
    public int GetGemAll = 0;
    public bool CraftBlender = false;

    bool blendQuest = false;
    bool airLineQuest = false;

    public Sprite fishimg;

    public Inventory inven;
    public static QuestImage instance;

    void Start () {
        instance = GetComponent<QuestImage>();
        for (int i = 0; i < 3; i++)
            RandomQuest(i);
	}

    public void CheckQuest()
    {
        for (int i = 0; i < 3; i++)
        {
            if (nowQuestCase[i] == 1) //물고기 길들이기
            {
                if(TameFish >= nowQuestCount[i])
                {
                    clearCount++;
                    if (clearCount >= maxCount)
                        StoryManager.instance.Ending();
                    else
                        RandomQuest(i);
                }
            }
            else if (nowQuestCase[i] == 2) //광석 채집하기
            {
                if(GetGemAll >= nowQuestCount[i])
                {
                    clearCount++;
                    if (clearCount >= maxCount)
                        StoryManager.instance.Ending();
                    else
                        RandomQuest(i);
                }
            }
            else if (nowQuestCase[i] == 4) //블렌더하나 제작하기
            {
                if (CraftBlender)
                {
                    clearCount++;
                    if (clearCount >= maxCount)
                        StoryManager.instance.Ending();
                    else
                        RandomQuest(i);
                }
            }
            else if (nowQuestCase[i] < 100) //해초 수확하기
            {
                if (GetSeaWeed[(int)(nowQuestCase[i] * 0.1f) - 1] >= nowQuestCount[i])
                {
                    clearCount++;
                    if (clearCount >= maxCount)
                        StoryManager.instance.Ending();
                    else
                        RandomQuest(i);
                }
            }
            else
            {
                if (GetGem[(int)(nowQuestCase[i] * 0.01f) - 41] >= nowQuestCount[i])
                {
                    clearCount++;
                    if (clearCount >= maxCount)
                        StoryManager.instance.Ending();
                    else
                        RandomQuest(i);
                }
            }
        }
    }

    public void RandomQuest(int slot)
    {
        switch (Random.Range(0, 5))
        {
            case 0://해초 수확하기 (2 ~ 5)
                int RandomSeaWeed = Random.Range(1, 6);
                questImg[slot].sprite = inven.itemInfo[RandomSeaWeed].ItemImg;

                int RandomSeaCount = Random.Range(2, 6);
                questText[slot].text = inven.itemInfo[RandomSeaWeed].ItemName + "을/를 " + RandomSeaCount + "개 수확하기";

                if (alreadyQuest.Contains(RandomSeaCount))
                {
                    RandomQuest(slot);
                    return;
                }
                alreadyQuest.Add(RandomSeaCount);
                nowQuestCase[slot] = RandomSeaWeed * 10;
                nowQuestCount[slot] = RandomSeaCount;

                break;


            case 1://물고기 길들이기 1
                questImg[slot].sprite = fishimg;
                questText[slot].text = "물고기를 한마리 길들이기";
                if (alreadyQuest.Contains(10))
                {
                    RandomQuest(slot);
                    return;
                }
                alreadyQuest.Add(10);
                nowQuestCase[slot] = 1;
                nowQuestCount[slot] = 1;

                break;


            case 2://광석 채집하기 (4 ~ 6)
                int RandomGemCount = Random.Range(4, 7);
                questImg[slot].sprite = inven.itemInfo[47].ItemImg;
                questText[slot].text = "광석을 " + RandomGemCount + "개 채집하기";

                if (alreadyQuest.Contains(20 + RandomGemCount))
                {
                    RandomQuest(slot);
                    return;
                }
                alreadyQuest.Add(20 + RandomGemCount);
                nowQuestCase[slot] = 2;
                nowQuestCount[slot] = RandomGemCount;

                break;

            case 3://~ 광석 채집하기 4
                int RandomGem = Random.Range(41, 48);
                questImg[slot].sprite = inven.itemInfo[RandomGem].ItemImg;

                questText[slot].text = inven.itemInfo[RandomGem].ItemName + "을/를 4개 채집하기";

                if (alreadyQuest.Contains(30  + RandomGem))
                {
                    RandomQuest(slot);
                    return;
                }
                alreadyQuest.Add(30 + RandomGem);
                nowQuestCase[slot] = RandomGem * 100;
                nowQuestCount[slot] = 4;

                break;


            case 4://블렌더 제작하기 1
                questImg[slot].sprite = inven.itemInfo[34].ItemImg;
                questText[slot].text = "블렌더 1개 제작하기";

                if (alreadyQuest.Contains(40))
                {
                    RandomQuest(slot);
                    return;
                }
                alreadyQuest.Add(40);
                nowQuestCase[slot] = 4;
                nowQuestCount[slot] = 1;

                break;
        }
    }

    public void OpenQuestList()
    {
        if(isOpen)
            QuestList.SetActive(false);
        else
            QuestList.SetActive(true);
        isOpen = !isOpen;
    }
}
