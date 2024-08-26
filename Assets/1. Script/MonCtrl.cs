using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonCtrl : MonoBehaviour
{
    Rigidbody2D rig;
    Animator animator;
    SpriteRenderer sr;

    //���̸� ������ ���� ����
    Vector3 rayPos;

    //��޸��Ͱ� �̵��� ����
    int dir;

    //���� ��ȯ
    bool isChange;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        RandomDir();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        rig.velocity = new Vector2(dir, rig.velocity.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayPos, Vector2.down * 1);

        //���� ��ü�� ���ٸ�(�����������)
        if (!hit)
        {
            //���� ��ȯ
            ChangeDir();
        }

        //�¾Ҵٸ�
        else
        {
            //�ٽ� ������ �ٲ� �� �ֵ��� ����
            isChange = false;
            
        }

        switch (dir)
        {
            //�����ִٸ�
            case 0:
                animator.SetBool("isWalk", false);
                break;

            //�������� ���ٸ�
            case -1:
                sr.flipX = true;
                animator.SetBool("isWalk", true);
                rayPos = new Vector3(-1.0f, 0, 0);
                break;

            //���������� ���ٸ�
            case 1:
                sr.flipX = false;
                animator.SetBool("isWalk", true);
                rayPos = new Vector3(1.0f, 0, 0);
                break;

        }
    }

    void ChangeDir()
    {
        //���� ��ȯ
        dir *= -1;

        //������ȯ ����
        isChange = true;

        //�κ�ũ���
        CancelInvoke();

        Invoke("RandomDir", Random.Range(2f, 3f));
    }

    void RandomDir()
    {
        dir = Random.Range(-1, 2);

        Invoke("RandomDir", Random.Range(2f, 3f));
    }
}
