using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Damageable : MonoBehaviour
{
    [SerializeField] GameObject textInfoPrefab;
    [SerializeField] GameObject splatterPrefab;

    public event Action<float> OnDamage;

    private Rigidbody rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var damage = other.impulse.magnitude;
        if (damage > 5 && other.gameObject.CompareTag("Stairs"))
        {
            SpawnSplatter(other.GetContact(0), damage);
            OnDamage?.Invoke(damage);
        }
    }

    private void SpawnSplatter(ContactPoint c, float damage)
    {
        var splatter = GameObject.Instantiate(splatterPrefab, c.point, Quaternion.LookRotation(Vector3.down, rb.velocity)).GetComponentInChildren<DecalProjector>();
        float splatterScale = Mathf.Sqrt(damage/50f);
        splatter.size = new Vector3(splatterScale, splatterScale, splatter.size.z);
    }
}
