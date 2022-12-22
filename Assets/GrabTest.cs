using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTest : MonoBehaviour
{
    [SerializeField] private Grabber _grabber;
    [SerializeField] private float gravity;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
       _rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_rb.position, _grabber.transform.position) > .1f)
        {
            _rb.velocity = Vector3.zero;
            _rb.MovePosition(_grabber.transform.position);
        }
    }
} 