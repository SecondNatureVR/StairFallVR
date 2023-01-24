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
        game.OnDamageReceived += HandleDamage;
    }

    public void HandleDamage(float damage)
    {
        if (game.TotalDamage > 0)
            textMesh.SetText($"Damage: {Mathf.CeilToInt(game.TotalDamage)}");
        else
            textMesh.SetText("Throw Me!");
    }

    [ExecuteInEditMode]
    void Update()
    {
        if (Vector3.Distance(transform.position, anchor.position) > offset * 1.005f)
            transform.position = anchor.position + Vector3.up * offset;

        float scale = Mathf.Max(Vector3.Distance(transform.position, mainCamera.transform.position) / scaleReference, 1) * 1.25f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
