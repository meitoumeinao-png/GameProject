using System.Collections;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Player Movement Setting")]
    public float speed = 5f;
    public bool isGrounded;
    public bool left;
    public float jumpforce = 5f;


    void Start()
    {
        StartCoroutine(movement());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            left = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            left = true;
        }

    }
}
