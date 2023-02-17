using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    private GameManager game;
    private OVRGrabber[] grabbers;
    private BoxCollider playArea;
    private GameObject player;
    private Vector3 spawnPoint;

    private void Start()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        grabbers = GameObject.FindGameObjectsWithTag("Grabbers").Select(g => g.GetComponent<OVRGrabber>()).ToArray();
        playArea = GetComponent<BoxCollider>();
        player = GameObject.FindWithTag("Player");
        spawnPoint = player.transform.position;
    }

    public void LateUpdate()
    {
        foreach(var grabber in grabbers)
        {
            if (!playArea.bounds.Contains(grabber.transform.position) && grabber.grabbedObject != null)
            {
                game.DisablePlayer();
                gameObject.SetActive(false);
            }
        }

        if (!playArea.bounds.Contains(player.transform.position))
        {
            player.transform.position = spawnPoint;
        }
    }
}
