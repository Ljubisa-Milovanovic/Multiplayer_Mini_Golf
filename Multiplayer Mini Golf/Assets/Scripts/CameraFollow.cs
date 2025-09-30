
using Unity.Netcode;
using UnityEngine;
public class CameraFollow : NetworkBehaviour
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


    public override void OnNetworkSpawn()
    {
        _originalOffset = transform.position - target.position;
        _currentOffset = _originalOffset;
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleInput();

        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }
        else
        {
            
            Vector3 targetPosition = target.position + _currentOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
    }

    private void HandleInput()
    {
        
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
            scrollInput += 0.01f;
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            scrollInput -= 0.01f;

        if (scrollInput != 0)
        {
            
            _zoomFactor = Mathf.Clamp(_zoomFactor - scrollInput * zoomSpeed, minZoomDistance / _originalOffset.magnitude, maxZoomDistance / _originalOffset.magnitude);

            
            _currentOffset = _zoomFactor * (Quaternion.Euler(_currentXRotation, _currentYRotation, 0) * _originalOffset);
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

        
        _currentYRotation += mouseX;

        
        _currentXRotation = Mathf.Clamp(_currentXRotation + mouseY, -verticalRotationLimit, verticalRotationLimit);

        
        Quaternion rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0);
        _currentOffset = _zoomFactor * (rotation * _originalOffset);

        
        transform.position = target.position + _currentOffset;
        transform.LookAt(target);
    }
}