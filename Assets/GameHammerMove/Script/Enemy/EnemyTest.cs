/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : Characterremake
{
    public Transform[] patrolPoints;  // Các điểm tuần tra
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 10f;  // Phạm vi phát hiện đối tượng "Character"

    private int currentPatrolIndex = 0;
    private NavMeshAgent navAgent;
    private bool isChasing = false;
    private bool hasAttacked = false;  // Để kiểm soát tấn công một lần

    void Start()
    {
        base.Start();  // Gọi Start() của Characterremake
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.speed = patrolSpeed;
            if (patrolPoints.Length > 0)
            {
                navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
    }

    void Update()
    {
        base.Update();  // Gọi Update() của Characterremake

        if (!isDying)
        {
            if (!isChasing)
            {
                Patrol();
                DetectCharacterInRange();  // Tìm kiếm nhân vật trong phạm vi
            }
            else
            {
                ChaseTarget();  // Đuổi theo mục tiêu
            }
        }
    }

    // Hàm tuần tra
    void Patrol()
    {
        if (navAgent.remainingDistance < 0.5f && patrolPoints.Length > 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    // Hàm đuổi theo mục tiêu
    void ChaseTarget()
    {
        if (currentTarget != null)
        {
            navAgent.speed = chaseSpeed;
            navAgent.SetDestination(currentTarget.position);

            // Kiểm tra khoảng cách tới mục tiêu
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            if (distanceToTarget <= attackRange)
            {
                // Nếu khoảng cách đủ gần, dừng di chuyển và tấn công
                navAgent.isStopped = true;

                if (!hasAttacked)
                {
                    Attack();  // Tấn công chỉ một lần khi đứng yên
                    hasAttacked = true;  // Đánh dấu đã tấn công
                }
            }
            else
            {
                // Tiếp tục di chuyển đến mục tiêu nếu không ở trong phạm vi tấn công
                navAgent.isStopped = false;
                hasAttacked = false;  // Reset trạng thái tấn công khi Enemy di chuyển
            }

            // Nếu mục tiêu ra khỏi phạm vi phát hiện, quay lại tuần tra
            if (distanceToTarget > detectionRadius)
            {
                isChasing = false;
                navAgent.speed = patrolSpeed;
                currentTarget = null;
                navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
                hasAttacked = false;  // Reset tấn công khi kết thúc đuổi theo
            }
        }
        else
        {
            isChasing = false;
        }
    }

    // Tìm kiếm nhân vật trong phạm vi detectionRadius
    void DetectCharacterInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Character") && hitCollider.transform != transform)
            {
                currentTarget = hitCollider.transform;
                isChasing = true;
                navAgent.speed = chaseSpeed;
                break;
            }
        }
    }

    // Hiển thị phạm vi detection trong chế độ Scene
    void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();  // Gọi OnDrawGizmosSelected() của Characterremake
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}*/