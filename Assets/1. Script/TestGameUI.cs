using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameUI : MonoBehaviour
{
    public static TestGameUI Instance = null;

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
        //itemCount.text = PlayerController.Instance.itemCount.ToString();
        itemCount.text = TestPlayerScript.Instance.itemCount.ToString();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
