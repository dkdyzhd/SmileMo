using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance = null;

    public TMPro.TextMeshProUGUI itemCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ItemCountToText();
    }

    void ItemCountToText()
    {
       itemCount.text = PlayerController.Instance.itemCount.ToString();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
