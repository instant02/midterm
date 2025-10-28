using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Women : MonoBehaviour
{
    [Header("Target")]
    public Transform playerTransform;

    [Header("Movement")]
    public float stoppingDistance = 2.0f;

    [Header("Combat")]
    public float throwSpeed = 15f;

    public GameObject papper;

    private NavMeshAgent agent;
    private Animator animator;

    public float detectionRadius = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        if (animator == null)
            Debug.LogError("Animator component not found on " + gameObject.name);

        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (playerTransform != null && agent != null)
        {
            agent.SetDestination(playerTransform.position);
        }

        UpdateAnimatorState();
    }

    void UpdateAnimatorState()
    {
        if (animator == null || agent == null) return;

        float currentSpeed = agent.velocity.magnitude;

        if (currentSpeed > 0.1f)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void ThrowPapper()
    {
        if (papper == null)
        {
            Debug.LogError("Papper Prefab이 할당되지 않았습니다!");
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform enemyTarget = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemyTarget = hitCollider.transform;
                break; 
            }
        }


        if (enemyTarget == null)
        {

            return;
        }


        Vector3 throwDirection = enemyTarget.position - transform.position;


        Vector3 lookDirection = throwDirection;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        Vector3 spawnPosition = transform.position + (Vector3.up * 1.5f) + (transform.forward * 0.5f);

        GameObject papperInstance = Instantiate(papper, spawnPosition, Quaternion.LookRotation(throwDirection));

        Rigidbody rb = papperInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(throwDirection.normalized * throwSpeed, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogWarning("Papper Prefab에 Rigidbody 컴포넌트가 없습니다!");
        }
    }
}