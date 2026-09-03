using UnityEngine;

public class SansAct : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<ActCode>().onAct += OnAct;
    }

    void OnAct(GameObject bullet)
    {
        GetComponentInChildren<ActCode>().SwitchToMercySprite();
        Dialogue.UpdateText("* Can't keep dodging forever. Keep attacking.");
    }
}
