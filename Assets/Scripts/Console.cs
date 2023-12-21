using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
{
    public static Console instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text textBox;

    private List<string> outputs = new List<string>();

    public void ConsoleLog(string output)
    {
        if(outputs.Count >= 3) outputs.RemoveAt(0);
        outputs.Add(output);
        UpdateTextbox();
    }

    private void UpdateTextbox()
    {
        string output = "";

        foreach(string str in outputs)
        {
            output += str;
            output += "\n";
        }
        textBox.text = output;
    }
}
