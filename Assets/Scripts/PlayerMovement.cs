using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    public Timer timer;

    public GameObject player;
    public Collider meshCollider;
    public ParticleSystem glow;
    public VisualEffect electricity;
    private ParticleSystem.EmissionModule glowEmission;
    private new Rigidbody rigidbody;

    private PlayerControls controls;
    private Vector2 move;

    public Transform cam;

    public float turnSmoothTime = 0.1f;
    public float speed = 10f;
    public float dashSpeed = 10f;
    private float turnSmoothVelocity;

    public int maxHealth = 50;
    public int currentHealth;
    public HealthBar healthBar;

    private float dashCooldown = 1f;
    private float nextDash;
    private bool isDashing = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        controls = new PlayerControls();
        rigidbody = GetComponent<Rigidbody>();
        glowEmission = glow.emission;

        controls.Gameplay.Dash.performed += ctx => StartCoroutine(Dash());

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    private IEnumerator Dash()
    {
        if (Time.time > nextDash)
        {
            if (electricity)
            {
                electricity.Stop();
            }

            nextDash = Time.time + dashCooldown;

            meshCollider.isTrigger = true;
            isDashing = true;

            rigidbody.AddRelativeForce(Vector3.forward * dashSpeed, ForceMode.VelocityChange);

            yield return new WaitForSeconds(0.2f);

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            meshCollider.isTrigger = false;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);

            if (electricity)
            {
                electricity.Play();
            }
        }
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && !meshCollider.isTrigger)
        {
            TakeDamage(10);
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(move.x, 0, move.y).normalized;

        if (direction.magnitude >= 0.1f && !isDashing)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Move char. with transform
            transform.position += moveDirection.normalized * Time.deltaTime * speed;

            // Move char. with rigidbody
            //rigidbody.MovePosition(transform.position + (moveDirection.normalized * Time.deltaTime * speed));
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth < 1)
        {
            timer.StopTimer();
            Destroy(player);
        }
    }
}