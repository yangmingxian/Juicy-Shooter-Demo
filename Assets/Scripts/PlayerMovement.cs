using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    PlayerAnimationController animationController;

    public Vector2 direction;
    public float speed;
    public float moveDamping;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    protected void Update()
    {
        HandleInputs();
    }
    protected void FixedUpdate()
    {
        Move();
    }

    void HandleInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direction = new Vector2(moveX, moveY);
        if (direction.magnitude > 0)
        {
            animationController.PlayMoveLoop();
        }
        else if (direction.magnitude < 0.1f)
        {
            animationController.PlayIdleLoop();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isDashing && !isDashCooldown && direction.magnitude > 0)
            {
                StartCoroutine(PerformDash());
            }
        }

    }

    public void Move()
    {
        rb.velocity += speed * Time.fixedDeltaTime * direction.normalized;
        rb.velocity *= Mathf.Pow(1 - moveDamping, Time.fixedDeltaTime * 10);
    }

    public float dashDistance;
    public float dashDuration;
    private bool isDashing;
    public float smoothTime = 0.1f;


    private bool isDashCooldown;
    public float dashCooldown;
    private float dashCooldownTimer;

    public ParticleSystem speedLine;

    public void Dash()
    {
        if (!isDashing && direction.magnitude > 0)
        {
            StartCoroutine(PerformDash());
        }
    }


    IEnumerator PerformDash()
    {
        isDashing = true;
        Vector2 targetVelocity = direction.normalized * dashDistance / dashDuration;
        Vector2 currentVelocity = Vector2.zero;

        speedLine.Play();


        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = targetVelocity;

        isDashing = false;
        isDashCooldown = true;
        dashCooldownTimer = dashCooldown;
        StartCoroutine(DashCooldownTimer());
        speedLine.Stop();
        
    }
    IEnumerator DashCooldownTimer()
    {
        while (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            yield return null;
        }

        isDashCooldown = false;
    }


}
