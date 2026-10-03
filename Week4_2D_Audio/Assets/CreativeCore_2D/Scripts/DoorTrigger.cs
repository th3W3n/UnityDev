using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger entered;");
        anim.SetTrigger("PlayerProximity");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("trigger exited;");
        anim.SetTrigger("PlayerProximity");
    }
}
