using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public CharacterController controller;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && controller.isGrounded)
        {
           // GetComponent<Animator>().
        }
    }
}
