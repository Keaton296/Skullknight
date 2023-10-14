using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Fireball : MonoBehaviour
{
    public float velocity;
    [SerializeField] Rigidbody2D rb;
    public LayerMask hitMask;
    
    public void SetFireball(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        direction.Normalize();
        transform.right = direction;
        rb.velocity = direction * velocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hitMask == (hitMask | (1 << collision.gameObject.layer))) Destroy(gameObject);
    }
}
