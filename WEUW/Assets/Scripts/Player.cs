using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float maxSpeed = 5f;
    public float jumpforce = 1000f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public Alpha nearWord = null;

    bool isJump = false;
    bool isClimb = false;
    bool isNearWord = false;
    int jumpCount = 0;


    private static Player instance;
    public static Player Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 키입력 받는 부분
        if (Input.GetButtonDown("Jump"))
            isJump = true;

        GetInput();

        Move();
        Jump();
        useGravity();
    }
    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.C)) selectNearWord();
    }

    private void Move()
    {
        Vector3 veloX = Vector3.zero;
        Vector3 veloY = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            veloX = Vector3.left;
            sr.flipX = true;
            anim.SetBool("run", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            veloX = Vector3.right;
            sr.flipX = false;
            anim.SetBool("run", true);
        }
        else
            anim.SetBool("run", false);

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            veloX = Vector3.down;
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            veloX = Vector3.up;
        }


        transform.position += veloX * speed * Time.deltaTime;

    }

    void Jump()
    {
        if (!isJump || jumpCount > 1) return;

        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        isJump = false;
        jumpCount++;
        anim.SetBool("jump", true);

    }
    void useGravity()
    {
        if (isClimb)
        {
            rb.gravityScale = 0;
            return;
        }
        rb.gravityScale = 2;

    }

    void selectNearWord()
    {
        if (isNearWord)
        {
            GameManager gm = GameManager.Instance;
            Debug.Log("keydown C");

            gm.WordUIChange(nearWord);
            nearWord.End();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체의 충돌 표면이 위쪽을 향하는 것을 확인.
        if (collision.contacts[0].normal.y > 0.7f)
        {
            jumpCount = 0;
            isJump = false;
            anim.SetBool("jump", false);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ladder"))
        {
            anim.SetBool("jump", false);
            isClimb = true;
            jumpCount = 1;
            rb.velocity = Vector3.zero;
        }
        if (collision.CompareTag("word"))
        {
            Debug.Log("triggerenter");
            isNearWord = true;
            nearWord = collision.GetComponent<Alpha>();

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ladder"))
            isClimb = false;

        if (collision.CompareTag("word"))
        {
            isNearWord = false;
            nearWord = null;
        }
    }
}
    