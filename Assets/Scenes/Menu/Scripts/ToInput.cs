using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToInput : MonoBehaviour
{
    private InputField field;
    private Settings settings;
    void Awake()
    {
        field = GetComponent<InputField>();
        settings = GameObject.FindGameObjectWithTag("settings").GetComponent<Settings>();
        field.text = settings.enemiesMaxCount.ToString();
    }

    public void HandleInput(string input)
    {
        if (field.text == "") { return; }
        try
        {
            int intValue = Int32.Parse(field.text);
            settings.enemiesMaxCount = intValue;
        }
        catch (FormatException)
        {
            field.text = settings.enemiesMaxCount.ToString();
        }
    }
}
