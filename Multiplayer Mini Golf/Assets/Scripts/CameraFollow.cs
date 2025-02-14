//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{


//    private Vector3 _offset;
//    [SerializeField] private Transform target;
//    [SerializeField] private float smoothTime;
//    private Vector3 _currentVelocity = Vector3.zero;



//    private void Awake() => _offset = transform.position - target.position;

//    private void FixedUpdate()// if camera jitters try late or fixed update
//    {
//        Vector3 targetPosition = target.position + _offset;
//        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);

//    }
//}
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    private Vector3 _originalOffset;
    private Vector3 _currentOffset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float verticalRotationLimit = 35.0f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoomDistance = 0.5f;
    [SerializeField] private float maxZoomDistance = 5.0f;
    private Vector3 _currentVelocity = Vector3.zero;
    private float _currentXRotation = 0f;
    private float _currentYRotation = 0f;
    private float _zoomFactor = 1.0f;

    private void Awake()
    {
        _originalOffset = transform.position - target.position;
        _currentOffset = _originalOffset;
    }

    private void FixedUpdate()
    {
        HandleInput();

        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }
        else
        {
            // Use the current rotated offset instead of the original
            Vector3 targetPosition = target.position + _currentOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
    }

    private void HandleInput()
    {
        // Handle zoom with mouse wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Handle zoom with + and - keys
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
            scrollInput += 0.01f;
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            scrollInput -= 0.01f;

        if (scrollInput != 0)
        {
            // Update zoom factor
            _zoomFactor = Mathf.Clamp(_zoomFactor - scrollInput * zoomSpeed, minZoomDistance / _originalOffset.magnitude, maxZoomDistance / _originalOffset.magnitude);

            // Apply zoom to current offset
            _currentOffset = _zoomFactor * (Quaternion.Euler(_currentXRotation, _currentYRotation, 0) * _originalOffset);
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

        // Update horizontal rotation (left and right)
        _currentYRotation += mouseX;

        // Update vertical rotation (up and down) and clamp it
        _currentXRotation = Mathf.Clamp(_currentXRotation + mouseY, -verticalRotationLimit, verticalRotationLimit);

        // Apply the rotation and zoom to calculate the new offset
        Quaternion rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0);
        _currentOffset = _zoomFactor * (rotation * _originalOffset);

        // Apply the new position
        transform.position = target.position + _currentOffset;
        transform.LookAt(target);
    }
}