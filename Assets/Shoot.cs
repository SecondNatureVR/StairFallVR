using UnityEngine;


public class Shoot : MonoBehaviour
{
    [SerializeField] private float _maxForce;
    [SerializeField] private float _maxForceTime;
    private float _timeMouseButtonDown;

    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _timeMouseButtonDown = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                ThrowMe throwme = hitInfo.collider.GetComponentInParent<ThrowMe>();

                if (throwme != null)
                {
                    float mouseButtonDownDuration = Time.time - _timeMouseButtonDown;
                    float forcePercentage = mouseButtonDownDuration / _maxForce;
                    float forceMagnitude = Mathf.Lerp(1, _maxForce, forcePercentage);

                    Vector3 forceDirection = throwme.transform.position - _camera.transform.position;
                    forceDirection.y = 1;
                    forceDirection.Normalize();

                    Vector3 force = forceMagnitude * forceDirection;

                    throwme.TriggerRagdoll(force, hitInfo.point);
                }
            }
        }
    }
}
