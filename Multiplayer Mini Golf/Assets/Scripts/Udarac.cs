using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;


public class Udarac : NetworkBehaviour
{
    private string PlayerName;
    bool SendNameFlag = true;
    public static Udarac Instance { get; private set; }
    private Camera _camera;
    private Timer _timer;

    public PhysicMaterial ballMaterial;
    public int Strokes = 0;

    public Material[] materials;
    private int materialCounter = 0;
    private int stopStopper = 0;
    Renderer rend;

    [SerializeField] private float shotPower;
    [SerializeField] private float MaxPower=0.6f;
    [SerializeField] private float stopVelocity = 2f; 
    [SerializeField] private float forceExponent = 0.5f;

    [SerializeField] private LineRenderer lineRenderer;

    public bool isIdle;
    private bool isAiming;

    private float lastYposition;
    private bool isGrounded;
    private const float groundedThreshold = 0.001f;

    private Rigidbody _rigidbody;

    public TextMeshProUGUI strokesText;


    public override void OnNetworkSpawn()
    {
        _timer = FindObjectOfType<Timer>();

        if (_timer != null)
        {
            _timer.StartFlag = true;
        }
        else
        {
            Debug.LogError("Timer component not found in the scene.");
        }
        _rigidbody = GetComponent<Rigidbody>();
        

        isAiming = false;
        lineRenderer.enabled = false;

        lastYposition = transform.position.y;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];

        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        Transform ballParent = transform.parent;
        
        _camera = ballParent.GetComponentInChildren<Camera>();

        if (_camera == null)
        {
            Debug.LogError("No camera found under Player GameObject!");
        }

        ballMaterial.bounciness = 1f; 
        ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;

        
        GetComponent<Collider>().material = ballMaterial;

        if (!IsOwner)
        {
            _camera.enabled = false;
        }

        

        strokesText = GameObject.FindWithTag("strokeCounter").GetComponent<TextMeshProUGUI>();

        
    }



    private void LateUpdate()
    {
        
        
        if (SendNameFlag)
        {
            if (IsOwner)
            {
                string myName = EditPlayerName.Instance.GetPlayerName();
                

                NameManager.instance.AddPlayerToListServerRpc(OwnerClientId,myName);
                Debug.Log("poslo sam info : " + myName + OwnerClientId);
            }
            SendNameFlag = false;
        }
    }
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;

        }
        
        if (stopStopper<20)
            stopStopper++;
        isGrounded = Mathf.Abs(lastYposition - transform.position.y) < groundedThreshold;
        lastYposition = transform.position.y;
        if (_rigidbody.velocity.magnitude < stopVelocity && isGrounded)
        {
            if(stopStopper>10)
                Stop();   
        }
        if (isIdle && isGrounded)
        {
            
            
            if (materialCounter < 15)
            {
                rend.sharedMaterial = materials[1];
            }
            if (materialCounter >= 15)
            {
                rend.sharedMaterial = materials[2];
            }
            if(materialCounter > 30)
            {
                materialCounter = 0;
            }
            materialCounter++;
        }
        else
        {
            rend.sharedMaterial = materials[0];
            materialCounter = 0;
        }
        ProcessAim();
        

    }

    private void OnMouseDown()
    {
        if (isIdle)
        {
            isAiming = true;
        }
    }

    private void ProcessAim()
    {
        

        if (!isAiming || !isIdle)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        

        if (!worldPoint.HasValue)
        {
            return;
        }

        DrawLine(worldPoint.Value);
        materialCounter = 0;
        rend.sharedMaterial = materials[0];

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(worldPoint.Value);
        }
        
    }

    private void Shoot(Vector3 worldPoint)
    {
        GameMenager.instance.lastLocation = transform.position;
        _rigidbody.constraints = RigidbodyConstraints.None;

        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        Debug.Log("razlika:" +horizontalWorldPoint.y +" transform " +transform.position.y + " direction" + direction);
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);


        
        strength = Mathf.Clamp(strength, 0f, MaxPower);

        
        float normalizedStrength = strength / MaxPower;

        
        float forceMultiplier = (normalizedStrength * normalizedStrength) + (forceExponent * normalizedStrength);
        

        
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        
        Vector3 force = -direction * forceMultiplier * shotPower;
        force.y = 0;
        _rigidbody.AddForce(force);
        Debug.Log("<color=green>Froce:</color> " + force + ", force y: " + force.y); 
        Strokes++;
        strokesText.text = Strokes.ToString();
        isIdle = false;
        

        this.GetComponent<AudioSource>().Play();

        
        NameManager.instance.UpdatePlayerTotalScoreServerRpc(OwnerClientId, 1);
    }

    private void DrawLine(Vector3 worldPoint)
    {
        
        float distance = Vector3.Distance(transform.position, worldPoint);
        Vector3 targetWorldPoint = worldPoint; 

        if (distance > MaxPower)
        {
            Vector3 direction = (worldPoint - transform.position).normalized;
            Vector3 limitedDirection = direction * MaxPower;
            Vector3 limitedWorldPoint = transform.position + limitedDirection;
            limitedWorldPoint.y = worldPoint.y; 
            targetWorldPoint = limitedWorldPoint;
        }


        Vector3[] positions = {
        transform.position,
        targetWorldPoint};
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
        
    }


    private void Stop()
    {
        if (Mathf.Abs(_rigidbody.velocity.magnitude) < stopVelocity && Mathf.Abs(_rigidbody.angularVelocity.magnitude) < stopVelocity)
        {
            

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            
            isIdle = true;
            
        }

    }

    private Vector3? CastMouseClickRay() 
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity))
        {
            
            return hit.point;
        }
        else
        {
            return null;
        }
    }


}
