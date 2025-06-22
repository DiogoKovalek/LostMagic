using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExibeText : MonoBehaviour, IText {
    private TextMeshProUGUI gui;
    public void UpdateText(string str) {
        if (gui == null) gui = GetComponent<TextMeshProUGUI>();
        gui.text = str;
    }
}
public interface IText {
    public void UpdateText(String str);
}
