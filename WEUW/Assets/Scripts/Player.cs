using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float maxSpeed = 5f;
    public float jumpforce = 1000f;
    public float maxJumpforce = 10;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;


    bool isJump = false;
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
        if (Input.GetButtonDown("Jump"))
            isJump = true;



        Move();
        Jump();
    }

    private void Move()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 vec = new Vector2(horizontal, 0);
        rb.AddForce(vec * Time.deltaTime * speed, ForceMode2D.Impulse);
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            if (rb.velocity.x > maxSpeed) rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            else if (rb.velocity.x < -maxSpeed) rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            if (rb.velocity.x < 0) 
                sr.flipX = true;
            else 
                sr.flipX = false;

            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);

        }
    }

    void Jump()
    {
        if (!isJump || jumpCount > 1) return;

        rb.AddForce(Vector2.up * Time.deltaTime * jumpforce, ForceMode2D.Impulse);
        if (rb.velocity.y > maxJumpforce) rb.velocity = new Vector2(rb.velocity.x, maxJumpforce);
        isJump = false;
        jumpCount++;
        anim.SetBool("jump", true);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = 0;
        isJump = false;
        anim.SetBool("jump", false);
    }
}
