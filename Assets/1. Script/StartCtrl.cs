using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;  //�ؽ�Ʈȿ�� ��Ÿ�� ��

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

    //IEnumerator Typing(string messasge)   //��� ���� �Ϳ� ���� ��
    //{
    //    yield return null;
    //}
}
