using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Sets the text value in the HP UI element
/// </summary>
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI PlayerHpTextbox;
    private void Awake()
    {
        EventManagerGlobal.Instance.EventTakeDamage.AddListener(UpdateHealth);
        UpdateHealth(60);
    }
    public void UpdateHealth(int health)
    {
        PlayerHpTextbox.text = health.ToString();
    }
}
