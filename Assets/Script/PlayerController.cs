using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField]
    public float speed = 5.0f;
    public bool right;
    public bool isgrounded = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoveLeftAndRight());
        StartCoroutine(Jump());
    }
    IEnumerator Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded == true)
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            isgrounded = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isgrounded == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator MoveLeftAndRight()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            right = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            right = false;
        }
        yield return new WaitForSeconds(1.0f);
    }

}
