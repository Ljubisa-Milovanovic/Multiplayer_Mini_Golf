using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;


public class Udarac : NetworkBehaviour
{
    private Camera _camera;
    //NetworkVariable<int> Stroke = new NetworkVariable<int>(0);



    public PhysicMaterial ballMaterial;
    public int Strokes = 0;



    public Material[] materials;
    private int materialCounter = 0;
    private int stopStopper = 0;
    Renderer rend;

    [SerializeField] private float shotPower;
    [SerializeField] private float MaxPower=0.6f;//0.5-0.8
    [SerializeField] private float stopVelocity = 2f; //The velocity below which the rigidbody will be considered as stopped
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
        _rigidbody = GetComponent<Rigidbody>();

        isAiming = false;
        lineRenderer.enabled = false;

        lastYposition = transform.position.y;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];

        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        Transform ballParent = transform.parent;
        // Find the Camera component in the player's children
        _camera = ballParent.GetComponentInChildren<Camera>();

        if (_camera == null)
        {
            Debug.LogError("No camera found under Player GameObject!");
        }



        ballMaterial.bounciness = 1f; // Default to no bounce
        ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;//average

        // Assign the material to the ball's collider
        GetComponent<Collider>().material = ballMaterial;

        if (!IsOwner)
        {
            _camera.enabled = false;
        }


        //set stroke text to strokeCounter tag

        strokesText = GameObject.FindWithTag("strokeCounter").GetComponent<TextMeshProUGUI>();

    }



    private void LateUpdate()
    {
        Vector3 velocity = _rigidbody.velocity;
        if(velocity.y>0)
            velocity.y = 0; // Eliminate vertical velocity
        _rigidbody.velocity = velocity;
    }
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;

        }
        //Debug.Log("<color=yellow>fixedupdate function called!</color> Velocity before zeroing: " + _rigidbody.velocity.magnitude +" y: "+_rigidbody.velocity.y + ", Angular Velocity: " + _rigidbody.angularVelocity.magnitude); // Debug log when Stop is called
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
            
            //Debug.Log("mf is idle and ready to explode.......PAUSE");
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
        //Debug.Log("<color=red>kraj fixedupdate function called!</color> Velocity before zeroing: " + _rigidbody.velocity.magnitude + ", Angular Velocity: " + _rigidbody.angularVelocity.magnitude); // Debug log when Stop is called

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
        //Debug.Log("<color=yellow>processAin() function called!</color> Velocity before zeroing: " + _rigidbody.velocity.magnitude + ", Angular Velocity: " + _rigidbody.angularVelocity.magnitude); // Debug log when Stop is called

        if (!isAiming || !isIdle)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        /*
        //just a test---------------------------

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.Value.x, transform.position.y, worldPoint.Value.z);
        float distance = Vector3.Distance(transform.position, horizontalWorldPoint);
        float strength = Mathf.Clamp(distance, 0f, MaxPower) / MaxPower;
        Debug.Log(" Snaga: " + strength.ToString());
        //just a test---------------------------
        */

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
        //Debug.Log("<color=yellow>aim processovan function called!</color> Velocity before zeroing: " + _rigidbody.velocity.magnitude + ", Angular Velocity: " + _rigidbody.angularVelocity.magnitude); // Debug log when Stop is called
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


        // Clamp strength to MaxPower
        strength = Mathf.Clamp(strength, 0f, MaxPower);

        // --- Non-Linear Force Scaling ---
        // Normalize strength to a 0-1 range based on MaxPower
        float normalizedStrength = strength / MaxPower;

        // Apply a power function or other non-linear curve to normalizedStrength
        //float forceMultiplier = Mathf.Pow(normalizedStrength, forceExponent)*7; // Example: Square function
        //float forceMultiplier = normalizedStrength;
        float forceMultiplier = (normalizedStrength * normalizedStrength) + (forceExponent * normalizedStrength);
        // Alternatively, you could use Mathf.Sqrt(normalizedStrength) for a different curve
        // Or even an exponential function Mathf.Exp(normalizedStrength) - 1; (adjust as needed)

        //Debug.Log("strenght: " + strength.ToString() + " normalized strength: " + normalizedStrength.ToString() + " forceMultiplayer: " + forceMultiplier.ToString());
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        // Apply the scaled force
        Vector3 force = -direction * forceMultiplier * shotPower;
        force.y = 0;
        _rigidbody.AddForce(force);
        Debug.Log("<color=green>Froce:</color> " + force + ", force y: " + force.y); // Debug log when Stop is called
        Strokes++;
        strokesText.text = "Strokes: " + Strokes.ToString();
        isIdle = false;
        //Debug.Log("i shot him");

    }

    private void DrawLine(Vector3 worldPoint)
    {
        //Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        float distance = Vector3.Distance(transform.position, worldPoint);

        Vector3 targetWorldPoint = worldPoint; // Assume original worldPoint by default

        if (distance > MaxPower)
        {
            // Calculate the direction vector from transform.position to horizontalWorldPoint
            Vector3 direction = (worldPoint - transform.position).normalized;

            // Multiply the normalized direction by 0.6f to get a vector of length 0.6f
            Vector3 limitedDirection = direction * MaxPower;

            // Calculate the limited world point by adding the limited direction to transform.position
            Vector3 limitedWorldPoint = transform.position + limitedDirection;

            // Set the y component of the limitedWorldPoint to be the same as the original worldPoint to maintain vertical position
            limitedWorldPoint.y = worldPoint.y; // or transform.position.y if you want line always in horizontal plane of origin

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
            //Debug.Log("<color=green>Stop() function called!</color> Velocity before zeroing: " + _rigidbody.velocity.magnitude + ", Angular Velocity: " + _rigidbody.angularVelocity.magnitude); // Debug log when Stop is called

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            // Make the Rigidbody kinematic to prevent further physics interactions
            //
            //rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            //_rigidbody.isKinematic = true;
            isIdle = true;
            //ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            //Debug.Log("<color=blue>Velocity after zeroing:</color> " + _rigidbody.velocity.magnitude + ", Angular Velocity: " + _rigidbody.angularVelocity.magnitude); // Verify velocity is zeroed
            //Debug.Log("uso sam u if stopa");
        }
        //rigidbody.velocity = Vector3.zero;
        //rigidbody.angularVelocity = Vector3.zero;
        //isIdle = true;
        //Debug.Log("mf is stoped");
        //_rigidbody.isKinematic = false;
    }

    private Vector3? CastMouseClickRay() //invisible floor required on every level for it to work
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
            Debug.Log("hit is: " + hit.point.ToString());
            return hit.point;
        }
        else
        {
            return null;
        }
    }
    
}
