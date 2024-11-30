using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characterremake : MonoBehaviour
{
    public Animator anim;
    private string currentAnim;
    [SerializeField] public float attackRange = 5f;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] internal float attackCooldown = 1f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform firePoint;
    private float nextAttackTime;
    private Transform currentTarget;
    public bool canAttack = true;
    protected bool isDying = false;
    [SerializeField] private float deathDelay = 3f;

    [SerializeField] private int defeatedEnemiesCount = 0;
    private Player playerMovementScript;

   protected void Start()
    {
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
        playerMovementScript = GetComponent<Player>();
    }

    protected void Update()
    {
        if (!isDying)
        {
            FindNearestTarget();
            if (currentTarget != null && canAttack && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
                canAttack = false;
            }
        }
    }

    void FindNearestTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        float closestDistance = attackRange;
        Transform nearestCharacter = null;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Character") && hitCollider.transform != transform)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestCharacter = hitCollider.transform;
                }
            }
        }
        currentTarget = nearestCharacter;
    }

    public void Attack()
    {
        if (currentTarget != null)
        {
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            transform.rotation = lookRotation;
            ChangeAnim("Attack");
            Debug.Log("tan cong");
            GameObject projectile = Instantiate(weaponPrefab, firePoint.position, firePoint.rotation);
            projectile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            Weapon projectileController = projectile.GetComponent<Weapon>();
            if (projectileController == null)
            {
                projectileController = projectile.AddComponent<Weapon>();
            }
            projectileController.SetShooter(transform);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (currentTarget.position - firePoint.position).normalized;
                rb.velocity = direction * projectileSpeed;
                StartCoroutine(RotateProjectileContinuously(projectile));
            }
        }
    }

    IEnumerator RotateProjectileContinuously(GameObject projectile)
    {
        float rotationSpeed = 500f;
        while (projectile != null)
        {
            projectile.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    public void OnHit()
    {
        if (!isDying)
        {
            isDying = true;
            StartCoroutine(DeathSequence());
            Debug.Log("Aaaaaaaaaaaaaaaaaaa");
        }
    }

    IEnumerator DeathSequence()
    {
        ChangeAnim("Dead");
        Debug.Log("da chet");
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;
        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().isKinematic = true;
        float deathAnimationTime = 1f;
        yield return new WaitForSeconds(deathAnimationTime);
        yield return new WaitForSeconds(deathDelay - deathAnimationTime);
        Destroy(gameObject);
    }

   protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    public void IncrementDefeatCountAndCheck()
    {
        defeatedEnemiesCount++;
        if (defeatedEnemiesCount >= 3)
        {
            IncreaseScaleAndRange(0.1f, 0.5f);
            defeatedEnemiesCount = 0;
            Debug.Log("grow up");
        }
    }

    private void IncreaseScaleAndRange(float scaleIncrease, float rangeIncrease)
    {
        transform.localScale += Vector3.one * scaleIncrease;
        attackRange += rangeIncrease;
    }
}
