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
    float timeSinceDone = 0f;

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
            timeSinceDone = 0f;

        }
        if (index < text.Length)
        {
            displayText += text[index];
            index++;
        } else
        {
            timeSinceDone += Time.deltaTime;
            if (timeSinceDone > 2f)
            {
                UpdateText("");
            }
        }
        textMesh.text = displayText;

    }
}
