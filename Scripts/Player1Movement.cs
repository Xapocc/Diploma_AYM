using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    [SerializeField] private Light m_Light;
    [SerializeField] private Camera player1Camera;
    [SerializeField] private Camera player2Camera;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject player1;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorRun;
    [SerializeField] private Animator animatorIdle;
    [SerializeField] private Animator animatorJump;
    [SerializeField] private Animator animatorWalk;
    [SerializeField] private AudioSource forrestFootsteps;
    [SerializeField] private AudioSource concreteFootsteps;
    private AudioSource footsteps;

    //скорость персонажа
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    //Скорость прыжка персонажа
    [SerializeField] private float jumpSpeed = 8.0f;
    //Переменная гравитации персонажа
    [SerializeField] private float gravity = 20.0f;
    //Переменная движения персонажа
    private Vector3 moveDir = Vector3.zero;
    //Переменная, содержащая компонент CharacterController
    private CharacterController controller;

    private const float mapShiftX = 1000f;
    private const float camShiftX = 0f;
    private const float camShiftY = 0.845f;
    private const float camShiftZ = 0.241f;

    //private bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        footsteps = forrestFootsteps;
    }

    public void ChangeFootstepsSound(bool value) 
    {
        if (value)
            footsteps = concreteFootsteps;
        else
            footsteps = forrestFootsteps;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ChangeFootstepSoundTrigger")
        {
            footsteps.mute = true;
            ChangeFootstepsSound(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ChangeFootstepSoundTrigger")
        {
            footsteps.mute = true;
            ChangeFootstepsSound(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        footsteps.mute = true;
        player2Camera.transform.position = new Vector3(
        this.transform.position.x + mapShiftX + camShiftX,
        this.transform.position.y + camShiftY,
        this.transform.position.z + camShiftZ);

        if (controller.isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.runtimeAnimatorController = animatorRun.runtimeAnimatorController;
                }
                else
                {
                    animator.runtimeAnimatorController = animatorWalk.runtimeAnimatorController;
                }

                player1Camera.transform.parent = head.transform;
                footsteps.mute = false;
            }
            else
            {
                animator.runtimeAnimatorController = animatorIdle.runtimeAnimatorController;
                player1Camera.transform.parent = player1.transform;
            }
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);

            moveDir *= Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        }


        if (Input.GetKey(KeyCode.Space) && controller.isGrounded)
        {
            moveDir.y = jumpSpeed;

            animator.runtimeAnimatorController = animatorJump.runtimeAnimatorController;
            footsteps.mute = false;

            player1Camera.transform.parent = head.transform;
        }

        

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    Vector3 v3 = controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position;

        //    Debug.Log("x = " + v3.x + " y = " + v3.y + " z = " + v3.z);
        //}

        //if (Input.GetKeyDown(KeyCode.C) && !isCrouching)
        //{
        //    Vector3 v3 = controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position - controller.gameObject.transform.position;
        //    v3.y /= -2f;
        //    controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position = v3 + controller.gameObject.transform.position;

        //    Debug.Log(controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position.y + " v3 = " + v3.y);
        //    isCrouching = true;
        //}

        //if (Input.GetKeyUp(KeyCode.C) && isCrouching)
        //{
        //    Vector3 v3 = controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position - controller.gameObject.transform.position;
        //    v3.y *= -2f;
        //    controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position = v3 + controller.gameObject.transform.position;

        //    Debug.Log(controller.gameObject.GetComponentsInChildren<Camera>()[0].transform.position.y + " v3 = " + v3.y);
        //    isCrouching = false;
        //}

        if (Input.GetMouseButtonDown(1))
        {
            m_Light.enabled = !m_Light.enabled;
        }

        if (Input.GetMouseButton(0))
        {
            player2Camera.fieldOfView = 20;
        }
        else
        {
            player2Camera.fieldOfView = 60;
        }

        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);
    }
}
