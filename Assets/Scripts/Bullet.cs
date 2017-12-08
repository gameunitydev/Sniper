using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the bullet
/// </summary>
public class Bullet : MonoBehaviour
{


    /// <summary>
    /// How fast should the bullet travel?
    /// </summary>
    public float Speed;

    /// <summary>
    /// How long till this bullet gets destroyed?
    /// </summary>
    public float LifeTime = 5;

    /// <summary>
    /// Is this the bullet in slow motion?
    /// </summary>
    public bool IsTheSlowMoBullet;

    /// <summary>
    /// Strength of this bullet. (How much damage it does to enemies)
    /// </summary>
    public float Strength = 2;


    /// <summary>
    /// The movement vector of this bullet.
    /// </summary>
    private Vector3 movement;

    // Use this for initialization
    void OnEnable()
    {

        IsTheSlowMoBullet = false;
        //Get the movement vector
        movement = new Vector3(0, 0, Speed);
        //Transform to global space
        movement = transform.TransformVector(movement);
        //Add the force continuously
        GetComponent<Rigidbody>().AddForce(movement);
        //Destroy after LifeTime seconds
        StartCoroutine(DelayedDestroy());

    }

    /// <summary>
    /// Scale the force vector this bullet is travelling with
    /// </summary>
    public void ScaleForce(float scalar)
    {
        //we use scalar-1 since we've already added movement once
        GetComponent<Rigidbody>().AddForce(movement * (scalar - 1));
    }

    /// <summary>
    /// Destroy the bullet after LifeTime seconds
    /// </summary>
    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSecondsRealtime(LifeTime);
        if (!IsTheSlowMoBullet)
            Destroy(this.gameObject);
    }
}
