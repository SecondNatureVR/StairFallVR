using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Transform playerHMD;
    [SerializeField] public GameObject ragdollPrefab;
    public float damage;

    private void Start()
    {
        damage = 0;
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
    }
}
