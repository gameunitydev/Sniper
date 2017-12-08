using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityStandardAssets.ImageEffects;
public class LMGController : SniperController
{

    [Header("LMG Settings")]
    public GameObject Barrel;
    //Used for selective blurring 
    public Renderer GunRenderPlane;
    public float MaxBarrelRollSpeed = 5;

    /// <summary>
    /// How long the gun will be preparing before we can fire.
    /// </summary>
    public float GunChargeTime = 1;




    private float timeFireBegan;
    private float timeFireEnded;
    private float timeGunPrepBegan;
    //Last roll value (value from 0 to 1 times MaxBarrelRollSpeed)
    private float lastRollPercentage;
    private float startingRollPercentage = 0;
    private bool isGunPreparing = false;
    private bool isGunWindingDown = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        isGunPreparing = false;
    }

    public override void OnFireBegin()
    {
        timeGunPrepBegan = Time.time;
        isGunWindingDown = false;
        isGunPreparing = true;
        startingRollPercentage = lastRollPercentage;
        StartCoroutine(FirePrepareIE());
    }

    private IEnumerator FirePrepareIE()
    {
        //We don't need to wait the entire time if we left off in the middle of warm up or warm down
        float delay = (1 - startingRollPercentage) * GunChargeTime;
        yield return new WaitForSeconds(delay);
        if (isGunPreparing)
        {
            FireCenteredWithBullet();
            timeFireBegan = Time.time;
            isGunPreparing = false;
            base.OnFireBegin(); //Will set IsFiring to true
        }

    }

    public override void OnFireEnd()
    {
        base.OnFireEnd();
        timeFireEnded = Time.time;
        isGunPreparing = false;
        isGunWindingDown = true;
        startingRollPercentage = lastRollPercentage;
        StartCoroutine(FireWindDownIE());
    }

    private IEnumerator FireWindDownIE()
    {
        //We don't need to wait the entire time if we left off in the middle of warm up or warm down
        float delay = (startingRollPercentage) * GunChargeTime; //it's not 1 - here because this is how much we have left to go back down
        yield return new WaitForSeconds(delay);
        if (isGunWindingDown)
            isGunWindingDown = false;
    }


    protected override void OnFinishedScopeTransition()
    {
        base.OnFinishedScopeTransition();
        if (Camera.GetComponent<Blur>())
        {
            Camera.GetComponent<Blur>().enabled = ViewingScope;
        }
        if (GunRenderPlane)
        {
            GunRenderPlane.enabled = ViewingScope;
        }
    }

    protected override void Update()
    {
        base.Update();
        RotationControls();

        //Autofire
        if (IsFiring)
        {
            Barrel.transform.Rotate(new Vector3(0, 0, MaxBarrelRollSpeed));
            if (Time.time - timeFireBegan > TimeBetweenShots)
            {
                FireBulletFromGunMuzzle();
                timeFireBegan = Time.time;
            }
            lastRollPercentage = 1; //100 percent of MaxBarrelRollSpeed
        }
        if (isGunPreparing)
        {
            float percentage = startingRollPercentage + ((Time.time - timeGunPrepBegan) / GunChargeTime);
            float angle = MaxBarrelRollSpeed * percentage;
            //Rotate barrel slowly then build up speed
            Barrel.transform.Rotate(new Vector3(0, 0, angle));
            lastRollPercentage = percentage;
        }

        if (isGunWindingDown)
        {
            float startPerc = 1 - startingRollPercentage; //b/c we're going backwards
            float percentage = startPerc - (1 - ((Time.time - timeFireEnded) / GunChargeTime));
            float angle = MaxBarrelRollSpeed * percentage;
            //Rotate barrel quickly then slowly stop
            Barrel.transform.Rotate(new Vector3(0, 0, angle));
            lastRollPercentage = percentage;
        }

    }

}
