using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Camera camera;
    private Rigidbody2D rig;
    private SpriteRenderer sr;

    public float speed =3.0f;
    public float sprintSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public int jumpCount = 0;


    private bool isSprint = false;
    private Vector2 move;
    Vector2 originPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        camera = GetComponent<Camera>();
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originPos = transform.position;
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rig.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprint = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprint = false;
        }

        if (horizontal != 0)
        {
            animator.SetBool("isWalk", true);

            // horizontal 이 왼쪽이라면 실행
            sr.flipX = Input.GetAxisRaw("Horizontal") == -1;

            if (isSprint)    //run
            {
                if (rig.velocity.x > sprintSpeed)
                {
                    rig.velocity = new Vector2(sprintSpeed, rig.velocity.y);
                }
                if (rig.velocity.x < -speed)
                {
                    rig.velocity = new Vector2(-sprintSpeed, rig.velocity.y);
                }
            }

            else    //walk
            {
                if (rig.velocity.x > speed)
                {
                    rig.velocity = new Vector2(speed, rig.velocity.y);
                }
                if (rig.velocity.x < -speed)
                {
                    rig.velocity = new Vector2(-speed, rig.velocity.y);
                }
            }
        }

        else    //idle
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
            animator.SetBool("isWalk", false);
        }
    }
    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            jumpCount++;
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    private void Update()
    {
        Move();
        Jump();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
        }

        if(collision.gameObject.tag == "Spike")
        {
            Debug.Log("스파이크 충돌");
            animator.SetTrigger("Hit");
            Vector2 targetPos = originPos - new Vector2(2, 0);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 1);
        }
    }
}
