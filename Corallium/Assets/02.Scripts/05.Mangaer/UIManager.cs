using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool canOpenInven = false;

    public enum ScreenState
    {
        None,
        Inventory,
        Craft
    }
    public ScreenState screenState = ScreenState.None;

    public bool isPause = false;
    public bool isCrafting = false;

    public Camera mainCam;

    [Header("[Inventory]")]
    public Transform Inventory;
    public Transform SelectedSlot;
    public Transform itemInstruction;
    public Transform Hotbar;

    [Header("[Screen]")]
    public GameObject PauseScreen;
    public GameObject CraftScreen;
    public GameObject PrinterScreen;
    public GameObject BlenderScreen;
    public GameObject SkillScreen;

    [Header("[Button]")]
    public Text NowItem;            //현재 아이템 하단에 표시
    public RectTransform InvenBtn;
    public GameObject CraftBtn;
    public GameObject RightHand_Icons;
    public GameObject LeftHand_Icons;
    public GameObject CameraBtn;

    public GameObject leftOnIcon;
    public GameObject leftOffIcon;

    [Header("[Alarm]")]
    public Text[] AlarmTxt = new Text[3];

    [Header("[Craft]")]
    public float CraftingTime;
    public GameObject CanclePanel;
    public GameObject CraftBtnImg;
    public GameObject CancleEmpty;
    public Image CancleFill;
    public CraftUI nowClicked;
    public CraftUI FirstCraft;

    [Space]
    [Space]
    public ClickMove clickMove;
    public Inventory inventory;

    public static UIManager instance;

    private void Awake()
    {
        instance = GetComponent<UIManager>();
    }

    private void Update()
    {
        if (CancleFill.gameObject.activeSelf)
        {
            CancleFill.fillAmount = (Time.time - CraftingTime) / nowClicked.CraftTime;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Alarm("꽉 참!");
        }
    }

    //인벤토리 버튼
    public void InvenButton()
    {
        if (!isPause && canOpenInven)
        {
            if (screenState == ScreenState.None)
            {
                screenState = ScreenState.Inventory;

                clickMove.camOn = false;
                clickMove.on = false;

                Inventory.gameObject.SetActive(true);
                Inventory.localPosition = new Vector3(-20f, 10, 0);

                if (inventory.SelectedSlot >= 0)
                {
                    inventory.SelectUpdate();
                    SelectedSlot.gameObject.SetActive(true);
                }

                StartCoroutine("CraftOpenAni");
            }
            else if (!isCrafting)
            {
                screenState = ScreenState.None;

                clickMove.camOn = true;
                clickMove.on = true;

                Inventory.gameObject.SetActive(false);
                SelectedSlot.gameObject.SetActive(false);
                itemInstruction.gameObject.SetActive(false);
                CraftScreen.gameObject.SetActive(false);

                PrinterScreen.SetActive(false);
                if (BlenderScreen.activeSelf)
                {
                    BlenderScreen.SetActive(false);
                    Hotbar.Translate(95.5f, 0, 0);
                }

                FirstCraft.MatReset();

                if (CraftBtn.transform.localPosition.y <= -5)
                    StartCoroutine("CraftCloseAni");
            }
        }
    }

    public void CraftButton()
    {
        if (!isPause && screenState != ScreenState.Craft) //열기
        {
            screenState = ScreenState.Craft;

            FirstCraft.MatReset(); //초기화

            Inventory.gameObject.SetActive(false);
            SelectedSlot.gameObject.SetActive(false);
            itemInstruction.gameObject.SetActive(false);

            CraftScreen.gameObject.SetActive(true);
        }
        else if (!isPause)
        {
            screenState = ScreenState.Inventory; //인벤토리로 변경

            FirstCraft.MatReset(); //초기화

            Inventory.gameObject.SetActive(true);
            Inventory.localPosition = new Vector3(-20f, 10, 0);

            if (inventory.SelectedSlot >= 0)
            {
                inventory.SelectUpdate();
                SelectedSlot.gameObject.SetActive(true);
            }

            CraftScreen.gameObject.SetActive(false);
        }
    }

    IEnumerator CraftOpenAni()
    {
        StopCoroutine("CraftCloseAni");

        RectTransform CraftRect = CraftBtn.GetComponent<RectTransform>();
        CraftRect.localPosition = InvenBtn.localPosition;
        CraftBtn.gameObject.SetActive(true);
        for (int i = 0; i < 9; i++)
        {
            CraftRect.localPosition = new Vector3(CraftRect.localPosition.x, CraftRect.localPosition.y - 1.8f * i, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator CraftCloseAni()
    {
        StopCoroutine("CraftOpenAni");

        RectTransform CraftRect = CraftBtn.GetComponent<RectTransform>();
        for (int i = 0; i < 9; i++)
        {
            CraftRect.localPosition = new Vector3(CraftRect.localPosition.x, CraftRect.localPosition.y + 1.8f * i, 0);
            yield return new WaitForSeconds(0.001f);
        }
        CraftBtn.gameObject.SetActive(false);
    }

    public void PauseButton()
    {
        if (!isPause)
            PauseScreenOn();
        else
            PauseScreenOff();
    }

    void PauseScreenOn()
    {
        PauseScreen.SetActive(true);
        isPause = false;
        Time.timeScale = 0;
    }

    void PauseScreenOff()
    {
        PauseScreen.SetActive(false);
        isPause = true;
        Time.timeScale = 1;
    }

    public void PlayButton()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
        clickMove.camOn = true;
        clickMove.on = true;
        isPause = false;
    }

    public void HomeButton()
    {
        StartCoroutine("HomeAni");
    }

    public void ExitButton()
    {
        StartCoroutine("ExitAni");
    }

    public void LeftButton(bool on)
    {
        clickMove.leftHand = on;
        if(on)
        {
            LeftHand_Icons.SetActive(true);
            RightHand_Icons.SetActive(false);
        }
        else
        {
            LeftHand_Icons.SetActive(false);
            RightHand_Icons.SetActive(true);
        }
        leftOnIcon.SetActive(on);
        leftOffIcon.SetActive(!on);
    }

    public void SkillTreeOpen()
    {
        SkillScreen.SetActive(true);
        clickMove.camOn = false;
        clickMove.on = false;

        canOpenInven = false;
    }
    public void SkillTreeClose()
    {
        SkillScreen.SetActive(false);
        clickMove.camOn = true;
        clickMove.on = true;

        canOpenInven = true;
    }

    public void Crafting()
    {
        StartCoroutine("CraftAni");
    }

    IEnumerator CraftAni()
    {
        isCrafting = true;
        CraftingTime = Time.time;

        CanclePanel.SetActive(true);
        CancleEmpty.SetActive(true);
        CraftBtnImg.SetActive(false);
        CancleFill.gameObject.SetActive(true);

        yield return new WaitForSeconds(nowClicked.CraftTime);

        isCrafting = false;
        for (int i = 0; i < nowClicked.MatInfos.Length; i++)
        {
            inventory.RemoveSlot(nowClicked.MatInfos[i].MatId, nowClicked.MatInfos[i].MatCount);
        }

        inventory.AddSlot(nowClicked.Craftid, 1);
        if(nowClicked.Craftid == 34)
        {
            QuestImage.instance.CraftBlender = true;
        }
        CraftingTime = 0;
        CanclePanel.SetActive(false);
        CancleEmpty.SetActive(false);
        CraftBtnImg.SetActive(true);
        CancleFill.gameObject.SetActive(false);

        nowClicked.CraftButton();
    }

    public void PrinterButton()
    {
        if (!isPause) //열기
        {
            PrinterScreen.gameObject.SetActive(true);

            clickMove.camOn = false;
            clickMove.on = false;

            if (inventory.SelectedSlot >= 0)
            {
                inventory.SelectUpdate();
                SelectedSlot.gameObject.SetActive(true);
            }
        }
    }
    public void PrinterClose()
    {
        if (!isPause) //닫기
        {
            PrinterScreen.gameObject.SetActive(false);

            clickMove.camOn = true;
            clickMove.on = true;
        }
    }

    public void BlenderButton()
    {
        if (!isPause) //열기
        {
            screenState = ScreenState.Inventory;
            BlenderScreen.SetActive(true);

            //플레이어가 열은 블렌더의 흙의 갯수가 0보다 클때
            if (GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>().soilCount > 0)
                inventory.InvenId[21] = 51;
            else
                inventory.InvenId[21] = 0;
            inventory.InvenCount[21] = GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>().soilCount;

            if (GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>().MatId > 0)
            {
                inventory.InvenId[20] = GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>().MatId;
                inventory.InvenCount[20] = 1;
            }
            else
            {
                inventory.InvenId[20] = 0;
                inventory.InvenCount[20] = 0;
            }

            if (GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>().SuccesId > 0)
            {
                inventory.InvenId[22] = GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>().SuccesId;
                inventory.InvenCount[22] = 1;
            }
            else
            {
                inventory.InvenId[22] = 0;
                inventory.InvenCount[22] = 0;
            }

            inventory.SlotUpdate(20);
            inventory.SlotUpdate(21);
            inventory.SlotUpdate(22);

            clickMove.camOn = false;
            clickMove.on = false;

            Inventory.gameObject.SetActive(true);
            Inventory.localPosition = new Vector3(-115.5f, 10, 0);
            Hotbar.Translate(-95.5f,0, 0);
            if (inventory.SelectedSlot >= 0)
            {
                inventory.SelectUpdate();
                SelectedSlot.gameObject.SetActive(true);
            }

            canOpenInven = true;
        }
    }

    public void BlenderBtn()
    {
        Blender nowBlender = GameObject.Find("Player").GetComponent<PlayerAct2>().nowObj.GetComponent<Blender>();
        if (nowBlender.soilCount == 0)
            Alarm("흙이 부족합니다!");
        else if(nowBlender.MatId == 0)
            Alarm("섞을 재료가 없습니다!");
        else
            nowBlender.BlendStart();
    }

    public void Alarm(string alarm)
    {
        for (int i = 0; i < 3; i++)
        {
            if (!AlarmTxt[i].gameObject.activeSelf)
            {
                AlarmTxt[i].gameObject.SetActive(true);
                AlarmTxt[i].text = alarm;
                StartCoroutine("AlarmAni", AlarmTxt[i]);
                break;
            }
        }
    }

    IEnumerator AlarmAni(Text Alarm_)
    {
        float nowY = 30;
        float nowAlpha = 1;
        Alarm_.transform.localPosition = new Vector3(0, 30, 0);
        Alarm_.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 20; i++)
        {
            Alarm_.transform.localPosition = new Vector3(0, nowY += 0.3f, 0);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 20; i++)
        {
            Alarm_.transform.localPosition = new Vector3(0,nowY += 0.3f,0);
            Alarm_.color = new Color(255, 255, 255, nowAlpha -= 0.06f);
            yield return new WaitForSeconds(0.001f);
        }
        Alarm_.gameObject.SetActive(false);
    }

    public void NowItemAlarm(string alarm)
    {
        NowItem.gameObject.SetActive(true);
        NowItem.text = alarm;
        StopCoroutine("NowItemAlarmAni");
        StartCoroutine("NowItemAlarmAni");
    }

    IEnumerator NowItemAlarmAni()
    {
        float nowAlpha = 0;
        for (int i = 0; i < 5; i++)
        {
            NowItem.color = new Color(255, 255, 255, nowAlpha += 0.2f);
            yield return new WaitForSeconds(0.001f);
        }
        NowItem.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 20; i++)
        {
            NowItem.color = new Color(255, 255, 255, nowAlpha -= 0.06f);
            yield return new WaitForSeconds(0.001f);
        }
        NowItem.gameObject.SetActive(false);
    }

    IEnumerator HomeAni()
    {
        yield return StartCoroutine("SaveData");
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
        clickMove.camOn = true;
        clickMove.on = true;
        isPause = false;
        SceneManager.LoadScene(0);
    }

    IEnumerator ExitAni()
    {
        yield return StartCoroutine("SaveData");
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
        clickMove.camOn = true;
        clickMove.on = true;
        isPause = false;
        Application.Quit();
    }

    IEnumerator SaveData()
    {
        //저장하기
        Debug.Log("저장 추가 바람");
        yield return null;
    }
}