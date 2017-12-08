using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperArenaPVPArmyShooter : SniperController
{

    /// <summary>
    /// Used to show radial zoom status
    /// </summary>
    public GameObject ScopeUICenter;

    public Text txtReload;

    public Text txtZoom;

    public Transform powerupsHolder;

    public Button btnToggleScope;

    [Header("Ammo")]
    //How much ammo before reload
    public int AmmoPerClip = 3;
    /// <summary>
    /// How long does reload last
    /// </summary>
    public float ReloadTime = 2;

    [Header("Powerups")]
    public float PowerupDuration = 5;
    public float HealthRegenSpeed = 5;
    public float SuperchargeBulletSpeedMultiplier = 2;
    private bool isDraggingZoom = false;

    public override bool ViewingScope
    {
        get
        {
            return base.ViewingScope;
        }

        set
        {


            if (isMiniReloading && value)
            {
                return;
            }

            base.ViewingScope = value;

            //Swap Root mesh to make scope nicer
            transform.GetChild(0).transform.Find("Gun").GetComponent<MeshRenderer>().enabled = !value;
            transform.GetChild(0).transform.Find("Gun").GetChild(0).GetComponent<MeshRenderer>().enabled = value;

            //3D text renders twice - this fixes that
            if (value)
                Camera.cullingMask &= ~(1 << LayerMask.NameToLayer("3DText")); //Turn off the culling layer
            else
                Camera.cullingMask |= 1 << LayerMask.NameToLayer("3DText"); //Turn on the culling layer



        }



    }


    public override PrimaryHand SniperPrimaryHand
    {
        get
        {
            return base.SniperPrimaryHand;
        }

        set
        {
            if (primaryHand == value)
                return;
            base.SniperPrimaryHand = value;

            //We'll also need to adjust the zoom slider stuff because it won't work seamlessly
            Vector3 scale = ScopeUICenter.transform.localScale;
            ScopeUICenter.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            //Unreverse the text
            Vector3 txtScale = txtZoom.transform.localScale;
            txtZoom.transform.localScale = new Vector3(-txtScale.x, txtScale.y, txtScale.z);
            //See update for additional change regarding zoom slider bar.
        }
    }

    private int currentAmmo;
    //Are we panning right now? (as opposed to zooming)
    private bool panning = false;
    //Can we switch between panning and zooming?
    private bool canSwitch = true;

    private bool isRechargingHealth = false;
    private bool isSupercharged = false;
    private bool isTracking = false;

    private bool wasInScopeBeforeReloading = false;


    protected override void OnEnable()
    {
        base.OnEnable();
        currentAmmo = AmmoPerClip;

        isRechargingHealth = false;
        isSupercharged = false;


        //Make sure everything is interactable
        foreach (Transform child in powerupsHolder)
            child.GetComponent<Button>().interactable = true;

        isRechargingHealth = false;

        LeanTouch.OnFingerUp += OnFingerUp;

        //Set appropriate slider values
        HealthSlider.maxValue = MaxHealth;

        btnToggleScope.interactable = true;

        //Reset zoom rotation
        ScopeUICenter.transform.localEulerAngles = new Vector3(0, 0, 0);

        //Initialize Health
        Health = MaxHealth / 2;


    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LeanTouch.OnFingerUp -= OnFingerUp;
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

    private void OnFingerUp(LeanFinger finger)
    {
        isDraggingZoom = false;
        StartCoroutine(Delay());
    }


    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(.2f);
        canSwitch = true;
    }

    public override void OnFireBegin()
    {
        if (isReloading || isMiniReloading)
            return;
        base.OnFireBegin();
        if (ViewingScope)
            FireFromUIScope();
        else
        {
            FireCentered();
        }

        currentAmmo--;
        isMiniReloading = true;
        StartCoroutine("FireDelay");

    }

    private IEnumerator FireDelay()
    {
        //Apply the recoil effect, wait for it to be over, then either reload or allow firing again.
        if (ViewingScope)
            Recoil(TimeDelayBetweenShots, InitialRecoilSpeed);
        yield return new WaitForSeconds(TimeDelayBetweenShots);
        isMiniReloading = false;
        if (currentAmmo <= 0)
        {
            OnReload();
        }
        else
        {
            btnToggleScope.interactable = true;
        }

    }

    /// <summary>
    /// Called when reloading takes place.
    /// </summary>
    public override void OnReload()
    {
        if (isReloading)
            return;

        btnToggleScope.interactable = false;

        if (ViewingScope)
            ViewingScope = false;
        isReloading = true;
        isMiniReloading = false;

        StartCoroutine(ReloadDelay());
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(ReloadTime);
        currentAmmo = AmmoPerClip;
        isReloading = false;
        btnToggleScope.interactable = true;

    }

    /// <summary>
    /// Recharge health over time
    /// </summary>
    public void OnHealthRecharge()
    {
        OnPowerupBegin(Powerup.HealthRecharge);
        powerupsHolder.GetChild(0).GetComponent<Button>().interactable = false;
        isRechargingHealth = true;
        StartCoroutine(PowerupDelay(Powerup.HealthRecharge));

    }

    /// <summary>
    /// This will make the bullet result in one-shot kills
    /// </summary>
    public void OnSupercharge()
    {
        OnPowerupBegin(Powerup.Supercharge);
        BulletDamage = 999; //To make sure one-hit kills occur
        powerupsHolder.GetChild(1).GetComponent<Button>().interactable = false;
        isSupercharged = true;
        StartCoroutine(PowerupDelay(Powerup.Supercharge));

    }

    /// <summary>
    /// Start tracking enemies
    /// </summary>
    public void OnEnemyTrack()
    {
        OnPowerupBegin(Powerup.Track);
        powerupsHolder.GetChild(2).GetComponent<Button>().interactable = false;
        isTracking = true;
        StartCoroutine(PowerupDelay(Powerup.Track));

    }

    /// <summary>
    /// Wait for current powerup to be over, then raise event
    /// </summary>
    private IEnumerator PowerupDelay(Powerup powerup)
    {
        yield return new WaitForSeconds(PowerupDuration);
        switch (powerup)
        {
            case Powerup.HealthRecharge:
                powerupsHolder.GetChild(0).GetComponent<Button>().interactable = true;
                isRechargingHealth = false;
                OnPowerupEnd(Powerup.HealthRecharge);
                break;
            case Powerup.Supercharge:
                powerupsHolder.GetChild(1).GetComponent<Button>().interactable = true;
                BulletDamage = initBulletDamage;
                isSupercharged = false;
                OnPowerupEnd(Powerup.Supercharge);
                break;
            case Powerup.Track:
                powerupsHolder.GetChild(2).GetComponent<Button>().interactable = true;
                OnPowerupEnd(Powerup.Track);
                isTracking = false;
                break;
            default:
                break;
        }



    }

    protected override void RotationControls(bool ignoreGUI = true, bool linear = false)
    {
        if (isZooming)
            return;

        //For smooth rotation
        Vector3 prevEuler = new Vector3(xRot, yRot, 0);

        List<LeanFinger> fingers = LeanTouch.GetFingers(true);

        float sens = RotationSensitivity;
        if (ViewingScope)
        {
            if (Zoom != 0)
            {
                sens = RotationSensitivity * InvZoom(Zoom, .25f);
            }
            else
                sens = RotationSensitivity;
        }




        //Change in finger position (sliding)
        Vector2 delta = LeanGesture.GetScreenDelta(fingers) * sens;

        //For the x rotation, we don't use the Camera's euler angles directly.
        //We initialize xRot to zero before this method (since we're using localEulerAngles), then we constanly subtract delta.y
        //Then we clamp that independent value, and apply to the localEulerAngles.
        //Inspired from joel_b on http://answers.unity3d.com/questions/18680/limiting-rotation-of-an-object.html

        if (panning)
        {
            xRot -= delta.y;
            yRot += delta.x;
        }


        if (RestrictXRotation)
            xRot = WrapAngle(Mathf.Clamp(xRot, MinXRotation, MaxXRotation));
        if (RestrictYRotation)
            yRot = WrapAngle(Mathf.Clamp(yRot, MinYRotation, MaxYRotation));


        //
        //Root.transform.rotation = Quaternion.Euler();

        if (ExtraSmooth)
        {
            Quaternion look = Quaternion.Euler(new Vector3(xRot, yRot, 0));
            if (RotationSmooth != -1)
                Root.rotation = Quaternion.Slerp(Root.rotation, look, RotationSmooth);
            else
                Root.rotation = Quaternion.Slerp(Root.rotation, look, RotationSensitivity);

        }
        else
        {
            if (RotationSmooth != -1)
                Root.transform.eulerAngles = Vector3.Lerp(prevEuler, new Vector3(xRot, yRot, 0), RotationSmooth);
            else
                Root.transform.eulerAngles = Vector3.Lerp(prevEuler, new Vector3(xRot, yRot, 0), RotationSensitivity);

        }




        ShowMessage("CameraAngle", "Camera Angle : " + CameraRotation.ToString());
    }

    /// <summary>
    /// Called when you first start dragging the zoom grip
    /// </summary>
    public void BeginHoverZoom()
    {
        isDraggingZoom = true;
    }
    public void EndHoverZoom()
    {
        isDraggingZoom = false;
    }

    protected override void Update()
    {

        base.Update();


        ShowMessage("CameraAngle", "Camera Angle : " + CameraRotation.ToString());
        ShowMessage("Scope", "Toggle Scope : " + Convert.ToInt32(ViewingScope));
        ShowMessage("Fire", "Fire : " + Convert.ToInt32(IsFiring));
        ShowMessage("Reloading", "Is Reloading: " + Convert.ToInt32(isReloading));
        ShowMessage("MiniReloading", "Is Reloading While In Scope: " + Convert.ToInt32(isMiniReloading));

        ShowMessage("Supercharged", "Supercharged : " + Convert.ToInt32(isSupercharged));

        txtReload.text = string.Format("{0:00}", currentAmmo);

        UpdateHealth();
        RotationControls();

        if (IsUserControllable)
        {

            bool prevPanning = panning;

            List<LeanFinger> fingers = LeanTouch.GetFingers(false);

            if (fingers.Count > 0)
            {
                if (canSwitch)
                {
                    Ray ray = fingers[0].GetRay();
                    RaycastHit hit = default(RaycastHit);
                    if (isDraggingZoom && ViewingScope)
                    {
                        panning = false;
                        canSwitch = false;
                    }
                    else
                    {
                        panning = true;
                    }
                }
                if (!panning)
                {
                    Vector2 delta = LeanGesture.GetScreenDelta(fingers) * ZoomSpeed * Time.deltaTime;
                    ScopeCamera.fieldOfView -= delta.y;
                    ScopeCamera.fieldOfView = Mathf.Clamp(ScopeCamera.fieldOfView, MinZoom, MaxZoom);


                    //For 60 degrees of rotation 
                    float zRot;
                    if (SniperPrimaryHand == PrimaryHand.RightHanded)
                        zRot = Zoom * .6f;
                    else
                        zRot = -Zoom * .6f;
                    //Also adjust the progress bar
                    Transform scopeCenter = ScopeUICenter.transform;
                    scopeCenter.localEulerAngles = new Vector3(scopeCenter.localEulerAngles.x, scopeCenter.localEulerAngles.y, zRot);
                }


            }

            //Powerups Update 
            if (isRechargingHealth)
            {
                Health += Time.deltaTime * HealthRegenSpeed;
                //No need to clamp - handled by UpdateHealth
            }


            string zm = string.Format("X{0:00}", GetZoomFrom(0, 4));
            txtZoom.text = zm;


        }
    }
}
