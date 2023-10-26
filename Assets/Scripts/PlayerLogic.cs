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
    [SerializeField]
    Animator shaker;

    Rigidbody2D rb;
    [SerializeField]
    InputActionAsset controls;
    InputActionMap _actionMap;

    InputAction jumpAction;
    InputAction moveAction;

    float hInput = 0;

    [SerializeField]
    GameObject playerPieces;

    [SerializeField]
    ParticleSystem dustTrail;

    bool prev_grounded = false;

    public enum PlayerStates // not complex enough to be worth a whole state system
    {
        normal,
        dead,
        goal
    }

    PlayerStates state = PlayerStates.normal;

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
        if (state == PlayerStates.normal)
        {
            animator.SetBool("grounded", grounded);
            animator.SetFloat("speed", rb.velocity.x * Mathf.Abs(hInput));
        }

        if(grounded && !prev_grounded)
        {
            animator.SetTrigger("land");
            dustTrail.Play();
        }
    }

    private void LateUpdate()
    {
        prev_grounded = grounded;
    }

    private void OnDisable()
    {
        jumpAction.performed -= Jump_performed;
        jumpAction.canceled -= Jump_cancelled;
        jumpAction.started -= Jump_started;

        moveAction.performed -= Move;
        moveAction.canceled -= Move;
    }

    private void FixedUpdate()
    {
        if (state != PlayerStates.normal)
            return;

        Vector2 additional_velocity = Vector2.zero;

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

        if (!grounded && dustTrail.isPlaying)
            dustTrail.Stop();
    }

    void Jump_performed(InputAction.CallbackContext ctx)
    {
        if (rb == null)
            return;

        if (!grounded)
            return;

        //rb.AddForce(Vector2.up * jumpForce);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("jump");
    }

    void Jump_cancelled(InputAction.CallbackContext ctx)
    {
        if (rb == null)
            return;

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

    public void DeathByLaser()
    {
        Death();
    }

    public void DeathBySaw()
    {
        Death();
    }

    public void DeathByCrushed()
    {
        Death();
    }

    public void Death()
    {
        if (state == PlayerStates.dead)
            return;



        StartCoroutine(HandleDeathAnimations());

        
        //gameObject.SetActive(false);
    }

    IEnumerator HandleDeathAnimations()
    {
        state = PlayerStates.dead;
        shaker.SetTrigger("shake");
        var vel = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;

        yield return new WaitForSeconds(0.2f);

        GameObject p = Instantiate(Resources.Load("Pop") as GameObject);
        p.transform.position = transform.position;

        GameManager.instance.ResetLevel();
        var g = Instantiate(playerPieces, transform.position, Quaternion.identity);

        foreach (Rigidbody2D r2d in g.GetComponentsInChildren<Rigidbody2D>())
            r2d.velocity = -1 * vel;

        
        gameObject.SetActive(false);
    }
}
