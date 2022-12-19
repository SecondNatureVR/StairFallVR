using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowMe : MonoBehaviour
{
    private enum ThrowMeState
    {
        Idle,
        Dead,
        GetUp
    }

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private GameObject _animatedModel;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _navmeshAgent;

    private ThrowMeState _currentState = ThrowMeState.Idle;
    private void Awake()
    {
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case ThrowMeState.Idle:
                IdleBehavior();
                break;
            case ThrowMeState.Dead:
                DeadBehavior();
                break;
            case ThrowMeState.GetUp:
                GetUpBehavior();
                break;
        }
        
    }

    private void GetUpBehavior()
    {
        DisableRagdoll();
        _currentState = ThrowMeState.Idle;
    }

    private void DeadBehavior()
    {
        if (Input.GetButton("Fire2"))
        {
            _currentState = ThrowMeState.GetUp;
        }
    }

    private void IdleBehavior()
    {
        if (Input.GetButton("Fire1"))
        {
            EnableRagdoll();
            _currentState = ThrowMeState.Dead;
        }
    }

    private void EnableRagdoll() {
        CopyTransformData(_animatedModel.transform, _ragdoll.transform, _navmeshAgent.velocity);
        _ragdoll.gameObject.SetActive(true);
        _animatedModel.gameObject.SetActive(false);
        _navmeshAgent.enabled = false;
    }

    private void DisableRagdoll() { 
        _ragdoll.gameObject.SetActive(false);
        _animatedModel.gameObject.SetActive(true);
        _navmeshAgent.enabled = true;
    }

    private void CopyTransformData(Transform sourceTransform, Transform destinationTransform, Vector3 velocity) {
        if (sourceTransform.childCount != destinationTransform.childCount) {
            Debug.LogWarning("Invalid transform copy, they need to match transform hierarchies");
            return;
        }

        for (int i = 0; i < sourceTransform.childCount; i++) {
            var source = sourceTransform.GetChild(i);
            var destination = destinationTransform.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;
            var rb = destination.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.velocity = velocity;
            }

            CopyTransformData(source, destination, velocity);
        }
    }
}
