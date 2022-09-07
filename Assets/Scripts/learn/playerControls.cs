using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerControls : MonoBehaviour
{
    public Transform cam;
    public float WalkSpeed = 2f;
    public float RunSpeed = 5f;
    public float gravity = 15f;
    public float jumpForce = 8f;
    private CharacterController characterController;
    private Animator animator;
    private NewPlayerInput input;
    private int velocityState;
    private int jumpState;
    private int groundedState;
    private int fallingState;
    private float velocity = 0;
    private float targetSpeed;
    private bool sprint;
    private bool jump;
    private bool jumping;
    private bool isGrounded;
    private float speed;
    private Vector2 currentMovementInput;
    private Vector3 moveDir;
    private float initVelocity;
    private float turnVelocity;
    private void Awake() {
        input = new NewPlayerInput();

        input.playerControls.move.performed += (ctx)=> currentMovementInput = ctx.ReadValue<Vector2>();
        input.playerControls.move.canceled += (ctx) => currentMovementInput = ctx.ReadValue<Vector2>();

        input.playerControls.sprint.performed += (ctx)=> sprint = ctx.ReadValueAsButton();

        input.playerControls.jump.performed += (ctx) => jump = ctx.ReadValueAsButton();
        input.playerControls.jump.canceled += (ctx) => jump = ctx.ReadValueAsButton();
    }

    private void OnEnable() {
        input.playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController =  GetComponent<CharacterController>();
        velocityState = Animator.StringToHash("velocity");
        jumpState = Animator.StringToHash("isJump");
        groundedState = Animator.StringToHash("isGrounded");
        fallingState = Animator.StringToHash("isFalling");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;

        HandleMove();
        HandleRotation();
        HandleGravity();
        HandleJump();
    }

    private void OnDisable() {
        input.playerControls.Disable();
    }

    private void HandleMove() {
        speed = WalkSpeed;
        if(currentMovementInput != Vector2.zero) {
            targetSpeed = 0.3f;
            if(sprint) {
                targetSpeed = 1f;
                speed = RunSpeed;
            }
            velocity = Mathf.Lerp(velocity, targetSpeed, 10f * Time.deltaTime);
            animator.SetFloat(velocityState, velocity);
        } else {
            speed = 0;
            targetSpeed = 0f;
            velocity = Mathf.Lerp(velocity, targetSpeed, 10f * Time.deltaTime);
            if(velocity < 0.01f) velocity = 0;
            animator.SetFloat(velocityState,velocity);
        }

        characterController.Move(moveDir.normalized * speed * Time.deltaTime + new Vector3(0, initVelocity, 0) * Time.deltaTime);
    }

    private void HandleJump() {
        if(jump && !jumping && isGrounded) {
            animator.SetBool(jumpState, true);
            initVelocity = jumpForce;
            jumping = true;
        }
    }

    private void HandleGravity() {
        if(isGrounded && !jumping) {
            animator.SetBool(jumpState, false);
            animator.SetBool(groundedState, true);
            initVelocity = -gravity * Time.deltaTime;
        } else {
            animator.SetBool(groundedState, false);
            initVelocity -= gravity * Time.deltaTime;
            jumping = jump || false;
        }
    }

    private void HandleRotation() {
        if(currentMovementInput != Vector2.zero) {
            Vector3 dir = new Vector3(currentMovementInput.x, 0, currentMovementInput.y).normalized;
            float targetAngle = MathF.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            moveDir =  Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }
    }
}
