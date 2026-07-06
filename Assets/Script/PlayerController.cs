using System;
using System.Collections;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    private float jumptimecounter;
    private float maxjumpforce;
    private float duration = 10.0f;
    [Header("Collision Detection")]
    [SerializeField] private float groundcheckdistance;
    public bool isgrounded;
    [SerializeField]private LayerMask ground;
    [Header("Dash")]
    public float dashspeed = 50.0f;
    [SerializeField]public float dashduration = 0.5f;
    private float dashtimer;
    private float maxdashtime;
    private bool isdashing;
    private bool candash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxdashtime = dashduration;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isgrounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        MovingLeftOrRight();
        StartCoroutine(SuperJump());
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
        Flip(moveinput);
        rb.linearVelocity = new Vector2((speed * moveinput), rb.linearVelocityY);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded == true )
        {
            Debug.Log("Jump");
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
    //I dont really like how the player can have multiple airdash with this code,depending if the duration is shortened
    // DO STATE THIS IN LATER COMMIT.
    IEnumerator Dash()
    {
        Debug.Log("CanDash");
        dashtimer += Time.deltaTime;
        if (dashtimer <= maxdashtime)
        {
            yield return null;
        }
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
        dashtimer = 0f;

    }
    //Collision Detection
    private void HandleCollision()
    {
        isgrounded = Physics2D.Raycast(transform.position, Vector2.down, groundcheckdistance, ground);
    }

    //Flip 
    void Flip(float moveinput)
    {
        if (moveinput > 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
    //When I jump the object does not move;Capsule Collision Issue;
    IEnumerator SuperJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            isgrounded = false;
            float time =+ Time.deltaTime;
            jumptimecounter = time;
            yield return new WaitForSecondsRealtime(duration);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpforce * jumptimecounter);
            jumptimecounter = 0;
            yield return null;
        }

    }
}
