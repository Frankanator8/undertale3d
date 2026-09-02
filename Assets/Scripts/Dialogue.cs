using System;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    String displayText;
    int index;
    
    public static String text;
    static bool updateText = false;

    public static void UpdateText(String newText)
    {
        text = newText;
        updateText = true;
    }
    
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        UpdateText("");
    }

    // Update is called once per frame
    void Update()
    {
        if (updateText)
        {
            index = 0;
            updateText = false;
            displayText = "";
        }
        if (index < text.Length)
        {
            displayText += text[index];
            index++;
        }
        textMesh.text = displayText;

    }
}
