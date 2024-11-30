using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Characterremake
{
    [SerializeField] CharacterController characController;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] private float speed = 1f;
    float Gravity = 5f;
    private float verticalVelocity;
    [SerializeField] Quaternion targetRotation;
    public bool isRunning;
    private float SpeedRotation = 5f;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveInput = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 move = right * moveInput.x + forward * moveInput.z;
        move.y = Vertical();

        characController.Move(move * speed * Time.deltaTime);

        bool isMoving = moveInput.x != 0 || moveInput.z != 0;
        if (isMoving)
        {
            targetRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * SpeedRotation);
            if (!isRunning)
            {
                ChangeAnim(Constants.ANIM_RUN);
                isRunning = true;
                canAttack = false; 
                Debug.Log("Chay");
            }
        }
        else
        {
            if (isRunning)
            {
                ChangeAnim(Constants.ANIM_IDLE);
                isRunning = false;
                canAttack = true; 
                Debug.Log("Dung yen");
            }
        }
    }

    private float Vertical()
    {
        if (characController.isGrounded) verticalVelocity = -1;
        else verticalVelocity -= Gravity * Time.fixedDeltaTime;
        return verticalVelocity;
    }
}
