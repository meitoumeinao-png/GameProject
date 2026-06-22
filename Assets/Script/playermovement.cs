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
    public float playerinput;
    public float playerfloattime = 3f;
    //I guess I want to manage the time taken for the player to move with coroutine,so I will implement travel time and transform for it to work in a 2d Environment

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

 
}
