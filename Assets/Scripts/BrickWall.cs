using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : MonoBehaviour
{
    [SerializeField] KeyCode input;
    bool triggered;

    [SerializeField] GameObject triggerIndicator;

    private void Start()
    {
        triggerIndicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
            triggerIndicator.SetActive(true);
        }
    }

    private void Update()
    {
        if(triggered && Input.GetKeyDown(input))
        {
            Destroy(gameObject);
        }
    }
}
