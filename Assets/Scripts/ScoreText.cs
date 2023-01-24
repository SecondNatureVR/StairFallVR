using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] public TMP_Text textMesh;
    [SerializeField] public SavedScore score;
    [SerializeField] public string title;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        textMesh.SetText($"{title}: {Mathf.CeilToInt(score.value)}");
    }
}
