using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamagable
{
    [Header("Barrel settings")]
    public float maximumHealth;
    public float currentHealth;

    public string reactOnTag;


    #region Unity Methods

    private void Start()
    {
        currentHealth = maximumHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == reactOnTag)
        {
            //  ReciveDamage(other.GetComponent<AS_Bullet>().Damage);
        }
    }
    #endregion

    #region IDamagable

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set { currentHealth = value; }
    }

    public float MaxHealth
    {
        get
        {
            return maximumHealth;
        }
        private set { maximumHealth = value; }
    }

    public void ReciveDamage(float damage)
    {
        currentHealth -= damage;


        // Play some damage effect
        ReciveDamageEffect();
        // Check for mission outcome
        RussiaMission.Instance.CheckIsMissionFinished();

        if (currentHealth <= 0)
        {
            currentHealth = 0;



            gameObject.SetActive(false);
        }
    }

    public void ReciveDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {

    }

    public void ReciveDamageEffect()
    {

    }


    #endregion
}
