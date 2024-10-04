using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static UnityEditor.Experimental.GraphView.GraphView;
[RequireComponent(typeof(Rigidbody2D))]
public class CollisionChecker : MonoBehaviour
{
    [SerializeField] LayerMask checkLayerMask;
    //[SerializeField] BoxCollider2D boxCollider;
    //[SerializeField] float extraHeight = .01f;
    [FormerlySerializedAs("inTrigger")] public bool IsColliding;
    public Action<Collider2D> onCollisionEnter2D;
    public Action<Collider2D> onTriggerEnter2D;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(checkLayerMask == (checkLayerMask| (1 << other.gameObject.layer)))
        {
            onTriggerEnter2D?.Invoke(other);
            IsColliding = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (checkLayerMask == (checkLayerMask | (1 << other.gameObject.layer)))
        {
            IsColliding = false;

        }
    }

}
