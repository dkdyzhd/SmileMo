using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCtrl : MonoBehaviour
{
    public Transform player;

    //ī�޶� �̵��ӵ�
    public float cameraSpeed;

    //ī�޶� ���� ������ �߽ɰ� ũ��
    public Vector2 CamCenter, CamSize;

    //ī�޶� ���α���, ���α����� ����
    float heightHalf, widthHalf;

    private void Start()
    {
        // orthographic : ���� �������� ���ٹ��� ���� 2Dȭ�鿡�� ���
        //ī�޶� ���� ������ ����
        heightHalf = Camera.main.orthographicSize;

        //ī�޶� ���� ������ ����
        widthHalf = (float)Screen.width / (float)Screen.height * heightHalf;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;   //���������� ��Ÿ����

        Gizmos.DrawWireCube(CamCenter, CamSize);    //ī�޶� ��������
    }

    private void LateUpdate()
    {
        Vector3 playerTarget = new Vector3(player.position.x, player.position.y, transform.position.z);

        //�ε巴�� ����ٴϵ���
        transform.position = Vector3.Lerp(transform.position, playerTarget, Time.deltaTime * cameraSpeed);  

        float limitX = CamSize.x * 0.5f - widthHalf;
        float limitY = CamSize.y * 0.5f - heightHalf;

        float clampX = Mathf.Clamp(transform.position.x, CamCenter.x - limitX, CamCenter.x + limitX);
        float clampY = Mathf.Clamp(transform.position.y, CamCenter.y - limitY, CamCenter.y + limitY);

        //ī�޶� ���� ����
        transform.position = new Vector3(clampX, clampY,transform.position.z);
    }
}
