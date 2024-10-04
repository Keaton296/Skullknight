using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HangPoint : MonoBehaviour
{
    [SerializeField] private Transform m_holdPoint;
    [SerializeField] private Transform m_climbPoint;
    [SerializeField] private BoxCollider2D m_collider;
    public Collider2D HandCollider => m_collider;
    public Transform HoldPoint => m_holdPoint;
    public Transform ClimbPoint => m_climbPoint;
}
