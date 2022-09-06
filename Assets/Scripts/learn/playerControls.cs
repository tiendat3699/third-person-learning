using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerControls : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private NewPlayerInput input;
    private int velocityState;
    private float velocity = 0;
    private float targetSpeed;
    private bool sprint;
    private Vector2 currentMovement;
    private void Awake() {
        input = new NewPlayerInput();
        input.playerControls.move.performed += (ctx)=> {
            currentMovement = ctx.ReadValue<Vector2>();
        };

        input.playerControls.move.canceled += (ctx) => {
            currentMovement = ctx.ReadValue<Vector2>();
        };

        input.playerControls.sprint.performed += (ctx)=> {
            sprint = ctx.ReadValueAsButton();
        };
    }

    private void OnEnable() {
        input.playerControls.Enable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        velocityState = Animator.StringToHash("velocity");
    }

    // Update is called once per frame
    void Update()
    {
        HandheldMove();
        HandheldRotation();
    }

    private void OnDisable() {
        input.playerControls.Disable();
    }

    private void HandheldMove() {
        if(currentMovement.x != 0 || currentMovement.y != 0) {
            targetSpeed = 0.3f;
            if(sprint) {
                targetSpeed = 1f;
            }
            velocity = Mathf.Lerp(velocity, targetSpeed, 10f * Time.deltaTime);
            animator.SetFloat(velocityState, velocity);
        } else {
            targetSpeed = 0f;
            velocity = Mathf.Lerp(velocity, targetSpeed, 10f * Time.deltaTime);
            if(velocity < 0.01f) velocity = 0;
            animator.SetFloat(velocityState,velocity);
        }
    }

    private void HandheldRotation() {
        Vector3 newPositon = new Vector3(currentMovement.x, 0, currentMovement.y);
        if(newPositon != Vector3.zero) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newPositon), 10f * Time.deltaTime);
        }
    }
}
