using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ThrowMe : MonoBehaviour, IGrabbableMessageTarget
{
    public UnityEvent OnGrabbedEvent;
    public enum ThrowMeState
    {
        Idle,
        Dead,
        GetUp
    }

    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private Transform _hipsBone;
    [SerializeField] private String _standUpStateName;

    private GameManager game;
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
        // TODO: Decouple
        game = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
            // TODO: Decouple
            game.ResetScene();

            //AlignPositionToHips();
            //_currentState = ThrowMeState.GetUp;
            //DisableRagdoll();

            //_animator.Play(_standUpStateName);
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
        // TODO: Decouple
        game.StartTimer();
        EnableRagdoll();
        foreach (var rb in _ragdollRigidbodies)
        {
            rb.velocity = Vector3.zero;
        }
        _currentState = ThrowMeState.Dead;
        OnGrabbedEvent.Invoke();
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
        if (collider.gameObject.layer != 7 && collider.transform.root != transform ) {
            var grabber = collider.GetComponent<OVRTouchSample.Hand>();
            if (grabber && grabber.isGrabbing)
                TriggerRagdoll(Vector3.zero, collider.attachedRigidbody.position);
        }
    }
}
