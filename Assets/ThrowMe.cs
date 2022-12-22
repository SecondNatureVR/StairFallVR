using System;
using System.Linq;
using Cinemachine.Utility;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private UnityEngine.AI.NavMeshAgent _navmeshAgent;
    [SerializeField] private Transform _hipsBone;
    [SerializeField] private String _standUpStateName;

    private ThrowMeState _currentState = ThrowMeState.Idle;
    private Rigidbody[] _ragdollRigidbodies;
    private Animator _animator;
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
        //Walk();
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
        _navmeshAgent.enabled = false;
        Walk();
    }

    private void DisableRagdoll() {
        _animator.enabled = true;
        _navmeshAgent.enabled = true;
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

    private void Walk()
    {
       if (_navmeshAgent.enabled == false)  {
            return;
       }
       if (_navmeshAgent.hasPath == false || _navmeshAgent.remainingDistance < 1f)
        ChooseNewPosition();
    }

    private void ChooseNewPosition() {
        Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        var destination = transform.position + randomOffset;
        _navmeshAgent.SetDestination(destination);
    }

    private void OnTriggerEnter(Collider collider)
    {
            //TriggerRagdoll((collider.transform.position - transform.position).normalized, collider.attachedRigidbody.position);
        if (collider.gameObject.layer != 7 && collider.transform.root != transform ) {
            TriggerRagdoll(Vector3.zero, collider.attachedRigidbody.position);
        }
    }
}
