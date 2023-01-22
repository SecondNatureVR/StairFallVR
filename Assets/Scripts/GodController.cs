using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GodController : MonoBehaviour
{
    [SerializeField] public Transform PlayerHMD;
    private Vector2 inputValue;
    public void OnMove(InputValue value)
    {
        inputValue = value.Get<Vector2>();
    }

    public void Update()
    {
        Vector3 move = PlayerHMD.TransformPoint(inputValue.x, 0, inputValue.y);
        transform.Translate(move, Space.World);
    }
}
