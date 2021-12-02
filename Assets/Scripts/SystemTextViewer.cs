using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SystemType { Money = 0, Build, NextWave }
public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;

    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch(type)
        {
            case SystemType.Money:
                textSystem.text = "Not enough gold";
                break;
            case SystemType.Build:
                textSystem.text = "Invalid build tower";
                break;
            case SystemType.NextWave:
                textSystem.text = "Wave incoming";
                break;

        }
        tmpAlpha.FadeOut();
    }
}
