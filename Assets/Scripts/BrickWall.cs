using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : MonoBehaviour
{
    public GameObject destroydebris;

    void DestroyBuilding()
    {
        Instantiate(destroydebris, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (collision.transform.GetComponent<PlayerMovement>().isTackling)
            {
                DestroyBuilding();
            }
            else
            {
                collision.transform.GetComponent<Player>().Die();
            }
            
        }
    }
}
