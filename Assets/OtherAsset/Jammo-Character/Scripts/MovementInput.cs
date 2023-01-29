using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
public class MovementInput : MonoBehaviour
{

    public float Velocity;
    [Space]

    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed = 0.1f;
    public Animator anim;
    public float Speed;
    public float allowPlayerRotation = 0.1f;
    public Camera cam;
    //public CharacterController controller;
    public bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0, 1f)] public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)] public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)] public float StartAnimTime = 0.3f;
    [Range(0, 1f)] public float StopAnimTime = 0.15f;

    public float verticalVel;
    private Vector3 moveVector;

    private Rigidbody rigid;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        InputMagnitude();

        //isGrounded = controller.isGrounded;
        if (true)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        //moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        //rigid.velocity = (moveVector);

        TestJump();
    }

    void PlayerMoveAndRotation()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        InputZ = Input.GetAxisRaw("Vertical");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            if (desiredMoveDirection.sqrMagnitude > 0.01f)
                RotatePlayer(ThirdPersonCameraControll.IsRopeAim || ThirdPersonCameraControll.IsPetAim);
            rigid.velocity = desiredMoveDirection.normalized * Time.deltaTime * Velocity;

            // if (desiredMoveDirection.magnitude > 0.01f)
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            Vector3 velocity = desiredMoveDirection.normalized * Time.deltaTime * Velocity;
            velocity.y = rigid.velocity.y;
            rigid.velocity = velocity;
            //rigid.position += desiredMoveDirection.normalized * Time.deltaTime * Velocity;
            //rigid.MovePosition(transform.position + desiredMoveDirection * Time.deltaTime * Velocity);
        }
    }

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    private void RotatePlayer(bool isRotate)
    {
        if (isRotate) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    void InputMagnitude()
    {
        //Calculate Input Vectors
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        //Calculate the Input Magnitude
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player

        if (Speed > allowPlayerRotation)
        {
            if (anim)
            {
                anim.SetFloat("Blend", Speed, StartAnimTime, Time.deltaTime);
            }
            PlayerMoveAndRotation();
        }
        else if (Speed < allowPlayerRotation)
        {
            if (anim)
            {
                anim.SetFloat("Blend", Speed, StopAnimTime, Time.deltaTime);
            }
        }
    }

    public float jumpforce = 1f;
    private void TestJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
    }
}
