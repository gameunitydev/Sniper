using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    public enum Priority
    {
        None,
        Low,
        High
    }


    /// <summary>
    /// Speed of the object
    /// </summary>
    public float Speed = 100;

    public float TimeBetweenVelChange = 3.0f;

    /// <summary>
    /// Health of this enemy.
    /// </summary>
    public float Health = 1;

    /// <summary>
    /// This is what can be shown and attached to the enemy
    /// </summary>
    public GameObject HUDGraphic;

    public float InsetToBeConsideredInPlayerView = .3f;

    /// <summary>
    /// Velocity of this object
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// Reference to the CharacterController
    /// </summary>
    private CharacterController controller;

    /// <summary>
    /// Reference to the player
    /// </summary>
    private SniperController sniper;

    //Used for timed velocity switching
    private float time = 0;

    private System.Random rand;
    private float initSpeed = 0;
    private float initHealth = 0;
    private Vector3 startPos;


    public Color StartColor = Color.red;

    private bool alive = true;

    private const string STANDARD_SHADER = "Standard";
    private const string XRAY_SHADER = "Custom/StandardOccluded";

    private string initHudText = "";

    /// <summary>
    /// How important that this target is killed
    /// </summary>
    public Priority EnemyPriority;

    /// <summary>
    /// Is the enemy alive?
    /// </summary>
    public bool Alive
    {
        get
        {
            return alive;
        }
        set
        {
            alive = value;

            if (!alive)
            {
                GetComponent<Renderer>().enabled = false;
                HUDGraphic.GetComponent<Renderer>().enabled = false;
                velocity = Vector3.zero;

            }
            else
            {
                GetComponent<Renderer>().enabled = true;
                velocity = Vector3.zero;
            }
        }
    }


    private bool isTarget = false;
    /// <summary>
    /// Is this enemy the target?
    /// </summary>
    public bool IsTarget
    {
        get
        {
            return isTarget;
        }
        set
        {
            isTarget = value;
            if (value)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = StartColor;
            }
        }
    }

    void Start()
    {
        initSpeed = Speed;

    }

    // Use this for initialization
    void OnEnable()
    {
        Alive = true;

        controller = GetComponent<CharacterController>();
        //Create a random object with the instance id as the seed to avoid duplicate random objects
        rand = new System.Random(gameObject.GetInstanceID());
        velocity = GetRandomUnitVector();
        sniper = GameObject.FindWithTag("Player").GetComponent<SniperController>();
        if (startPos == Vector3.zero)
            startPos = transform.position;
        else
            transform.position = startPos;

        if (initHealth == 0)
        {
            initHealth = Health;
        }
        else
        {
            Health = initHealth;
        }

        initHudText = HUDGraphic.GetComponent<TextMesh>().text;

        //Hide the exclamation point
        HUDGraphic.GetComponent<Renderer>().enabled = false;

        //subscribe to events
        sniper.TimeFreeze += HandleTimeFreeze;
        sniper.BulletCamEvent += HandleBulletCam;
        sniper.PowerupBegin += HandlePowerupBegin;
        sniper.PowerupEnd += HandlePowerupEnd;
        sniper.InstinctBegin += HandleInstinctBegin;
        sniper.InstinctEnd += HandleInstinctEnd;

        StartCoroutine(EnableDelay());

    }

    //Instinct refers to see-through-walls stuff
    private void HandleInstinctBegin()
    {
        GetComponent<Renderer>().material.shader = Shader.Find(XRAY_SHADER);
    }
    private void HandleInstinctEnd()
    {
        GetComponent<Renderer>().material.shader = Shader.Find(STANDARD_SHADER);
    }


    IEnumerator EnableDelay()
    {
        yield return new WaitForSeconds(.1f);
        Speed = initSpeed;
    }

    void OnDisable()
    {
        //Unsubscribe from events
        sniper.TimeFreeze -= HandleTimeFreeze;
        sniper.BulletCamEvent -= HandleBulletCam;
        sniper.PowerupBegin -= HandlePowerupBegin;
        sniper.PowerupEnd -= HandlePowerupEnd;

        //Reset color
        GetComponent<Renderer>().material.color = StartColor;
        ResetHUDGraphic();

        if (IsTarget)
            IsTarget = false;
    }


    /// <summary>
    /// Change the health to a new value, such that it will default to this when re-enabled.
    /// </summary>
    /// <param name="value">New health value</param>
    public void ResetHealth(int value)
    {
        Health = value;
        initHealth = value;
    }

    // Update is called once per frame
    void Update()
    {

        if (!Alive)
            return;

        time += Time.deltaTime;

        if (time > TimeBetweenVelChange)
        {
            velocity = GetRandomUnitVector();
            time = 0;
        }

        controller.SimpleMove(velocity * Speed);


        Camera cam = sniper.ActiveCamera;
        Vector3 screenPt = cam.WorldToViewportPoint(transform.position);
        float inset = InsetToBeConsideredInPlayerView;
        if (screenPt.x > inset && screenPt.x < (1 - inset) && screenPt.y > inset && screenPt.y < (1 - inset) && screenPt.z > 0)
        {
            CurrentlyInPlayerView();
        }
        else
        {
            CurrentlyOutOfPlayerView();
        }

    }

    private void CurrentlyInPlayerView()
    {
        if (sniper is SniperFury)
        {
            if (sniper.ViewingScope && IsTarget)
            {
                DisplayPriority();
            }
        }
    }

    private void CurrentlyOutOfPlayerView()
    {
        if (sniper is SniperFury)
        {
            ResetHUDGraphic();
        }
    }

    /// <summary>
    /// Event handler for TimeFreeze
    /// </summary>
    private void HandleTimeFreeze(bool value)
    {
        if (!Alive)
            return;

        if (value)
        {
            Speed /= sniper.TimeFreezeModifier;
        }
        else
        {
            Speed *= sniper.TimeFreezeModifier;
        }
    }

    /// <summary>
    /// Event handler for bullet cam
    /// </summary>
    private void HandleBulletCam(bool value)
    {
        if (!Alive)
            return;
        if (value)
        {
            Speed = 0;
        }
        else
        {
            Speed = initSpeed;
        }
    }

    private void HandlePowerupBegin(Powerup power)
    {
        if (!Alive)
            return;
        switch (power)
        {
            case Powerup.HealthRecharge:
                break;
            case Powerup.Supercharge:
                break;
            case Powerup.Track:
                //Show the exclamation point
                HUDGraphic.GetComponent<Renderer>().enabled = true;
                break;
            case Powerup.Thermal:
                GetComponent<Renderer>().material.color = new Color32(0xFF, 0x99, 0x00, 0xFF);
                break;
            default:
                break;
        }
    }
    private void HandlePowerupEnd(Powerup power)
    {
        if (!Alive)
            return;
        switch (power)
        {
            case Powerup.HealthRecharge:
                break;
            case Powerup.Supercharge:
                break;
            case Powerup.Track:
                //Hide the exclamation point
                HUDGraphic.GetComponent<Renderer>().enabled = false;
                break;
            case Powerup.Thermal:
                GetComponent<Renderer>().material.color = StartColor;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Returns a new, random vector3 of length 1
    /// </summary>
    private Vector3 GetRandomUnitVector()
    {
        int mult = rand.Next(2) == 1 ? 1 : -1;

        double rx = rand.NextDouble();
        double ry = rand.NextDouble();
        double rz = rand.NextDouble();

        Vector3 r = new Vector3((float)rx, (float)ry, (float)rz) * mult;

        r.Normalize();
        return r;


    }

    /// <summary>
    /// Call when the bullet hits this enemy
    /// </summary>
    public void OnBulletHit(float damage)
    {
        if (!Alive)
            return;

        //Damage the enemy
        Health -= damage;

        if (Health > 0)
        {
            StartCoroutine(Hurt());
            //Don't continue unless enemy is dead
            return;
        }

        //If I just got hit, and bullet cam is active, it must be time to turn it off
        if (sniper.InBulletCam)
        {
            sniper.OnBulletCamEnd(this);
        }
        else
        {
            if (sniper is SniperFury)
            {
                SniperFury sf = (SniperFury)sniper;
                if (sf.canShootAdditionalBullet)
                    return;
            }
            Alive = false;
        }
    }

    /// <summary>
    /// Display the priority on the HUDGraphic. 
    /// </summary>
    public void DisplayPriority()
    {
        HUDGraphic.GetComponent<TextMesh>().text = EnemyPriority.ToString();
        HUDGraphic.GetComponent<MeshRenderer>().enabled = true;
    }
    public void ResetHUDGraphic()
    {
        HUDGraphic.GetComponent<TextMesh>().text = initHudText;
        HUDGraphic.GetComponent<MeshRenderer>().enabled = false;

    }

    /// <summary>
    /// Perform a flash effect when the enemy gets shot
    /// </summary>
    private IEnumerator Hurt()
    {
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
            yield return new WaitForSeconds(.1f);
            GetComponent<Renderer>().material.color = StartColor;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Alive)
            return;

        //This is only called when we're NOT using Advanced Sniper bullet.
        if (other.tag == "Bullet")
        {
            OnBulletHit(other.GetComponent<Bullet>().Strength);
        }
    }
}
