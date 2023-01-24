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
    [SerializeField] public SavedScore highscore;
    [SerializeField] public SavedScore lastScore;
    [SerializeField] public ResetTimer timer;
    private OVRGrabber[] grabbers;
    private bool playerDisabled = false;
    private float _totalDamage = 0;
    private float _lastBufferDamage = 0;
    private float _lastBufferTime = 0;
    private float _bufferWindow = 0.3f;

    public float TotalDamage {  get { return _totalDamage;  } }

    public Action<float> OnDamageReceived;

    private void Start()
    {
        grabbers = GameObject.FindGameObjectsWithTag("Grabbers").Select(g => g.GetComponent<OVRGrabber>()).ToArray();
        Physics.gravity = Vector3.down * gravity;
        foreach (var d in FindObjectsOfType<Damageable>())
        {
            d.OnDamage += HandleDamage;
        }
    }

    private void OnGUI()
    {
        Physics.gravity = Vector3.down * gravity;
    }

    public void ResetScene()
    {
        lastScore.value = TotalDamage;
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

    public void HandleDamage(float damage)
    {
        _totalDamage += damage;
        if (playerDisabled != false)
        {
            DisablePlayer();
        }

        highscore.value = Math.Max(_totalDamage, highscore.value);
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

    public void LateUpdate()
    {
        if (_totalDamage > _lastBufferDamage && Time.time - _lastBufferTime > _bufferWindow)
        {
            _lastBufferTime = Time.time;
            OnDamageReceived.Invoke(_totalDamage - _lastBufferDamage);
            _lastBufferDamage = _totalDamage;
        }
    }
}
