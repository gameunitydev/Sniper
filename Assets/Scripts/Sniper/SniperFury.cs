using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperFury : SniperController
{


    public Text txtReload, txtZoom;

    [Header("Ammo")]
    //How much ammo before reload
    public int AmmoPerClip = 3;
    /// <summary>
    /// How long does reload last
    /// </summary>
    public float ReloadTime = 2;

    public int TimeFreezesAvailable = 4;


    //Current ammo level
    private int currentAmmo;
    private bool wasInScopeBeforeReloading = false;
    private float rTime = 0;

    //For bullet cam
    public bool canShootAdditionalBullet = false;
    private bool wasViewingScope = false;

    public Button TimeFreezeButton;
    private int currentTimeFreezesAvailable;


    [Header("Targetting")]
    public GameObject[] Targets;
    public Button SatelliteButton;
    private bool performedSatelliteScan = false;

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
            }
            //ScopeParent.transform.Find("ScopeBlurLeft").GetComponent<Renderer>().enabled = ViewingScope;
            //ScopeParent.transform.Find("ScopeBlurRight").GetComponent<Renderer>().enabled = ViewingScope;


        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentAmmo = AmmoPerClip;
        sldZoom.interactable = true;
        performedSatelliteScan = false;

        //Assign some random priorities to the targets
        foreach (GameObject target in Targets)
        {
            int priority = UnityEngine.Random.Range(1, Enum.GetNames(typeof(Enemy.Priority)).Length);
            if (target.GetComponent<Enemy>())
                target.GetComponent<Enemy>().EnemyPriority = (Enemy.Priority)priority;
        }

        TimeFreezeButton.interactable = true;

        currentTimeFreezesAvailable = TimeFreezesAvailable;
        SetTimeFreezeButtonText(currentTimeFreezesAvailable);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //Reset priorities
        foreach (GameObject target in Targets)
        {
            if (target.GetComponent<Enemy>())
                target.GetComponent<Enemy>().EnemyPriority = Enemy.Priority.None;
        }
    }


    /// <summary>
    /// Highlights the targets and disable ui button afterwords
    /// </summary>
    public void OnSatelliteScan()
    {
        if (performedSatelliteScan)
            return;

        SatelliteButton.interactable = false;
        foreach (GameObject target in Targets)
        {
            if (target.GetComponent<Enemy>())
            {
                target.GetComponent<Enemy>().IsTarget = true;
            }
        }
        performedSatelliteScan = true;

    }

    protected override void OnFingerDown(LeanFinger finger)
    {
        base.OnFingerDown(finger);
        if (canShootAdditionalBullet)
        {
            Instantiate<GameObject>(BulletPrefab, Camera.transform.position, Camera.transform.rotation);
            canShootAdditionalBullet = false;
        }
    }

    public override void OnFireBegin()
    {

        if (isReloading || isMiniReloading)
            return;

        base.OnFireBegin();
        currentAmmo--;

        if (ViewingScope)
        {
            FireFromUIScope();
        }
        else
        {
            FireCentered();
        }

        isMiniReloading = true;
        wasViewingScope = ViewingScope;
        StartCoroutine("FireDelay");

    }

    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(TimeDelayBetweenShots / 4);
        if (currentAmmo > 0)
        {
            ViewingScope = false;
            yield return new WaitForSeconds(TimeDelayBetweenShots);
            if (wasViewingScope)
                ViewingScope = true;
            isMiniReloading = false;
        }
        else
        {
            OnReload();
            isMiniReloading = false;
        }

    }

    /// <summary>
    /// Called when reloading takes place.
    /// </summary>
    public override void OnReload()
    {
        if (isReloading)
            return;

        wasInScopeBeforeReloading = ViewingScope;
        if (ViewingScope)
            ViewingScope = false;
        IsFiring = false;
        isReloading = true;
        sldZoom.interactable = false;

        StartCoroutine(ReloadDelay());
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(ReloadTime / 4);


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

    }

    public override void OnBulletCamEnd(Enemy sender)
    {
        InBulletCam = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime /= BulletCamTimeScale;

        canShootAdditionalBullet = true;
        StartCoroutine(BulletCamAdditionalBullets(sender));
    }


    private IEnumerator BulletCamAdditionalBullets(Enemy sender)
    {
        yield return new WaitForSeconds(2.0f);
        canShootAdditionalBullet = false;

        base.OnBulletCamEnd(sender);

        CurrentSniperPanel.SetActive(true);
        OnReload();
        ViewingScope = false;
        sldZoom.value = sldZoom.maxValue;

    }


    public override void OnThermal()
    {
        if (ViewingScope)
        {
            if (!thermalsActive)
            {
                SetThermals(true);
                StartCoroutine("ThermalDelay");
            }
            else
            {
                SetThermals(false);
                StopCoroutine("ThermalDelay");
            }

        }
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

    private void SetTimeFreezeButtonText(int pressesRemaining)
    {
        TimeFreezeButton.transform.GetChild(0).GetComponent<Text>().text = "Time Freeze (" + pressesRemaining + ")";
    }

    public override void OnTimeFreezeBegin()
    {
        if (currentTimeFreezesAvailable > 0)
        {
            base.OnTimeFreezeBegin();
            currentTimeFreezesAvailable--;
            SetTimeFreezeButtonText(currentTimeFreezesAvailable);
        }
    }

    public override void OnTimeFreezeEnd()
    {
        base.OnTimeFreezeEnd();
        if (currentTimeFreezesAvailable <= 0)
        {
            TimeFreezeButton.interactable = false;
        }

    }


    protected override void Update()
    {
        txtReload.text = currentAmmo + "\n" + AmmoPerClip;
        string zm = string.Format("{0:0.0}x", GetZoomFrom(1, 6));
        txtZoom.text = zm;


        base.Update();
        if (IsUserControllable)
        {
            float prevZoom = Zoom;


            ShowMessage("Fire", "Fire : " + Convert.ToInt32(isMiniReloading && !isReloading));

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
