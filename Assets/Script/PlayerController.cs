using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
    public float dashspeed = 50.0f;
    [SerializeField]public float dashduration = 0.5f;
    private bool dashonce = false;
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
    //I dont really like how the player can have multiple airdash with this code,depending if the duration is shortened.
    IEnumerator Dash()
    {
        Debug.Log("CanDash");

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

}
