using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{




    [Header("Attack Information")]
    [SerializeField] private bool isAttacking;
    [SerializeField] private float comboTime = .3f;
    [SerializeField] private float comboTimeWindow;
    private int comboCounter; 

    [Header("Movement Speeds")]
    [SerializeField] private float jumpForce;
    [SerializeField] public float moveSpeed;
    private float xInput;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;
    private float dashTime;


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        Movement();
        CheckInput();
        AnimationControler();
        FlipController();
    }




    private void CheckInput() 
    {
        // Walking
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) Jump();

        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift)) Dash();

        // Attack
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartAttackEvent();
        }
    }
    private void StartAttackEvent()
    {
        if (isGrounded)
        {
            if (comboTimeWindow < 0) comboCounter = 0;
            isAttacking = true;
            comboTimeWindow = comboTime;
        }
    }
    private void Dash()
    {

        if (dashCooldownTimer < 0 && !isAttacking )
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }

    }
    private void Movement()
    {
        if (isAttacking)
        {
            // Attacking
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            // Dashing
            rb.velocity = new Vector2(facingDirection * dashSpeed, 0);
        } else
        {
            // Walking
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        }
    }
    private void Jump()
    {
        if (isGrounded) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void AnimationControler() 
    {
        bool isMoving = (rb.velocity.x != 0) ? true : false;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);

    }

    public void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight) Flip();
        if (rb.velocity.x < 0 && facingRight) Flip();
    }
    public void AttackOver()
    {
        // This function is being used in PlayerAnimEvents.cs
        isAttacking = false;
        comboCounter ++;
        
        if (comboCounter > 2) comboCounter = 0;
    }



    
}
