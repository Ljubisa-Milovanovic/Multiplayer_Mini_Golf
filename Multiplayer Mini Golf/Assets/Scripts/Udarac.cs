using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udarac : MonoBehaviour
{
    public PhysicMaterial ballMaterial;
    public int Strokes = 0;

    public Material[] materials;
    private int materialCounter = 0;
    private int stopStopper = 0;
    Renderer rend;

    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = .05f; //The velocity below which the rigidbody will be considered as stopped

    [SerializeField] private LineRenderer lineRenderer;

    public bool isIdle;
    private bool isAiming;

    private float lastYposition;
    private bool isGrounded;
    private const float groundedThreshold = 0.0001f;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        isAiming = false;
        lineRenderer.enabled = false;
    }

    void Start()
    {
        lastYposition = transform.position.y;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];

        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;


        
        ballMaterial.bounciness = 1f; // Default to no bounce
        ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;//average

        // Assign the material to the ball's collider
        GetComponent<Collider>().material = ballMaterial;

    }


    private void FixedUpdate()
    {
        if(stopStopper<20)
            stopStopper++;
        isGrounded = Mathf.Abs(lastYposition - transform.position.y) < groundedThreshold;
        lastYposition = transform.position.y;
        if (rigidbody.velocity.magnitude < stopVelocity && isGrounded)
        {
            if(stopStopper>10)
                Stop();   
        }
        if (isIdle && isGrounded)
        {
            
                //Vector3 velocity = rigidbody.velocity;
                //velocity.y = 0; // Eliminate vertical velocity
                //rigidbody.velocity = velocity;
            
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
        rigidbody.constraints = RigidbodyConstraints.None;

        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        Strokes++;
        rigidbody.AddForce(-direction * strength * shotPower); //ne dodat forcemode.impulse
        isIdle = false;
        Debug.Log("i shot him");
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = {
        transform.position,
        worldPoint};
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        if (rigidbody.velocity.magnitude < stopVelocity && Mathf.Abs(rigidbody.angularVelocity.magnitude) < stopVelocity)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            // Optional: Manually set the position to correct minor bounces
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x, Mathf.Round(position.y * 1000f) / 1000f, position.z);
            // Make the Rigidbody kinematic to prevent further physics interactions
            //
            //rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            //rigidbody.isKinematic = true;
            isIdle = true;
            //ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            //Debug.Log("uso sam u if stopa");
        }
        //rigidbody.velocity = Vector3.zero;
        //rigidbody.angularVelocity = Vector3.zero;
        //isIdle = true;
        //Debug.Log("mf is stoped");
        //rigidbody.isKinematic = false;
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
            //Debug.Log("hit is: " + hit.point.ToString());
            return hit.point;
        }
        else
        {
            return null;
        }
    }
    
}
