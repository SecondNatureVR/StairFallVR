using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Transform playerHMD;
    [SerializeField] public GameObject ragdollPrefab;
    [SerializeField] public float gravity = 9.81f;
    [SerializeField] public HighscoreScriptableObject highscore;
    [SerializeField] public ResetTimer timer;
    private OVRGrabber[] grabbers;
    private bool playerDisabled = false;
    public float damage;

    private void Start()
    {
        grabbers = GameObject.FindGameObjectsWithTag("Grabbers").Select(g => g.GetComponent<OVRGrabber>()).ToArray();
        Physics.gravity = Vector3.down * gravity;
        damage = 0;
    }

    private void OnGUI()
    {
        Physics.gravity = Vector3.down * gravity;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SpawnRagdoll(InputAction.CallbackContext context)
    {
        if (context.canceled || context.performed)
            return;
        Vector3 position;
        var ray = new Ray(playerHMD.position, playerHMD.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            position = hitInfo.point;
            GameObject.Instantiate(ragdollPrefab, position, Quaternion.identity);
        }
    }

    public void UpdateDamage(float damage)
    {
        this.damage += damage;
        if (playerDisabled != false)
        {
            DisablePlayer();
        }

        highscore.score = Math.Max(this.damage, highscore.score);
    }

    public void StartTimer()
    {
        timer.enabled = true;
    }

    public void DisablePlayer()
    {
        foreach(var grabber in grabbers) 
        {
            grabber.ForceRelease(grabber.grabbedObject);
            grabber.enabled = false;
        }
        playerDisabled = true;
    }
}
