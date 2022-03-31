using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] int jumpForce;
     
     
    [SerializeField] float fallMultiplier;
    [SerializeField] float interval;
    float lastTime = 0; 
     
    
    // last time of jump is noted with every jump,
    // because we want to block jumping for the next x seconds,
    // because it takes time before the player leaves the ground
    Rigidbody rb;
    public bool isGrounded = false;

    Animator anim;

    public int lastAnimState = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
         
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // if the player is on the ground,play audio
            isGrounded = true;    // player is grounded
            Debug.Log("landed");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // isGrounded = false;

            anim.SetTrigger("jump");

            Debug.Log("airborn");
        }
    }

    void MakeJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        

    }


    void JumpDetection()
    {
        if (isGrounded &&  ((Input.GetKeyDown(KeyCode.Space)) || Input.GetKeyDown(KeyCode.Mouse0))  && Time.time >= lastTime + interval)
        {
            
            lastTime = Time.time;
            MakeJump();
        }
    }

    void IncreaseFallSpeed()
    {
        // increase velocity in downwards direction by gravity multiplied by the multiplier
        // (1 times gravity already applied by the physics engine) and deltaTime
        // we use vector2.up but then we multiply by gravity which is negative
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            
    }

    void Update()
    {
        JumpDetection();
        
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
            IncreaseFallSpeed();
    }
}