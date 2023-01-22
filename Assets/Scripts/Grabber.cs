using Unity.VisualScripting;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _openHand;
    [SerializeField] private GameObject _closedHand;
    private float height = 1.0f;
    private Rigidbody _grabbed = null;

    private void Awake()
    {
        _openHand.SetActive(true);
        _closedHand.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        height += 0.1f * Input.mouseScrollDelta.y;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, height, hitInfo.point.z);
        }

        transform.LookAt(hitInfo.transform);

        if (_grabbed != null)
        {
            if (Vector3.Distance(_grabbed.position, transform.position) > .1f)
            {
                _grabbed.velocity = Vector3.zero;
                _grabbed.MovePosition(transform.position);
            }
        }
    }

    private void OnMouseDown()
    {
        if (_grabbed != null)
        {
            _grabbed = null;
            _openHand.SetActive(true);
            _closedHand.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetMouseButtonDown(0) && _grabbed == null)
        {
            Debug.Log("Grab");
            ThrowMe throwme = other.GetComponentInParent<ThrowMe>();
            if (throwme != null)
            {
                Debug.Log("ThrowMe");
                throwme.GrabRagdoll(this);
                _grabbed = other.attachedRigidbody;
                _openHand.SetActive(false);
                _closedHand.SetActive(true);
            }
        }
    }
}
