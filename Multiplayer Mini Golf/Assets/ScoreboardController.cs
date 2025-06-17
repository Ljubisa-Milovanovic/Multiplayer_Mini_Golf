using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    public Animator animator;

    public void OpenScoreBoard()
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }
    }

    public void CloseScoreBoard()
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }
    }
}
