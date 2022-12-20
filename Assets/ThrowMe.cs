using System;
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
    [SerializeField] private GameObject _animatedModel;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _navmeshAgent;
    [SerializeField] private Transform _hipsBone;
    [SerializeField] private String _standUpStateName;
    [SerializeField] private Animator _animator;

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
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_standUpStateName)) {
            _currentState = ThrowMeState.Idle;
        }
    }

    private void DeadBehavior()
    {
        if (Input.GetButton("Fire2"))
        {
            AlignPositionToHips();
            _currentState = ThrowMeState.GetUp;
            DisableRagdoll();

            _animator.Play(_standUpStateName);
        }
    }

    private void IdleBehavior()
    {
        Walk();
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
            _navmeshAgent.isStopped = true;
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
}
