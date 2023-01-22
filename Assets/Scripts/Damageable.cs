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

    private AudioSource audioSource;
    private AudioClip[] clips;
    private Rigidbody rb;
    
    private GameManager game;

    private void Start()
    {
        clips = Resources.LoadAll("Assets/SFX", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        rb = GetComponent<Rigidbody>();
        game = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var damage = other.impulse.magnitude;
        if (damage > 5 && other.gameObject.CompareTag("Stairs"))
        {
            PlaySFX();
            SpawnText(Mathf.Round(damage).ToString());
            SpawnSplatter(other.GetContact(0), damage);
            game.UpdateDamage(damage);
        }
    }

    private void SpawnText(string text)
    {
        var textInfo = GameObject.Instantiate(textInfoPrefab, transform.position + Vector3.up * 2, Quaternion.identity).GetComponent<TextInfo>();
        textInfo.Init(text, Color.red);
    }

    private void SpawnSplatter(ContactPoint c, float damage)
    {
        var splatter = GameObject.Instantiate(splatterPrefab, c.point, Quaternion.LookRotation(Vector3.down, rb.velocity)).GetComponentInChildren<DecalProjector>();
        float splatterScale = Mathf.Sqrt(damage/50f);
        splatter.size = new Vector3(splatterScale, splatterScale, splatter.size.z);
    }

    private void PlaySFX()
    {
        if (clips.Length > 0)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length - 1)];
            audioSource.Play();
        }
    }
}
