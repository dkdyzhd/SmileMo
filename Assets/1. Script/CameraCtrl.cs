using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCtrl : MonoBehaviour
{
    public Transform player;

    //카메라 이동속도
    public float cameraSpeed;

    //카메라 제한 영역의 중심과 크기
    public Vector2 CamCenter, CamSize;

    //카메라 세로길이, 가로길이의 절반
    float heightHalf, widthHalf;

    private void Start()
    {
        // orthographic : 직각 투명으로 원근법이 없는 2D화면에서 사용
        //카메라 세로 길이의 절반
        heightHalf = Camera.main.orthographicSize;

        //카메라 가로 길이의 절반
        widthHalf = (float)Screen.width / (float)Screen.height * heightHalf;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;   //빨간색으로 나타내기

        Gizmos.DrawWireCube(CamCenter, CamSize);    //카메라 영역제한
    }

    private void LateUpdate()
    {
        Vector3 playerTarget = new Vector3(player.position.x, player.position.y, transform.position.z);

        //부드럽게 따라다니도록
        transform.position = Vector3.Lerp(transform.position, playerTarget, Time.deltaTime * cameraSpeed);  

        float limitX = CamSize.x * 0.5f - widthHalf;
        float limitY = CamSize.y * 0.5f - heightHalf;

        float clampX = Mathf.Clamp(transform.position.x, CamCenter.x - limitX, CamCenter.x + limitX);
        float clampY = Mathf.Clamp(transform.position.y, CamCenter.y - limitY, CamCenter.y + limitY);

        //카메라에 제한 적용
        transform.position = new Vector3(clampX, clampY,transform.position.z);
    }
}
