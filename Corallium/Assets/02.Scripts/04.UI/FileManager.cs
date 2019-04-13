using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{
    public Inventory inven;

    string m_strPath = "Assets/";
    public TextAsset textAsset;

    private void Awake()
    {
        Parse();
    }

    public void WriteData(string strData)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
        writer.WriteLine(strData);
        writer.Close();
    }

    public void Parse()
    {
        StringReader sr = new StringReader(textAsset.text);

        string source = sr.ReadLine();
        string[] values;

        int i = 1;
        while (source != null)
        {
            values = source.Split(',');
            if (values.Length == 0)
            {
                sr.Close();
                return;
            }
            source = sr.ReadLine();
            inven.itemInfo[i].ItemTxt = values[0];
            inven.itemInfo[i].ItemName = values[1];
            inven.itemInfo[i].ItemExp = values[2];
            i++;
        }
    }
}
