using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool left, right, up, down;

    private Rigidbody rb;
    private Collider col;

    public float speed;
    private static ILogger logger = Debug.unityLogger;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(left)
        {
            rb.AddForce(Vector3.left * speed);
        }
        
        if(right)
        {
            rb.AddForce(Vector3.right * speed);
        }

        if(up)
        {
            rb.AddForce(Vector3.up * speed);
        }

        if(down)
        {
            rb.AddForce(Vector3.down * speed);
        }

    }

    public void MoveLeft()
    {
        left = true;
    }

    public void StopMovingLeft()
    {
        left = false;
    }

    public void MoveRight()
    {
        right = true;
    }

    public void StopMovingRight()
    {
        right = false;
    }

    public void MoveUp()
    {
        up = true;
    }

    public void StopMovingUp()
    {
        up = false;
    }

    public void MoveDown()
    {
        down = true;
    }

    public void StopMovingDown()
    {
        down = false;
    }

}
