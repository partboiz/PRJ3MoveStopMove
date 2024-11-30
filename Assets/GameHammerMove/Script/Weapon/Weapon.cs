using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*public GameObject owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && other.gameObject != owner)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }*/
    /*public string ownerId;
    public bool isPlayerWeapon;*/



    private Transform shooter;

    public void SetShooter(Transform shooterTransform)
    {
        shooter = shooterTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != shooter && other.CompareTag("Character"))
        {
            Characterremake character = other.GetComponent<Characterremake>();
            if (character != null)
            {
                character.OnHit();
                Characterremake shooterCharacter = shooter.GetComponent<Characterremake>();
                if (shooterCharacter != null)
                {
                    shooterCharacter.IncrementDefeatCountAndCheck();
                }
            }
            Destroy(gameObject);
        }
    }
}


