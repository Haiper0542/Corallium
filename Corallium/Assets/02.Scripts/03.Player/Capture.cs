using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Capture : MonoBehaviour
{
    bool on = false;
    public GameObject CameraCanvas;
    public GameObject InvenCanvas;
    public GameObject IconCanvas;

    public void CameraMode()
    {
        on = !on;
        if (on)
        {
            InvenCanvas.SetActive(false);
            IconCanvas.SetActive(false);
            CameraCanvas.SetActive(true);
        }
        else
        {
            InvenCanvas.SetActive(true);
            IconCanvas.SetActive(true);
            CameraCanvas.SetActive(false);
        }
    }

    public void TakeShot()
    {
        CameraCanvas.SetActive(false);
        StartCoroutine("ScreenShot");
        CameraCanvas.SetActive(true);
    }

    IEnumerator ScreenShot()
    {
        yield return new WaitForEndOfFrame();

        byte[] imageByte;
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);

        tex.Apply();

        imageByte = tex.EncodeToPNG();
        DestroyImmediate(tex);

        if (!Directory.Exists(Application.persistentDataPath + "/Corallium_Gallary"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Corallium_Gallary");
        }

        File.WriteAllBytes(Application.persistentDataPath + "/Corallium_Gallary/" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss").ToString() + ".png", imageByte);
    }
}