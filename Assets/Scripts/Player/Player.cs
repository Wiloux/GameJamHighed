using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private new Collider2D collider;
    PlayerMovement controller;
    public bool isDead = false;

    public bool isInBuilding;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        controller = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Input.GetMouseButtonDown(0) && !controller.isTackling && controller.IsGrounded())
            {
                controller.isTackling = true;
                animator.SetTrigger("Attack");
            }

            if (transform.position.y < -1 || rb.velocity == Vector2.zero)
            {
                Die();
            }
        }
        else
        {

            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }


    public void StopAttack()
    {
        controller.isTackling = false;
    }

    public void Die()
    {
        // temp
        controller.speed = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        rb.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
        isDead = true;
        controller.anim.SetTrigger("Dead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<FloorTrigger>() != null) { isInBuilding = !isInBuilding; Debug.Log("in building = " + isInBuilding); }
    }
}
