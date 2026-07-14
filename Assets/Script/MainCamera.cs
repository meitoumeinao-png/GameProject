using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Vector3 pos;
    public Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + pos;
    }
}
