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
    int m_ReserveJump = 0;      //���� ���� ����

    public float knockbackForce = 250.0f;   // �¿� �������� �и��� ��
    public float knockbackVForce = 350.0f;  // ���� �������� �и��� �� //knockbackVerticalForce
    public float knockbackDuration = 0.6f; // �и��� �ð�
    private bool isKnockedBack = false;  // �и� ���� ����

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

        //�˹� �ڷ�ƾ ���� ���� Ÿ�̸Ӹ� �̿��� �����ð����� �и�����
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

        //ĳ���� �̵�
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

            // horizontal �� �����̶�� ����
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
            Debug.Log("������ũ �浹");
            animator.SetTrigger("Hit");

            KnockBack(other);
            //a_Timer = 0.1f;

        }

        if (other.gameObject.tag == "Monster")
        {
            Debug.Log("���� �浹");
            animator.SetTrigger("Hit");

            KnockBack(other);
        }
    }

    // ���� �ð� ���� �и� ���¸� �����ϴ� �ڷ�ƾ
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
            knockbackDir.x = -1.0f; //������ �ڷΰ��� �ϰ� �; ������ ��ȯ

        // �и��� �� ����
        rig.velocity = Vector2.zero;  // ���� �ӵ� �ʱ�ȭ
        rig.AddForce(new Vector2(knockbackDir.x * knockbackForce, knockbackVForce));

        // �и� ���� ����
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
