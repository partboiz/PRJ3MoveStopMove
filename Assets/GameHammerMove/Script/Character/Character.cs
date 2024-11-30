using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator anim;
    private string currentAnim;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float fireSpeed = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float weaponLifetime = 3f;
    private float nextAttackTime;
    private Transform currentTarget;
    protected bool isMoving;
    private bool isAttacking;
    private bool isDead = false;  
    [SerializeField] private float attackRotateSpeed = 15f;
    [SerializeField] protected string targetTag = "Character";
    [SerializeField] private float weaponIgnoreTime = 0.3f;
    private List<WeaponTracker> activeWeapons = new List<WeaponTracker>();

    protected virtual void Start()
    {
        if (firePoint == null)
        {
            GameObject newFirePoint = new GameObject("FirePoint");
            firePoint = newFirePoint.transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = new Vector3(0, 0, 2f);
        }
    }

    void Update()
    {
        if (isDead) return;

        FindNearestTarget();

        if (CanAttack() && !isAttacking)
        {
            StartAttack();
            nextAttackTime = Time.time + attackCooldown;
        }

        CheckWeaponLifetime();
    }
    private class WeaponTracker
    {
        public GameObject weapon;
        public Vector3 spawnPosition;
        public float spawnTime;
        public Collider ownerCollider;

        public WeaponTracker(GameObject w, Vector3 pos, Collider owner)
        {
            weapon = w;
            spawnPosition = pos;
            spawnTime = Time.time;
            ownerCollider = owner;
        }
    }

    private void CheckWeaponLifetime()
    {
        for (int i = activeWeapons.Count - 1; i >= 0; i--)
        {
            WeaponTracker tracker = activeWeapons[i];
            if (tracker.weapon != null)
            {
                if (Time.time - tracker.spawnTime >= weaponLifetime)
                {
                    Destroy(tracker.weapon);
                    activeWeapons.RemoveAt(i);
                }
            }
            else
            {
                activeWeapons.RemoveAt(i);
            }
        }
    }

    private void RotateToTarget(float speed)
    {
        if (currentTarget == null) return;

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        }
    }

    private bool CanAttack()
    {
        return currentTarget != null &&
               Time.time >= nextAttackTime &&
               !isMoving;
    }

    private void FindNearestTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        float closestDistance = attackRange;
        Transform nearestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag) && hitCollider.gameObject != gameObject)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hitCollider.transform;
                }
            }
        }
        currentTarget = nearestEnemy;
    }

    private void StartAttack()
    {
        if (currentTarget == null) return;
        isAttacking = true;
        StartCoroutine(AttackRotationCoroutine());
        Debug.Log("tan cong");
        ChangeAnim(Constants.ANIM_ATTACK);
        Invoke("PerformAttack", 0.2f);
    }

    private System.Collections.IEnumerator AttackRotationCoroutine()
    {
        float attackDuration = 0.7f;
        float elapsedTime = 0f;
        while (elapsedTime < attackDuration && currentTarget != null)
        {
            RotateToTarget(attackRotateSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void PerformAttack()
    {
        if (weaponPrefab == null || firePoint == null || currentTarget == null)
        {
            EndAttack();
            return;
        }
        GameObject weapon = Instantiate(weaponPrefab, firePoint.position, Quaternion.Euler(90f, 90f, -90f));
        Collider characterCollider = GetComponent<Collider>();
        if (weapon.TryGetComponent<Collider>(out Collider weaponCollider))
        {
            Physics.IgnoreCollision(weaponCollider, characterCollider, true);
        }
        Vector3 direction = (currentTarget.position - firePoint.position).normalized;
        if (weapon.TryGetComponent<Rigidbody>(out Rigidbody weaponRb))
        {
            weaponRb.velocity = direction * fireSpeed;
        }
        activeWeapons.Add(new WeaponTracker(weapon, firePoint.position, characterCollider));
        StartCoroutine(EnableCollisionAfterDelay(weapon));
        Invoke("EndAttack", 0.5f);
    }

    private IEnumerator EnableCollisionAfterDelay(GameObject weapon)
    {
        yield return new WaitForSeconds(weaponIgnoreTime);
        if (weapon != null && weapon.TryGetComponent<Collider>(out Collider weaponCollider))
        {
            WeaponTracker tracker = activeWeapons.Find(w => w.weapon == weapon);
            if (tracker != null && tracker.ownerCollider != null)
            {
                float distanceFromOwner = Vector3.Distance(weapon.transform.position, tracker.ownerCollider.transform.position);
                if (distanceFromOwner > 1f)
                {
                    Physics.IgnoreCollision(weaponCollider, tracker.ownerCollider, false);
                }
            }
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        if (!isMoving)
        {
            ChangeAnim("Idle");
        }
    }

    void OnDrawGizmosSelected()
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

    private IEnumerator HandleDeath()
    {
        isDead = true;
        isAttacking = false;
        isMoving = false;
        ChangeAnim("Dead");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && !isDead)
        {
            WeaponTracker weaponInfo = activeWeapons.Find(w => w.weapon == other.gameObject);
            if (weaponInfo == null)
            {
                Debug.Log($"{gameObject.name} was hit by weapon!");
                Destroy(other.gameObject);
                StartCoroutine(HandleDeath());
            }
        }
    }
}