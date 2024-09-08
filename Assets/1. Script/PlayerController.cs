using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    private Animator animator;
    private Camera camera;
    private Rigidbody2D rig;
    private SpriteRenderer sr;

    public List<Transform> responPoints = new List<Transform>();

    Transform respawnPoint;

    public float speed =3.0f;
    public float sprintSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public int jumpCount = 0;
    public int itemCount = 0;


    private bool isSprint = false;
    private Vector2 move;
    Vector2 originPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        camera = GetComponent<Camera>();
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Instance = this;
    }

    private void Start()
    {
        originPos = transform.position;
        respawnPoint = responPoints[0];
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

        if (0.0f < a_Timer)
        {
            a_Timer -= Time.deltaTime;
            rig.AddForce(new Vector3(-15.0f, 0.0f, 0.0f), ForceMode2D.Impulse);
        }

    }

    private void OnDestroy()
    {
        Instance = null;
    }

    float a_Timer = 0.0f;
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
            //Vector2 targetPos = originPos - new Vector2(200, 0);
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, 1);
            //collision은 한 번만 일어나는 작업 -> Vector값을 아무리 많이 줘도 부딪혔을 때 
            //MoveTowards의 속도 1 만큼 한 프레임만 이동하고 끝나기 때문에 효과가 없었던 것
            //타이머를 사용해서 쭉 밀리게 하는 방법도 있음

            //Vector2 CacPos = transform.position + new Vector3(-3.0f, 0.0f, 0.0f);
            //transform.position = CacPos;
            //현재 position으로 움직이는 것은 부자연스러움
            //캐릭터 상태를 만들어놓아서 넉백상태가 되었을때 뒤로 밀리도록 해보기
            //to do : 캐릭터 상태 만들기 (walk / jump / knockback ... )


            a_Timer = 0.1f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Item")
        {
            itemCount++;
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Monster")
        {
            Debug.Log("몬스터 충돌");
            animator.SetTrigger("Hit");
            a_Timer = 0.1f;
        }

        switch (other.gameObject.name)
        {
            //case "StartPoint":
            //    if (responPoints.IndexOf(respawnPoint) < 1)
            //    {
            //        respawnPoint = responPoints[0];
            //    }
            //    break;

            case "Trap":
                {
                    respawnPoint = responPoints[0];
                    transform.position = respawnPoint.position;
                }
                break;
        }
    }
}
