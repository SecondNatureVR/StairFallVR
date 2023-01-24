using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResetTimer : MonoBehaviour
{
    public UnityEvent OnTimerEnd;
    [SerializeField] TMP_Text textMesh;
    private GameManager game;
    private float lastTime;

    public int countdown
    {
        get
        {
            return 10 - Mathf.FloorToInt(Time.time - lastTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
        game = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        OnTimerEnd = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.SetText(Mathf.Max(0, countdown).ToString());
       if (countdown < 0)
        {
            game.ResetScene();
        } 
    }
}
