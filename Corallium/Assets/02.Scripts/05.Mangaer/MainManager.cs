using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject ExitScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitScreen.SetActive(true);
            Application.Quit();
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MainSceneButton()
    {
        SceneManager.LoadScene(0);
    }

    public void DictButton()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitCancle()
    {
        ExitScreen.SetActive(false);
    }

    public void ExitConfirm()
    {
        Application.Quit();
    }
}
