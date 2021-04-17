using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public PlayerMovement player;
    public Animator anim;
    private Rigidbody2D rb;
    public bool isFalling;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void Fall()
    {
        anim.SetTrigger("Fall");
        isFalling = true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D contact = other.GetContact(0);
        if (other.transform.tag == "Player")
        {
            if (!isFalling)
            {
                if (player.isTackling)
                {
                    rb.AddForce(new Vector2(Random.Range(50,65),40), ForceMode2D.Impulse);
                    Fall();
                }
                else
                {
                    player.playerscript.Die();
                }
            }
        }

        if (other.transform.tag == "Ground")
        {
            if (isFalling)
            {
                Vector2 direction = contact.point - new Vector2(transform.position.x, transform.position.y);
                if (direction.x > 0)
                {
                    anim.SetTrigger("Wall");
                    rb.AddForce(new Vector2(-10, 5), ForceMode2D.Impulse);
                } else if (direction.y < 0)
                {
                   anim.SetTrigger("Ground");
                }
            }
        }
    }
}
