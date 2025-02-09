using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udarac : MonoBehaviour
{
    public Material[] materials;
    private int materialCounter = 0;
    Renderer rend;

    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = .05f; //The velocity below which the rigidbody will be considered as stopped

    [SerializeField] private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAiming;

    private float lastYposition;
    private bool isGrounded;

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
    }

    private void FixedUpdate()
    {
        isGrounded = (lastYposition == transform.position.y); // Checks if Y has changed since last frame
        lastYposition = transform.position.y;
        if (rigidbody.velocity.magnitude < stopVelocity && isGrounded)
        {
            Stop();   
        }
        if (isIdle && isGrounded)
        {
            Debug.Log("mf is idle and ready to explode.......PAUSE");
            if(materialCounter < 15)
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
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rigidbody.AddForce(-direction * strength * shotPower); //ne dodat forcemode.impulse
        isIdle = false;
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
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        isIdle = true;
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
