using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sniper3DSilentAssassinFury : SniperController
{


    public Text txtReload, txtZoom;
    public Button btnReload;
    public Transform btnFire;

    /// <summary>
    /// The ui to use for the bullet button.
    /// </summary>
    public GameObject BulletOutlinePrefab;

    [Header("Ammo")]
    //How much ammo before reload
    public int AmmoPerClip = 3;
    /// <summary>
    /// How long does reload last
    /// </summary>
    public float ReloadTime = 2;


    //Current ammo level
    private int currentAmmo;
    private bool wasInScopeBeforeReloading = false;
    private float rTime = 0;

    //For bullet cam
    private bool canShootAdditionalBullet = false;

    private bool waitingForEndOfActionCam = false;

    public Transform EnemyTarget;



    public override bool ViewingScope
    {
        get
        {
            return base.ViewingScope;
        }

        set
        {

            base.ViewingScope = value;

            transform.GetChild(0).transform.Find("Gun").GetComponent<MeshRenderer>().enabled = !value;
            transform.GetChild(0).transform.Find("Gun").GetChild(0).GetComponent<MeshRenderer>().enabled = value;

            if (value)
            {
                Reticule.gameObject.SetActive(true);
                UpdateFireButtonGraphics();
            }
            //ScopeParent.transform.Find("ScopeBlurLeft").GetComponent<Renderer>().enabled = ViewingScope;
            //ScopeParent.transform.Find("ScopeBlurRight").GetComponent<Renderer>().enabled = ViewingScope;


        }
    }

    /// <summary>
    /// Draws available bullets as white
    /// </summary>
    private void UpdateFireButtonGraphics()
    {
        for (int i = 0; i < currentAmmo; i++)
        {
            btnFire.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        for (int i = currentAmmo; i < AmmoPerClip; i++)
        {
            btnFire.transform.GetChild(i).GetComponent<Image>().color = Color.black;

        }
    }


    void Start()
    {
        //Populate the button with bullet outlines - only do this once
        for (int i = 0; i < AmmoPerClip; i++)
        {
            GameObject g = Instantiate(BulletOutlinePrefab);
            g.GetComponent<Image>().color = Color.white;
            g.transform.SetParent(btnFire.transform, false);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        currentAmmo = AmmoPerClip;
        sldZoom.interactable = true;
        btnThermal.interactable = true;
        CurrentSniperPanel.transform.GetChild(3).gameObject.SetActive(true);
        btnReload.gameObject.SetActive(false); //Only show when not full on ammo

        if (EnemyTarget != null)
        {
            EnemyTarget.GetComponent<Renderer>().material.color = Color.blue;
        }

        //Make everything white
        foreach (Transform child in btnFire.transform)
        {
            child.GetComponent<Image>().color = Color.white;
        }

    }

    /*
    /// <summary>
    /// Fire from the center of the UI scope centered on the screen 
    /// </summary>
    /// <param name="destructive">Should the bullet do damage to the enemy?</param>
    /// <returns>True if raycast hit an enemy, false otherwise</returns>
    protected override bool FireFromUIScope(bool destructive = true)
    {
        return base.FireFromUIScope(destructive);
        Ray renderRay = ScopeCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));


        //Use the raycast to determine if we've hit an enemy
        RaycastHit enemyHit;
        if (Physics.Raycast(renderRay, out enemyHit))
        {
            if (enemyHit.transform.tag == "Enemy")
            {
                if (destructive)
                {
                    Enemy enemy = enemyHit.transform.GetComponent<Enemy>();
                    enemiesRemaining--;
                    if (EnemyTarget != null && enemy.transform.name == EnemyTarget.transform.name)
                    { 
                        FireFromUIScopeWithBullet();
                        OnBulletCamBegin();
                    }
                    else
                    {
                        enemy.OnBulletHit(BulletDamage);
                    }

                }

                return true;
            }

        }


        return false;
    }
    */


    protected override void OnFingerDown(LeanFinger finger)
    {
        base.OnFingerDown(finger);
        if (canShootAdditionalBullet)
        {
            FireFromUIScope();
            canShootAdditionalBullet = false;
        }
    }

    public override void OnFireBegin()
    {

        if (isReloading || isMiniReloading)
            return;

        base.OnFireBegin();
        currentAmmo--;
        UpdateFireButtonGraphics();

        //Regardless of scope mode, determine if we hit an enemy
        bool hitEnemy;
        if (ViewingScope)
            hitEnemy = FireFromUIScopeWithBullet();
        else
            hitEnemy = FireCenteredWithBullet();

        //If we did hit an enemy, hide the main ui panel
        if (hitEnemy)
        {
            if (ViewingScope)
                CurrentSniperPanel.SetActive(false);
            IsUserControllable = false;
            waitingForEndOfActionCam = true;

            return;
        }
        //Otherwise, perform typical reload checks
        PreReloadActions();

    }
    /// <summary>
    /// Called when the action camera vfx are completed. 
    /// Here, we reset the ui panel, as well as resume reload checks
    /// </summary>
    protected override void OnActionCamEnd()
    {
        base.OnActionCamEnd();
        if (waitingForEndOfActionCam)
        {
            if (ViewingScope)
                CurrentSniperPanel.SetActive(true);
            IsUserControllable = true;
            waitingForEndOfActionCam = false;
            PreReloadActions();
        }
    }
    /// <summary>
    /// Determine if we need to reload or mini-reload
    /// </summary>
    private void PreReloadActions()
    {

        if (currentAmmo <= 0)
        {
            OnReload();
        }
        else
        {
            isMiniReloading = true;
            sldZoom.interactable = false;
            StartCoroutine(FireDelay());
            //Show the reload button
            btnReload.gameObject.SetActive(true);
        }
    }

    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(2f);
        CurrentSniperPanel.gameObject.SetActive(true);
        isMiniReloading = false;
        sldZoom.interactable = true;
    }

    /// <summary>
    /// Called when reloading takes place.
    /// </summary>
    public override void OnReload()
    {
        if (isReloading)
            return;

        btnReload.gameObject.SetActive(false);


        wasInScopeBeforeReloading = ViewingScope;
        if (ViewingScope)
            ViewingScope = false;
        IsFiring = false;
        isReloading = true;
        sldZoom.interactable = false;

        //Don't allow swapping scopes while reloading
        //CurrentSniperPanel.transform.GetChild(2).transform.Find("ToggleScope").GetComponent<Button>().interactable = false;


        StartCoroutine(ReloadDelay());
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(ReloadTime);
        sldZoom.interactable = true;

        currentAmmo = AmmoPerClip;
        isReloading = false;
        if (wasInScopeBeforeReloading)
        {
            ViewingScope = true;
            //Extra delay for the transition
            yield return new WaitForSeconds(0.5f);
        }
        UpdateFireButtonGraphics();

    }

    public override void OnBulletCamEnd(Enemy sender)
    {
        base.OnBulletCamEnd(sender);

        ViewingScope = false;
        if (currentAmmo < AmmoPerClip)
            OnReload();
        StartCoroutine(BulletCamEndDelay());
    }

    private IEnumerator BulletCamEndDelay()
    {
        yield return new WaitForSeconds(.5f);
        CurrentSniperPanel.gameObject.SetActive(true);
    }


    private IEnumerator BulletCamAdditionalBullets(AS_Enemy sender)
    {
        yield return new WaitForSeconds(2.0f);
        canShootAdditionalBullet = false;

    }



    protected override void OnFinishedScopeTransition()
    {
        base.OnFinishedScopeTransition();
        if (!ViewingScope)
            ScopeRenderUI.SetActive(false);
    }

    protected override void OnFinishedScopeTransitionMiddle()
    {
        base.OnFinishedScopeTransitionMiddle();
        if (ViewingScope)
            ScopeRenderUI.SetActive(true);

    }




    protected override void Update()
    {
        txtReload.text = currentAmmo + "/" + AmmoPerClip;
        string zm = string.Format("{0:0}X", GetZoomFrom(1, 5));
        txtZoom.text = zm;
        base.Update();


        if (IsUserControllable)
        {
            float prevZoom = Zoom;


            ShowMessage("Fire", "Fire : " + Convert.ToInt32(IsFiring));
            ShowMessage("MiniReload", "Is Adjusting Gun: " + Convert.ToInt32(isMiniReloading));

            RotationControls(true, true);
            ZoomSlideControls();


            //Switch to scope view now
            if (!isReloading)
            {
                if (Zoom >= 1 && prevZoom < 1)
                {
                    ViewingScope = true;
                }
                if (Zoom < 1 && prevZoom >= 1)
                {
                    ViewingScope = false;
                }
            }
        }
    }
}
