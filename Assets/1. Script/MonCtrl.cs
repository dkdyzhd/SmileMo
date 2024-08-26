using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonCtrl : MonoBehaviour
{
    Rigidbody2D rig;
    Animator animator;
    SpriteRenderer sr;

    //레이를 저장할 변수 선언
    Vector3 rayPos;

    //상급몬스터가 이동할 방향
    int dir;

    //방향 전환
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

        //맞은 물체가 없다면(낭떨어지라면)
        if (!hit)
        {
            //방향 전환
            ChangeDir();
        }

        //맞았다면
        else
        {
            //다시 방향을 바꿀 수 있도록 저장
            isChange = false;
            
        }

        switch (dir)
        {
            //멈춰있다면
            case 0:
                animator.SetBool("isWalk", false);
                break;

            //왼쪽으로 간다면
            case -1:
                sr.flipX = true;
                animator.SetBool("isWalk", true);
                rayPos = new Vector3(-1.0f, 0, 0);
                break;

            //오른쪽으로 간다면
            case 1:
                sr.flipX = false;
                animator.SetBool("isWalk", true);
                rayPos = new Vector3(1.0f, 0, 0);
                break;

        }
    }

    void ChangeDir()
    {
        //방향 전환
        dir *= -1;

        //방향전환 저장
        isChange = true;

        //인보크취소
        CancelInvoke();

        Invoke("RandomDir", Random.Range(2f, 3f));
    }

    void RandomDir()
    {
        dir = Random.Range(-1, 2);

        Invoke("RandomDir", Random.Range(2f, 3f));
    }
}
