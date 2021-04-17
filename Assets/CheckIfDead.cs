using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfDead : MonoBehaviour
{
    public Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Ground")
        {
            player.Die();
        }
    }
}
