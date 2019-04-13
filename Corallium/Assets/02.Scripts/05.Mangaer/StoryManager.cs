using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour {

    //이벤트
    /// <summary>
    /// 1. 튜토리얼
    /// 2. 동굴
    /// 3. 엔딩
    /// </summary>
    /// 

    public CraftUI[] craftUI = new CraftUI[7];
    public GameObject siyunObject;
    public GameObject siyunOnIcon;
    public GameObject siyunOffIcon;

    public Sprite cutScene_Ending01;
    public Sprite cutScene_Ending02;
    public Sprite cutScene_Ending03;
    public Sprite cutScene_Ending04;
    public GameObject GameClear;
    public GameObject ClearBtn;

    public static StoryManager instance;

    public Image fade;

    private void Awake()
    {
        instance = this;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ending();
        }
    }

    public void EnterCave()
    {

    }

    public void SiyunMode(bool on)
    {
        if (on)
        {
            for(int i = 0; i < 7; i++)
                craftUI[i].SIyunMode = true;
            siyunObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < 7; i++)
                craftUI[i].SIyunMode = false;
            siyunObject.SetActive(false);
        }
        siyunOnIcon.SetActive(on);
        siyunOffIcon.SetActive(!on);
    }

    public void Ending()
    {
        StartCoroutine("EndingAnim");
    }

    IEnumerator EndingAnim()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine("FadeInAnim", 0.03f);
        yield return new WaitForSeconds(1f);
        fade.color = new Color(255, 255, 255, 1);

        fade.sprite = cutScene_Ending01;
        yield return new WaitForSeconds(2.2f);
        fade.sprite = cutScene_Ending02;
        yield return new WaitForSeconds(2.2f);
        fade.sprite = cutScene_Ending03;
        yield return new WaitForSeconds(2.2f);
        fade.sprite = cutScene_Ending04;
        yield return new WaitForSeconds(1.8f);
        GameClear.SetActive(true);
        yield return new WaitForSeconds(1);
        ClearBtn.SetActive(true);
    }

    IEnumerator FadeOutAnim(float time)
    {
        fade.gameObject.SetActive(true);
        for (float i = 0; i <= 50; i++)
        {
            fade.color = new Color(0,0, 0, 1 - i / 50);
            yield return new WaitForSeconds(time);
        }
    }
    IEnumerator FadeInAnim(float time)
    {
        fade.gameObject.SetActive(true);
        for (float i = 0; i <= 50; i++)
        {
            fade.color = new Color(0, 0, 0, i / 50);
            yield return new WaitForSeconds(time);
        }
    }
}
