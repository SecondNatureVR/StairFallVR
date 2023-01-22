using System;
using System.Linq;
using Cinemachine.Utility;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class ThrowMe : MonoBehaviour, IGrabbableMessageTarget
{
    private enum ThrowMeState
    {
        Idle,
        Dead,
        GetUp
    }

    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private Transform _hipsBone;
    [SerializeField] private String _standUpStateName;

    private ThrowMeState _currentState = ThrowMeState.Idle;
    private Rigidbody[] _ragdollRigidbodies;
    private Animator _animator;

    public void OnGrabBegin()
    {
        Debug.Log("GrabBegin Detected");
        EnableRagdoll();
    }
    public void OnGrabEnd()
    {
        Debug.Log("GrabEnd Detected");
        DeadBehavior();
    }
    private void Awake()
    {
        _ragdollRigidbodies = _ragdoll.GetComponentsInChildren<Rigidbody>();
        _animator = _ragdoll.GetComponent<Animator>();
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
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_standUpStateName)) {
            _currentState = ThrowMeState.Idle;
        }
    }

    private void DeadBehavior()
    {
        if (isStable())
        {
            AlignPositionToHips();
            _currentState = ThrowMeState.GetUp;
            DisableRagdoll();

            _animator.Play(_standUpStateName);
        }
    }

    private void IdleBehavior()
    {
        // Walk();
    }

    private bool isStable()
    {
        foreach (var rb in _ragdollRigidbodies)
        {
            if (!rb.IsSleeping())
            {
                return false;
            }
        }
        return true;
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();
        Rigidbody hitRigidbody = _ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        _currentState = ThrowMeState.Dead;
    }

    public void GrabRagdoll(Grabber grabber) {
        EnableRagdoll();
        foreach (var rb in _ragdollRigidbodies)
        {
            rb.velocity = Vector3.zero;
        }
        _currentState = ThrowMeState.Dead;
    }

    private void EnableRagdoll() {
        _animator.enabled = false;
    }

    private void DisableRagdoll() {
        _animator.enabled = true;
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        transform.position = _hipsBone.position;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        _hipsBone.position = originalHipsPosition;
    }

    private void OnTriggerEnter(Collider collider)
    {
            //TriggerRagdoll((collider.transform.position - transform.position).normalized, collider.attachedRigidbody.position);
        if (collider.gameObject.layer != 7 && collider.transform.root != transform ) {
            TriggerRagdoll(Vector3.zero, collider.attachedRigidbody.position);
        }
    }
}
