using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAct2 : MonoBehaviour
{

    public Image fade;
    bool isOver = false;

    [Header("[Action]")]
    public GameObject nowObj;
    public int nowId;
    public int handId;
    bool canPick = false;

    public Transform player;

    [Header("[Slider]")]
    public Image Redslider;
    public GameObject Red;

    [Header("[Layer Mask]")]
    public LayerMask layerMask;
    public LayerMask ItemMask;
    public LayerMask soilMask;

    [Header("[Air]")]
    public Image Airslider;
    public Text AirText;
    public float Maxair = 100;
    public float air = 100;
    public bool charging = false;
    public GameObject BubbleParticle;

    [Space]
    public float airTime = 1.0f; //산소 감소 빈도
    float nextAirTime = 0.0f;

    [Space]
    public float ChargeTimeLeft = 0.2f; //산소 충전 빈도
    float ChargenextTime = 0.0f;

    [Header("[Pick Act]")]
    public bool isClick = false;
    public float ClickTime = 0.0f;
    public float TermTime = 1.0f;

    [Header("[Ride Act]")]
    public bool isRidden = false;
    public CharacterController RideObj;

    [Header("[Compass]")]
    public GameObject Compass;
    public bool OnCompass = false;

    [Header("[Module Icon]")]
    public GameObject LModuleIcon;
    public Image LModuleIconImg;
    public GameObject RModuleIcon;
    public Image RModuleIconImg;

    public GameObject nowFish;
    public bool isFish = false;
    public bool isSub = false;
    bool isFeeding = false;
    public Sprite fishImg;

    public Transform home;
    public Light dirLight;
    public Light dirLight2;

    [Header("[Pick Icon]")]
    public Image LPickImg;
    public Image RPickImg;

    public Sprite pickSprite; //줍기
    public Sprite plantSprite; //설치하기

    [Space]
    [Space]
    public SelectCircle selectCircle;
    public Inventory inven;
    public UIManager uiManager;
    public Animator animator;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Event"))
        {
            switch (other.name)
            {
                case "Bottle":
                    StartCoroutine("Bottle", other);
                    break;
                case "StartBoost":
                    StartCoroutine("StartBoost", other);
                    break;
                case "CureEvent":
                    Debug.Log("CureEvent");
                    break;
            }
        }
    }

    IEnumerator Bottle(Collider other)
    {
        int count = Random.Range(1, 5);
        int SeaWeed = Random.Range(1, 7);
        inven.AddSlot(SeaWeed, count);
        uiManager.Alarm(inven.itemInfo[SeaWeed].ItemName + "  + " + count.ToString());

        count = Random.Range(1, 3);
        SeaWeed = Random.Range(41, 48);
        inven.AddSlot(SeaWeed, count);

        Destroy(other.gameObject);
        yield return new WaitForSeconds(0.75f);
        uiManager.Alarm(inven.itemInfo[SeaWeed].ItemName + "  + " + count.ToString());
    }
    IEnumerator StartBoost(Collider other)
    {
        int count = Random.Range(4, 6);
        int SeaWeed = Random.Range(1, 7);
        inven.AddSlot(SeaWeed, count);
        uiManager.Alarm(inven.itemInfo[SeaWeed].ItemName + "  + " + count.ToString());

        Destroy(other.gameObject);
        yield return new WaitForSeconds(0.75f);

        count = Random.Range(4, 6);
        SeaWeed = Random.Range(1, 7);
        inven.AddSlot(SeaWeed, count);
        uiManager.Alarm(inven.itemInfo[SeaWeed].ItemName + "  + " + count.ToString());
    }

    public void Update()
    {
        //산소량 충전
        if (charging)
        {
            if (Time.time > ChargenextTime && air < Maxair)
            {
                ChargenextTime = Time.time + ChargeTimeLeft;
                air++;
                air = Mathf.Clamp(air, 0, 100);
            }
            if(air >= 99)
                AirText.text = "산소통 " + 100 + "/" + 100;
            else
                AirText.text = "산소통 " + (int)air + "/" + 100;
            BubbleParticle.SetActive(true);
        }
        else
        {
            //산소량 감소
            if (Time.time > nextAirTime && air > 0)
            {
                nextAirTime = Time.time + airTime;
                air--;
                air = Mathf.Clamp(air, 0, 100);
            }
            AirText.text = "산소통  " + (int)air + "/" + 100;
            BubbleParticle.SetActive(false);
        }
        Airslider.fillAmount = air * 0.01f;

        if(air <= 0 && !isOver)
        {
            StartCoroutine("AirOff");
        }

        //무슨아이템이 있는지 검사
        nowObj = null;
        nowId = 0;
        bool flag = false;
        canPick = false;
        RModuleIcon.SetActive(false);
        LModuleIcon.SetActive(false);

        isFish = false;
        isSub = false;
        nowFish = null;

        //차에 타고 있지 않으면
        if (!isRidden)
        {
            RaycastHit hit;
            if (isFish)
            {
                RModuleIcon.SetActive(true);
                LModuleIcon.SetActive(true);
                RModuleIconImg.sprite = fishImg;
                LModuleIconImg.sprite = fishImg;
            }
            if (Physics.SphereCast(player.position - player.forward, 0.6f, player.forward, out hit, 2.5f, layerMask))
            {
                //태그가 아이템인 경우
                if (hit.transform.CompareTag("Item"))
                {
                    nowObj = hit.transform.gameObject;
                    selectCircle.gameObject.SetActive(true);
                    selectCircle.Resize(nowObj.transform.position, nowObj.transform.localScale.x);
                    flag = true;
                    if (nowObj.name == "Copper" || nowObj.name == "Lithum" || nowObj.name == "Iron" || nowObj.name == "Gold" || nowObj.name == "Emerald")
                    {
                        if (nowObj.GetComponent<Gem>().canMine)
                            canPick = true;
                    }
                    else
                        canPick = true;
                }
                //태그가 흙 경우
                else if (hit.transform.CompareTag("Soil"))
                {
                    if (handId == 0 || handId > 20)
                    {
                        nowObj = hit.transform.gameObject;
                        selectCircle.gameObject.SetActive(true);
                        selectCircle.Resize(nowObj.transform.position, nowObj.transform.localScale.x);
                        flag = true;
                        canPick = true;
                    }
                    else
                    {
                        Vector3 pos = player.position + transform.forward * 1.5f;
                        if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y, Mathf.Round(pos.z)), Vector3.down, out hit, 10f, ItemMask))
                        {
                            selectCircle.gameObject.SetActive(true);
                            selectCircle.Resize(hit.point, 1);
                            flag = true;
                        }
                    }
                }

                //태그가 모듈인 경우
                else if (hit.transform.CompareTag("Module"))
                {
                    nowObj = hit.transform.gameObject;
                    selectCircle.gameObject.SetActive(true);

                    switch (nowObj.name)
                    {
                        case "Rover":
                        case "Rover(Clone)":
                            selectCircle.Resize(nowObj.transform.position + Vector3.down * 0.2f, 2.3f);
                            break;
                        case "Printer":
                        case "Printer(Clone)":
                            selectCircle.Resize(nowObj.transform.position, nowObj.transform.localScale.x);
                            break;
                        case "AirGen":
                        case "AirGen(Clone)":
                            selectCircle.Resize(nowObj.transform.position, nowObj.transform.localScale.x);
                            break;
                        case "Blender":
                        case "Blender(Clone)":
                            selectCircle.Resize(nowObj.transform.position + Vector3.up * 0.1f + Vector3.left * 0.1f, 1.5f);
                            break;
                        case "Sign":
                        case "Sign(Clone)":
                            selectCircle.Resize(nowObj.transform.position, nowObj.transform.localScale.x);
                            break;
                    }
                    flag = true;
                    canPick = true;
                    RModuleIcon.SetActive(true);
                    LModuleIcon.SetActive(true);
                }
                //태그가 잠수함
                else if (hit.transform.CompareTag("Submarine"))
                {
                    Debug.Log("!");
                    isSub = true;

                    RModuleIcon.SetActive(true);
                    LModuleIcon.SetActive(true);
                }
                //태그가 에어로프
                else if (hit.transform.name == "AirGen" || hit.transform.name == "AirGen(Clone)")
                {
                    nowObj = hit.transform.gameObject;
                    selectCircle.gameObject.SetActive(true);
                    selectCircle.Resize(nowObj.transform.position, nowObj.transform.localScale.x);
                    flag = true;
                    canPick = true;
                }
                else if (hit.transform.CompareTag("Fish") && handId > 0 && handId < 20)
                {
                    nowFish = hit.transform.gameObject;
                    selectCircle.gameObject.SetActive(true);
                    selectCircle.Resize(hit.transform.position + Vector3.up, 5);

                    isFish = true;
                    RModuleIcon.SetActive(true);
                    LModuleIcon.SetActive(true);
                    RModuleIconImg.sprite = fishImg;
                    LModuleIconImg.sprite = fishImg;
                }
            }
            //앞이 비어있고 해초를 들고 있는 경우
            else if (handId > 0) //해초,산호류
            {
                Vector3 pos = player.position + transform.forward * 1.5f;
                if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y, Mathf.Round(pos.z)), Vector3.down, out hit, 10f, ItemMask))
                {
                    if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length == 0)
                    {
                        selectCircle.gameObject.SetActive(true);
                        selectCircle.Resize(hit.point, 1);
                        flag = true;
                    }
                }
            }

            if (nowObj != null)
            {
                string[] j = nowObj.name.Split(new char[] { '(' });

                int i = 0;
                while (j[0] != inven.itemInfo[i].ItemTxt)
                    i++;
                nowId = i;

                //25 : 드릴
                if (inven.InvenId[12] == 25 && nowId > 40 && nowId < 48)
                    TermTime = inven.itemInfo[nowId].pickTime * 0.35f;
                else
                    TermTime = inven.itemInfo[nowId].pickTime;
            }
            else if (!flag)
            {
                nowId = -1;
                selectCircle.gameObject.SetActive(false);
            }

            if (nowId >= 31 && nowId <= 35)
            {
                RModuleIconImg.sprite = inven.itemInfo[nowId].ItemImg;
                LModuleIconImg.sprite = inven.itemInfo[nowId].ItemImg;
            }

            if (isClick)
            {
                animator.SetBool("isPicking", true);
                ClickTime += Time.deltaTime;
                Red.SetActive(true);
                Redslider.fillAmount = ClickTime / TermTime;
                if (ClickTime > TermTime)
                {
                    if (canPick)
                    {
                        if(nowId == 34)
                        {
                            if (nowObj.GetComponent<Blender>().soilCount > 0)
                                inven.AddSlot(51, nowObj.GetComponent<Blender>().soilCount);
                            if (nowObj.GetComponent<Blender>().MatId != 0)
                                inven.AddSlot(nowObj.GetComponent<Blender>().MatId, 1);
                        }

                        //아이템 지급
                        if(nowId > 0 && nowId < 20)
                        {
                            QuestImage.instance.GetSeaWeed[nowId - 1]++;
                            switch (nowId)
                            {
                                case 1://해초
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0://(해초 씨앗 1개)
                                            inven.AddSlot(nowId, 1);
                                            break;

                                        case 1://(해초 씨앗 1개 / 해초 1개(35 %)
                                            inven.AddSlot(nowId, 1);
                                            if (Random.Range(0, 100) <= 35)
                                                inven.AddSlot(63, 1);
                                            break;

                                        case 2://(해초 씨앗 1개(85 %), 2개(15 %) / 해초 1개(35 %), 2개(65 %)
                                            if (Random.Range(0, 100) <= 85)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 35)
                                                inven.AddSlot(63, 1);
                                            else
                                                inven.AddSlot(63, 2);
                                            break;
                                    }
                                    break;

                                case 2://아크로포라
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0://(해초 씨앗 1개)
                                            inven.AddSlot(nowId, 1);
                                            break;

                                        case 1://(해초 씨앗 1개 / 해초 1개(35 %)
                                            inven.AddSlot(nowId, 1);
                                            if (Random.Range(0, 100) <= 35)
                                                inven.AddSlot(63, 1);
                                            break;

                                        case 2://아크로포라 (해초 씨앗 1개(85%), 2개(15%) / 해초 1개(35%), 2개(65%)
                                            if (Random.Range(0, 100) <= 85)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 35)
                                                inven.AddSlot(63, 1);
                                            else
                                                inven.AddSlot(63, 2);
                                            break;
                                    }
                                    break;

                                case 3://버드 네스트 (산호 1개(70%), 2개(30%)/ 산호가지 0개(50%), 1개(50%))
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 1:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 2:
                                            if (Random.Range(0, 100) <= 70)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 50)
                                                inven.AddSlot(62, 1);
                                            break;
                                    }
                                    break;

                                case 4://자스민 폴립 (산호 1개(70%), 2개(30%)/ 산호가지 0개(50%), 1개(50%))
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 1:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 2:
                                            if (Random.Range(0, 100) <= 70)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 50)
                                                inven.AddSlot(62, 1);
                                            break;
                                    }
                                    break;

                                case 5://몬티오포라 (산호 1개(70%), 2개(30%)/ 산호가지 0개(50%), 1개(50%))
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 1:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 2:
                                            if (Random.Range(0, 100) <= 70)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 50)
                                                inven.AddSlot(62, 1);
                                            break;
                                    }
                                    break;

                                case 6://글로우 플루오 (해초 씨앗 1개(85%), 2개(15%) / 해초 1개(35%), 2개(65%)
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0://(해초 씨앗 1개)
                                            inven.AddSlot(nowId, 1);
                                            break;

                                        case 1://(해초 씨앗 1개 / 해초 1개(35 %)
                                            inven.AddSlot(nowId, 1);
                                            if (Random.Range(0, 100) <= 35)
                                                inven.AddSlot(63, 1);
                                            break;

                                        case 2://아크로포라 (해초 씨앗 1개(85%), 2개(15%) / 해초 1개(35%), 2개(65%)
                                            if (Random.Range(0, 100) <= 85)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 35)
                                                inven.AddSlot(63, 1);
                                            else
                                                inven.AddSlot(63, 2);
                                            break;
                                    }
                                    break;

                                case 7://펌핑 제니아 (산호 1개(70%), 2개(30%)/ 산호가지 0개(50%), 1개(50%))
                                    switch (nowObj.GetComponent<Coral>().index)
                                    {
                                        case 0:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 1:
                                            inven.AddSlot(nowId, 1);
                                            break;
                                        case 2:
                                            if (Random.Range(0, 100) <= 70)
                                                inven.AddSlot(nowId, 1);
                                            else
                                                inven.AddSlot(nowId, 2);
                                            if (Random.Range(0, 100) <= 50)
                                                inven.AddSlot(62, 1);
                                            break;
                                    }
                                    break;
                            }
                            Destroy(nowObj);
                        }
                        else if(nowId >= 41 && nowId <= 47)
                        {
                            QuestImage.instance.GetGem[nowId - 41]++;
                            QuestImage.instance.GetGemAll++;
                            nowObj.GetComponent<Gem>().resources--;
                            nowObj.GetComponent<Gem>().GemActiveFalse();
                            inven.AddSlot(nowId, 1);
                        }
                        else if (inven.AddSlot(nowId, 1))
                        {
                            if(nowId == 33)
                            {
                                AirManager.instance.AirGenList.Remove(hit.transform.GetComponent<AirGen>());
                                AirManager.instance.AirGen_Update();
                            }
                            Destroy(nowObj);
                        }
                        else
                        {
                            uiManager.Alarm("인벤토리에 공간이 없습니다!!");
                        }
                        Redslider.fillAmount = 0;
                        Red.SetActive(false);
                        ClickMove.instance.on = false;
                        isClick = false;
                        ClickTime = 0;
                    }
                    else
                        Planting();
                }
            }
            else
            {
                animator.SetBool("isPicking", false);
            }
            if (canPick)
            {
                LPickImg.sprite = pickSprite;
                RPickImg.sprite = pickSprite;
            }
            else
            {
                LPickImg.sprite = plantSprite;
                RPickImg.sprite = plantSprite;
            }
        }
        else
        {
            RModuleIcon.SetActive(true);
            LModuleIcon.SetActive(true);
        }
    }

    public void UseDown()
    {
        if (isRidden || uiManager.screenState != UIManager.ScreenState.None)
            return;
        switch (inven.InvenId[12])
        {
            case 21: //유도 표식
                OnCompass = !OnCompass;
                Compass.SetActive(OnCompass);
                break;
            case 24: //조명
                break;
            case 26: //삽
                inven.AddSlot(51, 1);
                break;
        }
    }

    public void PickDown()
    {
        if (isRidden || uiManager.screenState != UIManager.ScreenState.None)
            return;
        if (canPick)
        {
            isClick = true;
            ClickMove.instance.on = false;
        }

        if (handId > 0 && handId <= 20) //해초/산호류
        {
            RaycastHit hit;
            Vector3 pos = player.position + transform.forward * 1.5f;
            Debug.DrawRay(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down);
            if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, out hit, 10f))
            {
                if (hit.collider.gameObject.CompareTag("Soil"))
                {
                    isClick = true;
                    ClickMove.instance.on = false;
                }
            }
            if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, out hit, 10f, ItemMask))
            {
                if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length == 0)
                {
                    Debug.Log("isClick");
                    isClick = true;
                    ClickMove.instance.on = false;
                }
                else
                {
                    Debug.Log(Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length);
                }
            }
            else
            {
                Debug.Log("siba");
            }
        }
        else if (handId > 20 && handId <= 30) //도구
        {
        }
        else if (handId > 30 && handId <= 40) //모듈
        {
            RaycastHit hit;
            Vector3 pos = player.position + transform.forward * 1.5f;
            Debug.DrawRay(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, Color.red, 10);
            if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, out hit, 10f, ItemMask))
            {
                if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length == 0)
                {
                    isClick = true;
                    ClickMove.instance.on = false;
                }
            }
        }
        else if (handId > 40 && handId <= 50) //기타 아이템
        {
        }
        else if (handId > 50 && handId <= 60) //비옥토
        {
            RaycastHit hit;
            Vector3 pos = player.position + transform.forward * 1.5f;
            Debug.DrawRay(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, Color.red, 10);
            if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, out hit, 10f, ItemMask))
            {
                if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length == 0)
                {
                    isClick = true;
                    ClickMove.instance.on = false;
                }
            }
        }
        else if (handId > 60 && handId <= 70) //조합용 아이템
        {
        }
    }

    public void PickUp()
    {
        if (isRidden)
            return;
        isClick = false;
        ClickMove.instance.on = true;
        Redslider.fillAmount = 0;
        Red.SetActive(false);

        ClickTime = 0;
    }

    public void ModuleUse()
    {
        if (isRidden)
            TakeOffCar();

        else if (isFish)
            FishTalk();

        else if (isSub)
        {
            QuestImage.instance.CheckQuest();
            QuestImage.instance.QuestList.SetActive(true);
            QuestImage.instance.isOpen = true;
        }

        else if (nowId == 31) //로버
            RideCar();

        else if (nowId == 32 && !uiManager.isCrafting) //프린터
            uiManager.PrinterButton();

        else if (nowId == 34 && !uiManager.isCrafting) //혼합기
            uiManager.BlenderButton();

        else if (nowId == 35 && !uiManager.isCrafting) //표지판
            Debug.Log("추가 바람");
    }

    void FishTalk()
    {
        inven.RemoveSlot(handId, 1);
        nowFish.GetComponent<FishAI>().Feeding(handId);
        isFeeding = true;
    }

    void Planting()
    {
        Redslider.fillAmount = 0;
        Red.SetActive(false);
        RaycastHit hit;
        Vector3 pos = player.position + transform.forward * 1.5f;
        if (Physics.Raycast(new Vector3(Mathf.Round(pos.x), pos.y + 1, Mathf.Round(pos.z)), Vector3.down, out hit, 10f, ItemMask))
        {
            if (handId < 20)
            {
                if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, soilMask).Length == 0)
                {
                    GameObject newBlock = Instantiate(inven.itemInfo[handId].Block, hit.point, Quaternion.identity);
                    inven.InvenCount[inven.SelectedHot + 14]--;
                    inven.SlotUpdate(inven.SelectedHot + 14);

                    foreach (Collider col in Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity))
                    {
                        if (col.CompareTag("Soil"))
                        {
                            switch (col.gameObject.name)
                            {
                                case "Soil(Clone)":
                                    newBlock.GetComponent<Coral>().GrowSpeed = 0.95f;
                                    break;
                                case "FerSoil(Clone)":
                                    newBlock.GetComponent<Coral>().GrowSpeed = 0.7f;
                                    break;
                                case "BetterFerSoil(Clone)":
                                    newBlock.GetComponent<Coral>().GrowSpeed = 0.5f;
                                    break;
                            }
                        }
                    }
                }
            }
            else if(handId == 31)
            {
                if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length == 0)
                {
                    Instantiate(inven.itemInfo[31].Block, hit.point, Quaternion.Euler(Vector3.up * 90));
                    inven.InvenCount[inven.SelectedHot + 14]--;
                    inven.SlotUpdate(inven.SelectedHot + 14);
                }
            }
            else
            {
                if (Physics.OverlapBox(hit.point, Vector3.one * 0.5f, Quaternion.identity, layerMask).Length == 0)
                {
                    GameObject newBlock = Instantiate(inven.itemInfo[handId].Block, hit.point, Quaternion.identity);
                    inven.InvenCount[inven.SelectedHot + 14]--;
                    inven.SlotUpdate(inven.SelectedHot + 14);
                    if (handId == 33)
                    {
                        AirManager.instance.AirGen_Add(newBlock.GetComponent<AirGen>());
                        newBlock.transform.rotation = Quaternion.identity;
                    }
                }
            }
        }
        Redslider.fillAmount = 0;
        Red.SetActive(false);
        ClickMove.instance.on = false;
        isClick = false;
        ClickTime = 0;
    }

    public void RideCar()
    {
        isRidden = true;
        RideObj = nowObj.GetComponent<CharacterController>();
        StartCoroutine("RideAni");

        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(RideObj.transform);
        transform.position = RideObj.transform.position;
        transform.rotation = RideObj.transform.rotation;
        selectCircle.gameObject.SetActive(false);

        animator.SetBool("isRiding", true);
    }

    public void TakeOffCar()
    {
        isRidden = false;
        StopCoroutine("RideAni");
        RideObj.center = new Vector3(0, 1.5f, 0);
        player.transform.parent = null;
        transform.position = RideObj.transform.position + Vector3.forward * -1.7f;
        transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        RideObj = null;

        animator.SetBool("isRiding", false);
    }

    IEnumerator RideAni()
    {
        for(int i = 0; i < 10; i++)
        {
            RideObj.center = new Vector3(0, 1.5f - 0.03f * i, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator AirOff()
    {
        isOver = true;
        ClickMove.instance.on = false;
        fade.gameObject.SetActive(true);
        for (float i = 0; i <= 50; i++)
        {
            fade.color = new Color(0, 0, 0, i / 50);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
        player.position = home.position;
        air = Maxair;
        yield return new WaitForSeconds(1f);
        for (float i = 0; i <= 50; i++)
        {
            fade.color = new Color(0, 0, 0, 1 - i / 50);
            yield return new WaitForSeconds(0.04f);
        }
        fade.gameObject.SetActive(false);
        ClickMove.instance.on = true;
        isOver = false;
    }
}
