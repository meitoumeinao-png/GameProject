using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField]
    [Header("Movement")]
    public float speed = 5.0f;
    public bool right;
    public float jumpforce = 10.0f;
    [Header("Gravity")]
    public bool isgrounded = true;
    [Header("Dash")]
    public float dashspeed = 10.0f;
    [SerializeField]public float dashduration = 0.5f;
    private float dashtimer;
    private float maxdashtime;
    private bool isdashing;
    private bool candash = true;
    private float dashcooldown = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxdashtime = dashduration;
    }

    // Update is called once per frame
    void Update()
    {
        MovingLeftOrRight();
        Jump();
        ToDash();
    }

    void FixedUpdate()
    {
        //If Player is dashing then stop move or jump input from cancelling.
        if (isdashing)
        {
            return;
        }
        
    }
    //Code Basically gets input from A D,Uses the input to move the object using linear Velocity
    void MovingLeftOrRight()
    {
        float moveinput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2((speed * moveinput), rb.linearVelocityY);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded == true )
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
            isgrounded = false;
        }
    }
    //Used CoRountine as this feature will have a cooldown
    void ToDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && candash == true)
        {
            StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
        candash = false;
        isdashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashspeed, 0f);
        yield return new WaitForSeconds(dashduration);
        rb.gravityScale = originalGravity;
        isdashing = false;
        yield return new WaitForSeconds(dashduration);
        candash = true;

    }

}
