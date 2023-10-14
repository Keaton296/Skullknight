using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;
[RequireComponent(typeof(Rigidbody2D))]
public class CollisionChecker : MonoBehaviour
{
    [SerializeField] LayerMask checkLayerMask;
    //[SerializeField] BoxCollider2D boxCollider;
    //[SerializeField] float extraHeight = .01f;
    public bool inTrigger;
    public Action<Collider2D> onCollisionEnter2D;
    public Action<Collider2D> onTriggerEnter2D;
    private void FixedUpdate()
    {
        //inCollision = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeight, checkLayer);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(checkLayerMask == (checkLayerMask| (1 << other.gameObject.layer)))
        {
            onTriggerEnter2D?.Invoke(other);
            inTrigger = true;

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (checkLayerMask == (checkLayerMask | (1 << other.gameObject.layer)))
        {
            inTrigger = false;

        }
    }

}
