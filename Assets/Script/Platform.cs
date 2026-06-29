using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public PlayerController isgrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>();
        GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            isgrounded.isgrounded = true;
        }
    }
}
