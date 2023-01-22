using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class DamageUI : MonoBehaviour
{
    [SerializeField] public TMP_Text textMesh;
    [SerializeField] public Transform anchor;
    [SerializeField] public float offset = 3f;
    [SerializeField] public Camera mainCamera;
    private float scaleReference;
    private GameManager game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        scaleReference = Vector3.Distance(transform.position, mainCamera.transform.position);
    }

    [ExecuteInEditMode]
    void Update()
    {
        if (Vector3.Distance(transform.position, anchor.position) > offset * 1.05f)
            transform.position = anchor.position + Vector3.up * offset;
        textMesh.SetText($"Damage: {Mathf.CeilToInt(game.damage)}");

        float scale = Mathf.Max(Vector3.Distance(transform.position, mainCamera.transform.position) / scaleReference, 1);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
