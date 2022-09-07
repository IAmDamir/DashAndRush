using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Collider meshCollider;
    public VisualEffect electricity;

    public int maxHealth = 30;
    public int currentHealth;
    //public HealthBar healthBar;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;

    private bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;

    private bool alreadyAttacked = false;

    // States
    public float sightRange, attackRange;

    public bool playerInSightRange, playerInAttackRange;

    // Values for attack
    private float dashCooldown = 1f;

    public int dashPower = 50;
    private float nextDash;

    // Values to rotate
    public float rotationSpeed;

    private Coroutine lookCoroutine;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;
        }
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        meshCollider = GetComponent<Collider>();

        Physics.IgnoreLayerCollision(11, 12);
    }

    private void FixedUpdate()
    {
        ChasePlayer();

        // Check for Player in sight or attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && !meshCollider.isTrigger)
        {
            TakeDamage(10);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else if (walkPointSet && player)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        if (player)
        {
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            StartRotating();
            yield return new WaitForSeconds(2);
            if (Time.time > nextDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    public void StartRotating()
    {
        LookAt();
        /*
        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt());
        */
    }

    private void LookAt()
    {
        /*
        var turnTowardNavSteeringTarget = agent.steeringTarget;

        //Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        //rigidbody.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.Rotate(0, player.position.x * Time.deltaTime * rotationSpeed, 0);

        float time = 0f;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime;

            yield return null;
        }
        */
    }

    private IEnumerator Dash()
    {
        nextDash = Time.time + dashCooldown;

        electricity.Stop();

        //agent.enabled = false;
        meshCollider.isTrigger = true;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        rigidbody.AddRelativeForce(Vector3.forward * dashPower, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        //agent.enabled = true;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        alreadyAttacked = false;

        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        meshCollider.isTrigger = false;

        yield return new WaitForSeconds(dashCooldown);

        electricity.Play();
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //healthBar.SetHealth(currentHealth);

        if (currentHealth < 1)
        {
            Destroy(gameObject);
        }
    }
}