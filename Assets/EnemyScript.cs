using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public PlayerMovement player;
    public Animator anim;
    public bool isFalling;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fall()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            if (!isFalling)
            {
                if (player.isTackling)
                {
                    Fall();
                }
                else
                {
                    player.playerscript.Die();
                }
            }
        }
        Vector3 direction = other.transform.position - transform.position;
        Debug.Log(direction);
        if (Vector2.Dot(transform.forward, direction) > 0)
        {
            print("Back");
        }
        if (Vector2.Dot(transform.forward, direction) == 0)
        {
            print("Side");
        }
    }
}
