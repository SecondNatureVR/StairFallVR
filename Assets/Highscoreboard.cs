using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscoreboard : MonoBehaviour
{
    [SerializeField] public TMP_Text textMesh;
    [SerializeField] public HighscoreScriptableObject highscore;

    public void Update()
    {
        textMesh.SetText($"Highscore: {Mathf.CeilToInt(highscore.score)}");
    }
}
