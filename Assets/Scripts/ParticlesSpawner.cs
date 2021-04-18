using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ParticlesSpawner : MonoBehaviour
{
    [SerializeField] GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Instantiate(effect, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        Destroy(gameObject);
    }
}
