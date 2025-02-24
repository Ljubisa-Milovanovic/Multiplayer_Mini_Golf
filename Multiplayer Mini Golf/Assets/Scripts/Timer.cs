using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    private bool startFlag = false;

    public bool StartFlag   
    {
        get { return startFlag; }   
        set { startFlag = value; }  
    }

    void Update()
    {
        if (startFlag)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if (remainingTime < 0)
            {
                remainingTime = 0;
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
       // Debug.Log("<color=orange>Timer info:</color> flag: "+startFlag+" remainign time: " + remainingTime);
    }
}
