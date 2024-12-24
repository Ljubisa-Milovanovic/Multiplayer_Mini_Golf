using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udarac : MonoBehaviour
{
    Rigidbody rbLoptice;
    //public GameObject loptica;
    public float snagaUdarca;
    // Start is called before the first frame update
    void Start()
    {
        //set masu iz poblic vrednosti jer addforce zavisi od mase
        rbLoptice = GetComponent<Rigidbody>();
        proslaPozicija = rbLoptice.position;
    }
    private bool pomeraSe = true;
    private Vector3 proslaPozicija;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (proslaPozicija == rbLoptice.position)
        {
            pomeraSe = false;
            Debug.Log("Ne pomera se loptica trnt");
        }
        else
        {
            pomeraSe = true;
            Debug.Log("Pomera se loptica trnt");       
        }
        proslaPozicija = rbLoptice.position;
        if (pomeraSe)
        {
            int i = 0;
            if (Math.Abs(rbLoptice.position.x - proslaPozicija.x) < 0.0001)
            {
                i++;
                rbLoptice.velocity = new Vector3(0, rbLoptice.velocity.y, rbLoptice.velocity.z);
                //rbLoptice.velocity = new Vector3(proslaPozicija.x, rbLoptice.position.y, rbLoptice.position.z);
            }
            //if (Math.Abs(rbLoptice.position.y - proslaPozicija.y) < 0.01)
            //{
            //    i++;
            //    //rbLoptice.velocity = new Vector3(rbLoptice.position.x, proslaPozicija.y, rbLoptice.position.z);
            //}
            if (Math.Abs(rbLoptice.position.z - proslaPozicija.z) < 0.0001)
            {
                i++;
                rbLoptice.velocity = new Vector3(rbLoptice.velocity.x, rbLoptice.velocity.y, 0);
                //rbLoptice.velocity = new Vector3(rbLoptice.position.x, rbLoptice.position.y, proslaPozicija.z);
            }
            //if (i > 1)
            //{
            //    rbLoptice.velocity = new Vector3(0, rbLoptice.velocity.y, 0);
            //}
        }
        else
        {
            if (Input.GetMouseButtonUp(0))//left click pusten
            {
                rbLoptice.AddForce(transform.right * snagaUdarca, ForceMode.Force); //vector3 zavisi od misa
                Debug.Log("Levi klik pusten");
            }
        }
    }
}
