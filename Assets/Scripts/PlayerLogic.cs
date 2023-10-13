using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    float jumpForce = 5.0f;
    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    LayerMask groundMask;
    [SerializeField]
    CircleCollider2D groundCheck;
    bool grounded = false;
    [SerializeField]
    Animator animator;

    Rigidbody2D rb;
    [SerializeField]
    InputActionAsset controls;
    InputActionMap _actionMap;

    InputAction jumpAction;
    InputAction moveAction;

    float hInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        _actionMap = controls.FindActionMap("Player");

        jumpAction = _actionMap.FindAction("Jump");
        jumpAction.performed += Jump_performed;
        jumpAction.canceled += Jump_cancelled;
        jumpAction.started += Jump_started;

        moveAction = _actionMap.FindAction("Horizontal Movement");
        moveAction.performed += Move;
        moveAction.canceled += Move;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("grounded", grounded);
        animator.SetFloat("speed", rb.velocity.x);
    }

    private void FixedUpdate()
    {
        Vector2 additional_velocity = Vector2.zero;
        /*var hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundMask);
        if (hit)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }*/

        grounded = false;
        GetComponent<Collider2D>().sharedMaterial.friction = 0;

        var overlap = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheck.radius, groundMask);
        if(overlap)
        {
            grounded = true;
            GetComponent<Collider2D>().sharedMaterial.friction = 1;

            Rigidbody2D other_rb = overlap.gameObject.GetComponent<Rigidbody2D>();

            if(other_rb != null)
            {
                additional_velocity = other_rb.velocity;
                additional_velocity.y = 0;
            }
        }

        rb.velocity = new Vector2(hInput *  speed, rb.velocity.y) + additional_velocity;
    }

    void Jump_performed(InputAction.CallbackContext ctx)
    {
        if (!grounded)
            return;

        //rb.AddForce(Vector2.up * jumpForce);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Jump_cancelled(InputAction.CallbackContext ctx)
    {
        if (rb.velocity.y <= 0)
            return;

        rb.velocity = Vector2.zero;
    }

    void Jump_started(InputAction.CallbackContext ctx)
    {

    }

    private void Move(InputAction.CallbackContext ctx)
    {
        var amount = ctx.ReadValue<float>();
        hInput = amount;
        Debug.Log(amount);
    }
}
