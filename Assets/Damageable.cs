using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damageable : MonoBehaviour
{
    [SerializeField] GameObject textInfoPrefab;
    
    private GameManager game;

    private void Start()
    {
        game = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var damage = other.impulse.magnitude;
        if (damage > 0 && other.gameObject.CompareTag("Stairs"))
        {
            game.UpdateDamage(damage);
            var text = GameObject.Instantiate(textInfoPrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<TextInfo>();
            text.Init(Mathf.Round(damage).ToString(), Color.red);
        }
    }
}
