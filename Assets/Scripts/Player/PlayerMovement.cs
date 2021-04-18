using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector2 speedMinMax;
    [SerializeField] float speedGain;
    public float speed;
    public float jumpForce;
    public bool isTackling;
    public float diveForce;

    private Rigidbody2D rb;
    public Animator anim;
    private new Collider2D collider;
    public Player playerscript;

    public List<AudioClip> JumpEffects = new List<AudioClip>();
    public AudioClip diveEffect;

    private const float JUMP_PRESS_REMEMBER_DURATION = 0.3f;
    private float jumpPressedRememberTimer;

    private const float GROUND_REMEMBER_DURATION = 0.3f;
    private float groundedRememberTimer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        playerscript = GetComponent<Player>();

        speed = speedMinMax.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerscript.isDead)
        {
            anim.speed = (speed / speedMinMax.y) *2;
            if (speed < speedMinMax.y) speed += speedGain * Time.deltaTime;
            if (speed > speedMinMax.y) speed = speedMinMax.y;

            if (anim != null) anim.SetFloat("VelocityY", rb.velocity.y);

         //   if (Input.GetKeyDown(KeyCode.C)) { SoundManager.Instance.PlaySoundEffect(diveEffect); }
            if (Input.GetKey(KeyCode.C) && !IsGrounded() && !isTackling) { rb.gravityScale = diveForce; anim.SetBool("Diving", true); } else { rb.gravityScale = 4; anim.SetBool("Diving", false); }

            Run();

            bool isGrounded = IsGrounded();
            if (IsGrounded()) groundedRememberTimer = GROUND_REMEMBER_DURATION;

            if (Input.GetKeyDown(KeyCode.Space) && groundedRememberTimer > 0)
            {
                jumpPressedRememberTimer = JUMP_PRESS_REMEMBER_DURATION;
                SoundManager.Instance.PlaySoundEffect(JumpEffects[Random.Range(0, JumpEffects.Count)]);
            }

            if (groundedRememberTimer > 0 && jumpPressedRememberTimer > 0) Jump();


            jumpPressedRememberTimer -= Time.deltaTime;
            groundedRememberTimer -= Time.deltaTime;
        }

        if (playerscript.isDead)
        {
            rb.gravityScale = 4;
        }
        if(IsGrounded() && playerscript.isDead)
        {
            rb.drag = 5;
        }
        if (anim != null) anim.SetBool("Airborn", !IsGrounded());
    }

    private void Run()
    {
        Vector2 v = rb.velocity;
        v.x = speed;
        rb.velocity = v;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;
        jumpPressedRememberTimer = 0;
        groundedRememberTimer = 0;
    }


    public bool IsGrounded()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(collider.bounds.center.x, collider.bounds.min.y), Vector2.down, 1f);
        Debug.DrawRay(new Vector2(collider.bounds.center.x, collider.bounds.min.y), Vector2.down, Color.red, 0.1f);
        foreach(RaycastHit2D hit in hits)
        {
            if (!hit.collider.isTrigger)
            {
                if (hit.transform == transform) continue;
                return hit.collider.CompareTag("Ground");
            }
        }
        return false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Ground" && rb.velocity.y <= 0)
    //    {
    //        airBorn = false;
    //    }
    //}
}
