using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : MonoBehaviour
{

    
    [SerializeField] GameObject triggerIndicator;
    public GameObject destroydebris;

    private void Start()
    {
        triggerIndicator.SetActive(false);
    }


    void DestroyBuilding()
    {
        Instantiate(destroydebris, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Debug.Log("w");
            if (collision.transform.GetComponent<PlayerMovement>().isTackling)
            {
                Debug.Log("t");
                DestroyBuilding();
            }
            else
            {
                Debug.Log("f");
                collision.transform.GetComponent<Player>().Die();
            }
            
        }
    }
}
