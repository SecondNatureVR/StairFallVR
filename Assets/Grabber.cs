using Unity.VisualScripting;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private float height = 1.0f;

    // Update is called once per frame
    void Update()
    {
        height += 0.5f * Input.mouseScrollDelta.y;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, height, hitInfo.point.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowMe throwme = other.GetComponentInParent<ThrowMe>();
            if (throwme != null && !throwme.isGrabbed())
            {
                throwme.GrabRagdoll(this);
            }
        }
    }
}
