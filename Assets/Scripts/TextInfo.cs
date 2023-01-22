using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class TextInfo : MonoBehaviour
{
    [SerializeField] public TMP_Text textMesh;
    [SerializeField] public float fontScale = 3;
    private Transform mainCamera;
    private float duration;
    private float height;
    private Vector3 startPos;
    private float startTime;
    private float scaleReference;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        var c = new ConstraintSource();
        c.weight = 1f;
        c.sourceTransform = mainCamera.transform;
        GetComponent<LookAtConstraint>().AddSource(c);
    }

    public void Init(string text, Color color, float duration = 5.0f, float height = 8.0f)
    {
        startPos = transform.position;
        startTime = Time.time;
        this.duration = duration;
        this.height = height;
        textMesh.text = text;
        textMesh.color = color;

        if (mainCamera == null)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        scaleReference = Vector3.Distance(transform.position, mainCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float scale = Mathf.Max(Vector3.Distance(transform.position, mainCamera.transform.position) / scaleReference, 1) * fontScale;
        transform.localScale = new Vector3(scale, scale, scale);

        float t = Mathf.InverseLerp(startTime, startTime + duration, Time.time);
        transform.position = startPos + Vector3.up * Mathf.Lerp(0, height, t);
        textMesh.alpha = Mathf.Lerp(2, 0, t);

        if (Time.time > startTime + duration)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
