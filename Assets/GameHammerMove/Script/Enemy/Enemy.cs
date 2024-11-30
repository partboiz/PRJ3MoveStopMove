using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Characterremake
{
    private IState currentState;
    private NavMeshAgent navAgent;
    public Transform CurrentTarget { get; private set; }

    [SerializeField] private float detectionRange = 10f;
    public float DetectionRange => detectionRange;
    public NavMeshAgent NavAgent => navAgent;

    public bool isAttacking { get; private set; }

    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }  
    public IdleState IdleState { get; private set; }

    protected new void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
        PatrolState = new PatrolState();
        ChaseState = new ChaseState();
        AttackState = new AttackState();
        IdleState = new IdleState(); 
        ChangeState(IdleState);
    }

    protected new void Update()
    {
        base.Update();
        currentState?.OnExecute(this);
        if (CurrentTarget != null && Vector3.Distance(transform.position, CurrentTarget.position) <= attackRange && !isAttacking)
        {
            Attack();
        }
    }

    public new void Attack()
    {
        if (CurrentTarget != null && Vector3.Distance(transform.position, CurrentTarget.position) <= attackRange && !isAttacking)
        {
            base.Attack();
            isAttacking = true;
            canAttack = false;
            StartCoroutine(ResetAttackState());
        }
    }


    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        canAttack = true;
    }


    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public void SetTarget(Transform target)
    {
        CurrentTarget = target;
    }


    protected new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
