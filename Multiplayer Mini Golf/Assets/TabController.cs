using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    public Animator animator;

    public void OpenTab()
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }
    }

    public void CloseTab()
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }
    }
}
