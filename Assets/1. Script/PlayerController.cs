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

            // horizontal �� �����̶�� ����
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
            Debug.Log("������ũ �浹");
            animator.SetTrigger("Hit");
            //Vector2 targetPos = originPos - new Vector2(200, 0);
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, 1);
            //collision�� �� ���� �Ͼ�� �۾� -> Vector���� �ƹ��� ���� �൵ �ε����� �� 
            //MoveTowards�� �ӵ� 1 ��ŭ �� �����Ӹ� �̵��ϰ� ������ ������ ȿ���� ������ ��
            //Ÿ�̸Ӹ� ����ؼ� �� �и��� �ϴ� ����� ����

            //Vector2 CacPos = transform.position + new Vector3(-3.0f, 0.0f, 0.0f);
            //transform.position = CacPos;
            //���� position���� �����̴� ���� ���ڿ�������
            //ĳ���� ���¸� �������Ƽ� �˹���°� �Ǿ����� �ڷ� �и����� �غ���
            //to do : ĳ���� ���� ����� (walk / jump / knockback ... )


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
            Debug.Log("���� �浹");
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
