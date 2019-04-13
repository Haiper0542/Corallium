using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour {

    public int nowPoint = 0;
    public int score = 0;
    public Skill[] skills = new Skill[6];

    public Image SkillImg;
    public Text SkillName;
    public Text PointText;

    public bool isSkill = false;

    public PlayerAct2 playerAct2;
    public ClickMove clickMove;
    public Inventory inventory;

    public void Awake()
    {
        for(int i = 0;i<6;i++)
            skills[i].nowText.text = skills[i].nowIndex.ToString() + " / " + skills[i].maxIndex.ToString();
    }

    public void OnEnable()
    {
        nowPoint = -1;
        PointText.text = score.ToString() + "P";
        SkillImg.sprite = null;
        SkillName.text = "";
    }

    public void SetPoint(int i)
    {
        if (!isSkill)
        {
            nowPoint = i;
        }
        SkillImg.sprite = skills[i].Skillimg;
        SkillName.text = skills[i].SkillName;
    }

    public void Usepoint()
    {
        switch (nowPoint)
        {
            case 0:
                Tank_Upgrade();
                break;
            case 1:
                Speed_Upgrade();
                break;
            case 2:
                Slot_Upgrade();
                break;
            case 3:
                Lung_Upgrade();
                break;
            case 4:
                Sprint();
                break;
            case 5:
                BioSlot();
                break;
        }
        PointText.text = score.ToString() + "P";
    }

    void Tank_Upgrade()
    {
        if (score > 0 && skills[0].nowIndex < skills[0].maxIndex)
        {
            skills[0].nowIndex++;
            playerAct2.Maxair += 25;

            score--;
        }
    }

    void Speed_Upgrade()
    {
        Debug.Log("Speed_Upgrade");
        if (score > 0 && skills[1].nowIndex < skills[1].maxIndex)
        {
            Debug.Log("Speed_Upgrade");
            skills[1].nowIndex++;
            clickMove.Speed += 0.4f;

            score--;
        }
    }

    void Slot_Upgrade()
    {
        Debug.Log("Slot_Upgrade");
        if (score > 0 && skills[2].nowIndex < skills[2].maxIndex)
        {
            Debug.Log("Slot_Upgrade");
            inventory.SlotUpgrade();
            skills[2].nowIndex++;
            skills[2].nowText.text = skills[2].nowIndex.ToString() + " / " + skills[2].maxIndex.ToString();

            score--;
        }
    }

    void Lung_Upgrade()
    {
        if (score > 0 && skills[3].nowIndex < skills[3].maxIndex)
        {
            skills[0].nowIndex++;
            playerAct2.airTime += 0.25f;

            score--;
        }
    }

    void Sprint()
    {
        Debug.Log("Sprint");
        if (score > 0 && skills[4].nowIndex < skills[4].maxIndex)
        {
            Debug.Log("Sprint");

            score--;
        }
    }

    void BioSlot()
    {
        Debug.Log("BioSlot");
        if (score > 0 && skills[5].nowIndex < skills[5].maxIndex)
        {
            Debug.Log("BioSlot");

            score--;
        }
    }
}

[System.Serializable]
public struct Skill
{
    public Sprite Skillimg;
    public string SkillName;

    public int nowIndex;
    public int maxIndex;
    public Text nowText;
}