using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contract Killer Sniper derivation of SniperController
/// </summary>
public class ContractKillerSniper : SniperController
{


    public Text txtReload;
    public GameObject ScopeUICenter;
    public Text txtZoom;
    public Button toggleScopeButton;
    public GameObject zoomHighlight;
    [Header("Ammo")]
    //How much ammo before reload
    public int AmmoPerClip = 3;
    /// <summary>
    /// How long does reload last
    /// </summary>
    public float ReloadTime = 2;


    public float ReticuleOscillationSpeed = 2;

    public float DuckHeightOffset = 2;


    //Current ammo level
    private int currentAmmo;
    private bool wasInScopeBeforeReloading = false;

    private bool animatingReticule = false;
    private float rTime = 0;



    public override bool ViewingScope
    {
        get
        {
            return base.ViewingScope;
        }

        set
        {
            if (!isReloading)
            {
                base.ViewingScope = value;

                transform.GetChild(0).transform.Find("Gun").GetComponent<MeshRenderer>().enabled = !value;
                transform.GetChild(0).transform.Find("Gun").GetChild(0).GetComponent<MeshRenderer>().enabled = value;

                if (value)
                {
                    Reticule.gameObject.SetActive(true);


                    IsDucking = false;
                }
                //ScopeParent.transform.Find("ScopeBlurLeft").GetComponent<Renderer>().enabled = ViewingScope;
                //ScopeParent.transform.Find("ScopeBlurRight").GetComponent<Renderer>().enabled = ViewingScope;
            }


        }
    }
    private bool isDucking = false;
    private bool canSwitch = false;
    private bool isDraggingZoom = false;
    private bool panning = false;


