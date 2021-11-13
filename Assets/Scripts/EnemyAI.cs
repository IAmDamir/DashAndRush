using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rigidbody;
    public MeshCollider meshCollider;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Values for attack
    float dashCooldown = 1f;
    float nextDash;

    // Values to rotate
    public float rotationSpeed = 1f;

    private Coroutine lookCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();

        Physics.IgnoreLayerCollision(11, 12);
    }

    private void FixedUpdate()
    {
        // Check for Player in sight or attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        } else if (playerInSightRange && playerInAttackRange && Time.time > nextDash)
        {
            
            nextDash = Time.time + dashCooldown;
            StartCoroutine(AttackPlayer());
        } else
        {
            ChasePlayer();
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
        } else if (walkPointSet) {
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
        agent.SetDestination(player.position);
    }

    private IEnumerator AttackPlayer()
    {

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            StartRotating();
            yield return new WaitForSeconds(3);
            StartCoroutine(Dash());

            
        }
    }

    public void StartRotating()
    {
        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);

        float time = 0f;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * rotationSpeed;

            yield return null;
        }
    }

    IEnumerator Dash()
    {
        agent.enabled = false;
        meshCollider.isTrigger = true;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        rigidbody.AddRelativeForce(Vector3.forward * 25, ForceMode.Impulse);

        yield return new WaitForSeconds(2);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        alreadyAttacked = false;

        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        meshCollider.isTrigger = false;
        agent.enabled = true;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth < 1)
        {
            Destroy(player);
        }
    }
}
