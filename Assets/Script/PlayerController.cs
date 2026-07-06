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
    [Header("Jump")]
    private bool isjumping = false;
    public float jumpforce = 2.0f;
    private float secondspassed;
    private float maxseconds = 5.0f;
    private float maxjumpforce = 10.0f;
    [Header("Collision Detection")]
    private float groundcheckdistance = 3.0f;
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

    //Issue with this is that It only gets it for a frame and then is overwritten
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
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, Vector2.down*groundcheckdistance, Color.green);
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
 
    //I want to have a charging mechanic to increase jumpforce proportional to time while they charge,but i dont want them to jump out of frame.So I guess
    //I will make it so that as long as they hold down the spacebar, time will build up and by then.it will be seconds/max second * maxjumpforce
    void Jump()
    {
        if(Input.GetKey(KeyCode.Space) && isgrounded == true)
        {
            isjumping = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX,jumpforce);
        }
        while (isgrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            secondspassed =+ Time.fixedDeltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isjumping = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX,(secondspassed/maxseconds)*maxjumpforce);
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
    
    private void Awake()
    {
        ///if (isgrounded == true)
        ///{
           /// Debug.Log("Yes");
        ///}
    }

}
