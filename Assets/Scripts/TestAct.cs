using UnityEngine;

public class TestAct : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<ActCode>().onAct += OnAct;
    }

    void OnAct(GameObject bullet)
    {
        Destroy(bullet);
        GetComponentInChildren<ActCode>().SwitchToMercySprite();
        Dialogue.UpdateText("* You acted on the enemy.");
    }
}
