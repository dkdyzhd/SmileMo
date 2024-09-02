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
        //유니티에디터 실행하는 중일 때
#if UNITY_EDITOR
        //유니티 에디터 종료
        UnityEditor.EditorApplication.isPlaying = false;

        //빌드파일 실행중일 때 빌드 파일 종료
#else
    Application.Quit();

#endif

    }
}
