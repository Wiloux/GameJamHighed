using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfDead : MonoBehaviour
{
    public Player player;
    [SerializeField] float range;

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ground")) { player.Die(); break; }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
