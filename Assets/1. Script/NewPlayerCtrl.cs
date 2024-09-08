using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerCtrl : MonoBehaviour
{
    public static NewPlayerCtrl Instance = null;

    private Animator animator;
    private Camera camera;
    private Rigidbody2D rig;
    private SpriteRenderer sr;

    public List<Transform> responPoints = new List<Transform>();

    Transform respawnPoint;

    public float walkSpeed = 3.0f;
    public float jumpForce = 680.0f;
    public float sprintSpeed = 5.0f;
    public int itemCount = 0;

    private bool canDoubleJump = false;
    public CheckGroundColl m_CkGrdColl = null;
    int m_ReserveJump = 0;      //점프 예약 변수

    public float knockbackForce = 250.0f;   // 좌우 방향으로 밀리는 힘
    public float knockbackVForce = 350.0f;  // 수직 방향으로 밀리는 힘 //knockbackVerticalForce
    public float knockbackDuration = 0.6f; // 밀리는 시간
    private bool isKnockedBack = false;  // 밀림 상태 여부

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

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();

        //넉백 코루틴 구현 전에 타이머를 이용해 일정시간동안 밀리게함
        //if (0.0f < a_Timer)
        //{
        //    a_Timer -= Time.deltaTime;
        //    rig.AddForce(new Vector3(-15.0f, 0.0f, 0.0f), ForceMode2D.Impulse);
        //}
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    //float a_Timer = 0.0f;

    void MoveUpdate()
    {
        if (isKnockedBack == true)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //캐릭터 이동
        rig.velocity = new Vector2((horizontal * walkSpeed), rig.velocity.y);

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
                rig.velocity = new Vector2((horizontal * sprintSpeed), rig.velocity.y);
            }
        }

        else    //idle
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
            animator.SetBool("isWalk", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            m_ReserveJump = 3;
        }
        if (m_CkGrdColl.isGrounded == true)
        {
            if (0 < m_ReserveJump)
            {
                this.rig.velocity = new Vector2(rig.velocity.x, 0.0f);
                this.rig.AddForce(transform.up * this.jumpForce);
                animator.SetTrigger("Jump");
                m_ReserveJump = 0;
                canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") == true && canDoubleJump == true)
            {
                this.rig.velocity = new Vector2(rig.velocity.x, 0.0f);
                this.rig.AddForce(transform.up * this.jumpForce);
                animator.SetTrigger("Jump");
                canDoubleJump = false;
            }
        }

        if (0 < m_ReserveJump)
            m_ReserveJump--;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            Debug.Log("스파이크 충돌");
            animator.SetTrigger("Hit");

            KnockBack(other);
            //a_Timer = 0.1f;

        }

        if (other.gameObject.tag == "Monster")
        {
            Debug.Log("몬스터 충돌");
            animator.SetTrigger("Hit");

            KnockBack(other);
        }
    }

    // 일정 시간 동안 밀림 상태를 유지하는 코루틴
    IEnumerator KnockbackCoroutine()
    {
        isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }
    void KnockBack(Collision2D other)
    {
        Vector2 knockbackDir = transform.position - other.transform.position;
        if (knockbackDir.x <= 0.0f)
            knockbackDir.x = -1.0f;  
        else
            knockbackDir.x = -1.0f; //무조건 뒤로가게 하고 싶어서 음수로 변환

        // 밀리는 힘 적용
        rig.velocity = Vector2.zero;  // 현재 속도 초기화
        rig.AddForce(new Vector2(knockbackDir.x * knockbackForce, knockbackVForce));

        // 밀림 상태 설정
        StartCoroutine(KnockbackCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            itemCount++;
            Destroy(other.gameObject);
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
