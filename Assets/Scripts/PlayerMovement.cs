using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float jumpForce = 5f; 
    public float runSpeedMultiplier = 1.5f; 
    

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    
    private float m_JumpCooldown = 0.5f; 
    private float m_LastJumpTime = -0.5f; 

    
    private bool m_JumpInput;
    private bool m_IsRunningInput;


    AudioSource m_AudioSource;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_JumpInput = true;
        }

       
        m_IsRunningInput = Input.GetKey(KeyCode.LeftShift);
    }

   

    void FixedUpdate()
    {
       
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        
        bool isRunning = isWalking && m_IsRunningInput;
        m_Animator.SetBool("IsRunning", isRunning);

       
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        HandleJumping();

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
    }


    private void HandleJumping()
    {
     
        if (m_JumpInput)
        {
            
            bool canJump = (Time.time - m_LastJumpTime) >= m_JumpCooldown;

          
            if (canJump)
            {
                
                m_Animator.SetTrigger("Jump");

                
                m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

              
                m_LastJumpTime = Time.time;
            }

          
            m_JumpInput = false;
        }
    }

    void OnAnimatorMove()
    {

        float speed = m_Animator.deltaPosition.magnitude;

 
        if (m_Animator.GetBool("IsRunning"))
        {
            speed *= runSpeedMultiplier;
        }

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * speed);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}