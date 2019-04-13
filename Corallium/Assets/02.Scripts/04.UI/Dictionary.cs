using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Dictionary : MonoBehaviour
{
    [SerializeField]
    public Image dictImg;
    public string[] files = null;

    int whichShown = 0;

    private void Start()
    {
        if(Directory.Exists(Application.persistentDataPath + "/Corallium_Gallary"))
        {
            files = Directory.GetFiles(Application.persistentDataPath + "/Corallium_Gallary", "*.png");
            if (files.Length > 0)
            {
                GetPictureAndShowIt();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            GetPictureAndShowIt();
    }

    void GetPictureAndShowIt()
    {
        string pathToFile = files[whichShown];
        Texture2D texture = GetScreenshotImage(pathToFile);
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        dictImg.GetComponent<Image>().sprite = sp;
    }

    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;
        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }
        return texture;
    }

    public void NextPicture()
    {
        if (files.Length > 0)
        {
            whichShown += 1;
            if (whichShown > files.Length - 1)
                whichShown = 0;
            GetPictureAndShowIt();
        }
    }
}
