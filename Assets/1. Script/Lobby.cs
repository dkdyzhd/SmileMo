using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitBtn()
    {
        //����Ƽ������ �����ϴ� ���� ��
#if UNITY_EDITOR
        //����Ƽ ������ ����
        UnityEditor.EditorApplication.isPlaying = false;

        //�������� �������� �� ���� ���� ����
#else
    Application.Quit();

#endif

    }
}
