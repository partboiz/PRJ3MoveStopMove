using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private float updatePathInterval = 0.2f;
    private float updateTimer;
    private float minAttackInterval = 1f;
    private float attackTimer = 0f;

    public void OnEnter(Enemy enemy)
    {
        updateTimer = 0f;
        attackTimer = 0f;
        UpdateChaseTarget(enemy);
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.CurrentTarget == null)
        {
            enemy.ChangeState(enemy.PatrolState);  
            return;
        }

        updateTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (updateTimer >= updatePathInterval)
        {
            updateTimer = 0f;
            UpdateChaseTarget(enemy);
        }

        float distanceToTarget = Vector3.Distance(enemy.transform.position, enemy.CurrentTarget.position);

        if (distanceToTarget <= enemy.attackRange && attackTimer >= minAttackInterval && !enemy.isAttacking && enemy.canAttack)
        {
            enemy.ChangeState(enemy.AttackState);
            attackTimer = 0f;
        }
        else if (distanceToTarget > enemy.DetectionRange * 1.5f)
        {
            enemy.ChangeState(enemy.PatrolState);
        }
    }

    public void OnExit(Enemy enemy)
    {
        enemy.SetTarget(null);
        enemy.NavAgent.ResetPath();
    }

    private void UpdateChaseTarget(Enemy enemy)
    {
        if (enemy.CurrentTarget != null)
        {
            enemy.ChangeAnim(Constants.ANIM_RUN);
            Vector3 directionToTarget = (enemy.CurrentTarget.position - enemy.transform.position).normalized;
            Vector3 targetPosition = enemy.CurrentTarget.position - directionToTarget * enemy.attackRange;
            enemy.NavAgent.SetDestination(targetPosition);
        }
    }
}
