using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;  //텍스트효과 나타낼 것

public class StartCtrl : MonoBehaviour
{
    //public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    //IEnumerator Typing(string messasge)   //대사 같은 것에 쓰면 됨
    //{
    //    yield return null;
    //}
}
