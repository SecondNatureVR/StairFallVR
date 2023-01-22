using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    private GameManager game;
    private OVRGrabber[] grabbers;
    private BoxCollider playArea;

    private void Start()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        grabbers = GameObject.FindWithTag("Player").GetComponentsInChildren<OVRGrabber>();
        playArea = GetComponent<BoxCollider>();
    }

    public void Update()
    {
        foreach(var grabber in grabbers)
        {
            if (!playArea.bounds.Contains(grabber.transform.position) && grabber.grabbedObject != null)
            {
                game.DisablePlayer();
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
