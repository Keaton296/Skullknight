using Skullknight.Core;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float velocity;
    public int damage;
    [SerializeField] Rigidbody2D rb;
    public LayerMask hitMask;
    
    public void SetFireballTarget(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        direction.Normalize();
        transform.right = direction;
        rb.velocity = direction * velocity;
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        TryHittingTarget(target);
    }

    private void OnTriggerStay2D(Collider2D target)
    {
        TryHittingTarget(target);
    }

    private void TryHittingTarget(Collider2D target)
    {
        if(hitMask == (hitMask | (1 << target.gameObject.layer)))
        {
            var damageable = target.transform.GetComponent<IDamageable>();
            if (damageable != null && damageable.IsDamageable)
            {  //destroy by entity hit
                if (damageable.TakeDamage(damage))
                {
                    Destroy(gameObject);
                }
            }
            else
            { //destory by env collision
                Destroy(gameObject);
            }
        }
    }
}
