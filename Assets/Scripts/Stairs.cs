using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] public float minForce = 2.1f;
    [SerializeField] public float maxForce = 4f;
    private void OnCollisionEnter(Collision collision)
    {
        var rb = collision.rigidbody;
        float downV = Mathf.Min(Mathf.Abs(rb.velocity.y), maxForce);
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(minForce, downV), rb.velocity.z);
    }
}
