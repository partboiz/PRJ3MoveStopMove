using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private Vector3[] patrolPoints;
    private int currentPatrolIndex;
    private float patrolWaitTime = 2f;
    private float waitTimer;
    private bool isWaiting;

    public void OnEnter(Enemy enemy)
    {
        patrolPoints = new Vector3[4];
        float patrolRadius = 10f;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float angle = i * (360f / patrolPoints.Length);
            float x = enemy.transform.position.x + patrolRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = enemy.transform.position.z + patrolRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            patrolPoints[i] = new Vector3(x, enemy.transform.position.y, z);
        }
        currentPatrolIndex = 0;
        SetDestinationToNextPoint(enemy);
    }

    public void OnExecute(Enemy enemy)
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                isWaiting = false;
                SetDestinationToNextPoint(enemy);
            }
            return;
        }
        if (!enemy.NavAgent.pathStatus.Equals(NavMeshPathStatus.PathInvalid))
        {
            if (!enemy.NavAgent.pathPending && enemy.NavAgent.remainingDistance <= enemy.NavAgent.stoppingDistance)
            {
                isWaiting = true;
                waitTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, enemy.DetectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Character") && hitCollider.transform != enemy.transform)
            {
                enemy.SetTarget(hitCollider.transform);
                enemy.ChangeState(enemy.ChaseState);
                return;
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
        enemy.NavAgent.ResetPath();
    }

    private void SetDestinationToNextPoint(Enemy enemy)
    {
        if (enemy.NavAgent != null && patrolPoints.Length > 0)
        {
            enemy.ChangeAnim(Constants.ANIM_RUN); 
            enemy.NavAgent.SetDestination(patrolPoints[currentPatrolIndex]);
        }
    }
}