    public bool IsDucking
    {
        get
        {
            return isDucking;
        }
        set
        {
            isDucking = value;

            SetCrosshair(!value);

            if (value)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - DuckHeightOffset, transform.position.z);

            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + DuckHeightOffset, transform.position.z);

            }
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

            //Edit zoom highlight rotation
            if (primaryHand == PrimaryHand.LeftHanded)
                zoomHighlight.transform.localEulerAngles = new Vector3(0, 180, 0);
            else
                zoomHighlight.transform.localEulerAngles = new Vector3(0, 0, 0);

        }
    }



    /// <summary>
    /// Set the crosshair's display mode to firstValue, and the second to the opposite of that
    /// </summary>
    /// <param name="firstValue"></param>
    private void SetCrosshair(bool firstValue)
    {
        CurrentSniperPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = firstValue;
        CurrentSniperPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().enabled = firstValue;

        CurrentSniperPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = !firstValue;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentAmmo = AmmoPerClip;

        toggleScopeButton.interactable = true;

        Transform left = ScopeParent.transform.Find("ScopeBlurLeft");
        Transform right = ScopeParent.transform.Find("ScopeBlurRight");

        isReloading = false;
        isMiniReloading = false;
        //Set appropriate slider values
        HealthSlider.maxValue = MaxHealth;

        isDucking = false;
        canSwitch = false;
        isDraggingZoom = false;
        panning = false;

        LeanTouch.OnFingerUp += OnFingerUp;

        //Reset zoom rotation
        ScopeUICenter.transform.localEulerAngles = new Vector3(0, 0, 0);

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LeanTouch.OnFingerUp -= OnFingerUp;

    }


    /*
    protected override bool FireFromScope()
    {
        bool hitEnemy = false;

        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(ScopeCamera.transform.position, ScopeCamera.transform.forward, out hit))
        {
            if (hit.transform.tag == "Enemy")
            {
                if (hit.transform.GetComponent<Enemy>().Alive)
                    hitEnemy = true;
            }
        }

        Vector3 spawnLoc = ScopeCamera.ScreenToWorldPoint(new Vector3(Reticule.transform.position.x, Reticule.transform.position.y, 0));

        //Offset a little to center it
        spawnLoc += Root.right / 3.1f;
        
        //Offset to spawn from below
        //spawnLoc += -Root.up / 2f;

        GameObject bullet = Instantiate<GameObject>(BulletPrefab, spawnLoc, ScopeCamera.transform.rotation);
        bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x - 2 , bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z);


        if (hitEnemy)
        {
            bulletCamTarget = bullet.transform;
            bulletCamTarget.GetComponent<Bullet>().IsTheSlowMoBullet = true; //Make sure it doesn't get destroyed after lifetime
        }


        return hitEnemy;
    }*/

    public override void OnFireBegin()
    {
        if (isReloading || isMiniReloading)
            return;

        base.OnFireBegin();
        if (ViewingScope)
        {
            FireFromUIScope();

            StartCoroutine(BulletFireDelay());
            isMiniReloading = true;
        }
        else
        {
            if (IsDucking)
                ViewingScope = true;
            else
            {
                FireCentered();
                animatingReticule = true;
                StartCoroutine(BulletFireDelay());
                isMiniReloading = true;

            }
        }
        rTime = 0; //For reticule animation
        currentAmmo--;
        if (currentAmmo <= 0)
        {
            OnReload();
        }
    }


    public override void OnFireEnd()
    {
        base.OnFireEnd();

    }

    protected IEnumerator BulletFireDelay()
    {
        yield return new WaitForSeconds(TimeBetweenShots);
        isMiniReloading = false;
        //Autofire
        if (IsFiring)
            OnFireBegin();
        if (isDucking && !IsFiring)
            SetCrosshair(false);
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
        isMiniReloading = false;
        //Don't allow swapping scopes while reloading
        toggleScopeButton.interactable = false;


        StartCoroutine(ReloadDelay());
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(ReloadTime);
        currentAmmo = AmmoPerClip;
        isReloading = false;
        if (wasInScopeBeforeReloading)
        {
            ViewingScope = true;
            //Extra delay for the transition
            yield return new WaitForSeconds(0.5f);
            //Allow swapping of scopes again
            toggleScopeButton.interactable = true;
        }
        else
        {
            toggleScopeButton.interactable = true;
        }

    }



    protected override void RotationControls(bool ignoreGUI = true, bool linear = false)
    {
        //For smooth rotation
        Vector3 prevEuler = new Vector3(xRot, yRot, 0);

        List<LeanFinger> fingers = LeanTouch.GetFingers(ignoreGUI);

        Vector3[] corners = new Vector3[4];
        ScopeUICenter.transform.GetChild(0).GetComponent<RectTransform>().GetWorldCorners(corners);
        Vector3 pivot = corners[0];
        for (int i = 0; i < fingers.Count; i++)
        {
            if (fingers[i].LastScreenPosition.x > corners[0].x)
            {
                fingers.RemoveAt(i);
                i--;
            }
        }



        //Optional linear mode
        float sens = RotationSensitivity;
        if (linear && ViewingScope)
        {
            if (Zoom != 0)
            {
                sens = RotationSensitivity * InvZoom(Zoom, MinimumRotationModifier);
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


        xRot -= delta.y;
        yRot += delta.x;


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

    public void OnDuckToggle()
    {
        IsDucking = !IsDucking;
        if (ViewingScope)
            ViewingScope = false;

    }

    /// <summary>
    /// Called when you first start dragging the zoom grip
    /// </summary>
    public void BeginHoverZoom()
    {
        //Get all the fingers down, the one that is zooming must be the last one
        isDraggingZoom = true;
    }
    public void EndHoverZoom()
    {
        isDraggingZoom = false;
    }

    private void OnFingerUp(LeanFinger finger)
    {
    }


    protected override void Update()
    {
        base.Update();
        ShowMessage("Fire", "Fire : " + Convert.ToInt32(IsFiring));
        ShowMessage("Scope", "Toggle Scope : " + Convert.ToInt32(ViewingScope));
        ShowMessage("Reloading", "Is Reloading: " + Convert.ToInt32(isReloading));
        ShowMessage("MiniReloading", "Is Adjusting Root: " + Convert.ToInt32(isMiniReloading));

        ShowMessage("Ducking", "Is Ducking: " + Convert.ToInt32(isDucking));

        txtReload.text = "Ammo: " + currentAmmo + "/" + AmmoPerClip;

        UpdateHealth();


        string zm = string.Format("X{0:0}", GetZoomFrom(6, 20));
        txtZoom.text = zm;

        if (IsUserControllable)
        {
            RotationControls(false);

            //Zoom Controls
            List<LeanFinger> fingers = LeanTouch.GetFingers(false);

            Vector3[] corners = new Vector3[4];
            ScopeUICenter.transform.GetChild(0).GetComponent<RectTransform>().GetWorldCorners(corners);
            Vector3 pivot = corners[0];
            for (int i = 0; i < fingers.Count; i++)
            {
                if (fingers[i].LastScreenPosition.x <= corners[0].x)
                {
                    fingers.RemoveAt(i);
                    i--;
                }
            }


            if (fingers.Count > 0)
            {
                if (isDraggingZoom)
                {
                    Vector2 delta = LeanGesture.GetScreenDelta(fingers) * ZoomSpeed * Time.deltaTime;
                    ScopeCamera.fieldOfView += delta.y;
                    ScopeCamera.fieldOfView = Mathf.Clamp(ScopeCamera.fieldOfView, MinZoom, MaxZoom);


                    //For 60 degrees of rotation 
                    float zRot;
                    if (SniperPrimaryHand == PrimaryHand.RightHanded)
                        zRot = -Zoom * .6f;
                    else
                        zRot = Zoom * .6f;
                    //Also adjust the progress bar
                    Transform scopeCenter = ScopeUICenter.transform;
                    scopeCenter.localEulerAngles = new Vector3(scopeCenter.localEulerAngles.x, scopeCenter.localEulerAngles.y, zRot);
                }
            }

            if (animatingReticule)
            {
                rTime += Time.deltaTime * ReticuleOscillationSpeed;

                float osc = Mathf.PingPong(rTime, .25f) + 1;
                CurrentSniperPanel.transform.GetChild(0).transform.localScale = new Vector3(osc, osc, osc);

                if (rTime >= .5f)
                {
                    animatingReticule = false;
                    rTime = 0;
                }
            }

        }
    }
}
