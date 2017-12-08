using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SniperXFeatJasonStatham : SniperController
{


    public Text txtReload;
    public Text txtZoom;
    public Button swapGunButton;
    public Button fireButton;
    public Button focusButton;

    [Header("Ammo")]
    //How much ammo before reload
    public int AmmoPerClip = 3;
    /// <summary>
    /// How long does reload last
    /// </summary>
    public float ReloadTime = 2;

    [Header("Focus")]
    /// <summary>
    /// How long focus powerup lasts
    /// </summary>
    public float FocusTime = 4;

    /// <summary>
    /// What to look at while focusing.
    /// </summary>
    public GameObject[] Targets = new GameObject[2];

    //Current ammo level
    private int currentAmmo;
    private bool wasInScopeBeforeReloading = false;
    private float rTime = 0;
    private bool isSwappingGuns = false;
    private bool isFocusing = false;
    private bool isFocusingEnding = false;
    private float fovBeforeFocus = 0;
    private float zoomTime = 0;
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
                    focusButton.interactable = true;
                }
                else
                {
                    if (isFocusing)
                        OnFocusEnd();
                    isFocusing = false;
                    focusButton.interactable = false;
                }
                //ScopeParent.transform.Find("ScopeBlurLeft").GetComponent<Renderer>().enabled = ViewingScope;
                //ScopeParent.transform.Find("ScopeBlurRight").GetComponent<Renderer>().enabled = ViewingScope;
            }


        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentAmmo = AmmoPerClip;
        isSwappingGuns = false;
        sldZoom.interactable = true;
        fireButton.interactable = true;
        isFocusing = false;
        isFocusingEnding = false;
        fovBeforeFocus = 0;
        zoomTime = 0;
        focusButton.interactable = false;
        //This is a hacky way to a fix a unity bug with this button. 
        EventSystem.current.SetSelectedGameObject(fireButton.gameObject);
        EventSystem.current.SetSelectedGameObject(null);


        //Tell the targets that they are targets
        foreach (GameObject g in Targets)
        {
            if (g.GetComponent<Enemy>())
                g.GetComponent<Enemy>().IsTarget = true;
        }



    }


    public override void OnFireBegin()
    {
        if (isReloading || !fireButton.interactable)
            return;
        base.OnFireBegin();


        if (enemiesRemaining > 1)
        {
            if (ViewingScope)
            {
                FireFromUIScope();
            }
            else
            {
                FireCentered();
                StartCoroutine(BulletFireDelay());
            }

            rTime = 0; //For reticule animation
            currentAmmo--;
            if (currentAmmo <= 0)
            {
                OnReload();
            }
        }
        //Bullet cam on the last enemy
        else
        {
            if (ViewingScope)
            {
                if (FireFromUIScope(false))
                {
                    FireFromUIScopeWithBullet();
                    OnBulletCamBegin();
                }
            }
            else
            {
                if (FireCentered())
                {
                    OnBulletCamBegin();
                }
            }
        }
    }


    protected IEnumerator BulletFireDelay()
    {
        yield return new WaitForSeconds(0.5f);

    }



    /// <summary>
    /// Called when reloading takes place.
    /// </summary>
    public override void OnReload()
    {
        if (isReloading)
            return;


        wasInScopeBeforeReloading = ViewingScope;
        //Prevent further firing and zooming
        sldZoom.interactable = false;

        fireButton.interactable = false;
        StartCoroutine(ReloadDelay());
    }

    private IEnumerator ReloadDelay()
    {
        float yieldTime = isFocusing ? 1f : .5f;
        if (ViewingScope)
            Recoil(yieldTime, InitialRecoilSpeed);
        yield return new WaitForSeconds(yieldTime); //See the shot first

        //This fixes the bullet cam transition.
        CurrentSniperPanel.SetActive(true);

        if (ViewingScope)
        {
            ViewingScope = false;
            isFocusingEnding = true;
        }
        IsFiring = false;
        isReloading = true;

        yield return new WaitForSeconds(ReloadTime);


        isFocusingEnding = false;
        currentAmmo = AmmoPerClip;
        isReloading = false;
        if (wasInScopeBeforeReloading)
        {
            ViewingScope = true;
            //Extra delay for the transition
            yield return new WaitForSeconds(0.5f);
        }
        fireButton.interactable = true;
        sldZoom.interactable = true;

        //This is a hacky way to a fix a unity bug with this button. 
        EventSystem.current.SetSelectedGameObject(fireButton.gameObject);
        EventSystem.current.SetSelectedGameObject(null);
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


    public void OnSwapGun()
    {
        isSwappingGuns = true;
        swapGunButton.interactable = false;
        StartCoroutine(SwapGunDelay());
    }

    private IEnumerator SwapGunDelay()
    {
        yield return new WaitForSeconds(0.5f);
        swapGunButton.interactable = true;
        isSwappingGuns = false;

    }


    public void OnFocus()
    {
        if (isFocusing)
            return;

        //While no one does anything with this event, call it just in case someone does later.
        OnPowerupBegin(Powerup.Focus);
        //Get a midpoint between the objects
        //Use the nullable type so that we can tell if we ever added something to sum

        isFocusing = true;
        fovBeforeFocus = ScopeCamera.fieldOfView;
        zoomTime = 0;
        OnTimeFreezeBegin();

        StartCoroutine(FocusDelay());

        /*

        Vector3 sum = Vector3.zero;
        int targets = 0;
        foreach (GameObject g in Targets)
            if (g.GetComponent<Enemy>() && g.GetComponent<Enemy>().Alive)
            {
                sum += g.transform.position;
                targets++;
            }

        if (sum != null)
        {
            //Convert to non-nullable type
            Vector3 mid = sum / targets;

            //Look at that point (Convert to euler angles so we can set xRot and yRot to Slerp
            Vector3 look;
            if (!ViewingScope)
                look = (mid - Gun.transform.position).normalized;
            else
                look = (mid - ScopeCamera.transform.position).normalized;
            Vector3 targetRot = Quaternion.LookRotation(look).eulerAngles;
            xRot = targetRot.x;
            yRot = targetRot.y;
        }*/



    }

    private IEnumerator FocusDelay()
    {
        yield return new WaitForSeconds(FocusTime);
        OnFocusEnd();
        isFocusing = false;
        isFocusingEnding = true;
        yield return new WaitForSeconds(.5f);
        isFocusingEnding = false;
    }


    private void OnFocusEnd()
    {
        OnPowerupEnd(Powerup.Focus);
    }

    public override void OnBulletCamEnd(Enemy sender)
    {
        base.OnBulletCamEnd(sender);
        rTime = 0; //For reticule animation
        currentAmmo--;
        if (currentAmmo <= 0)
        {
            OnReload();
        }
    }

    protected override void Update()
    {

        txtReload.text = currentAmmo + "/" + AmmoPerClip;
        if (!isFocusing && !isFocusingEnding)
        {
            string zm;
            if (Zoom > 0)
                zm = string.Format("{0:0.00}X", GetZoomFrom(2, 9));
            else
                zm = "ZOOM";
            txtZoom.text = zm;
        }


        ShowMessage("Focus", "Focus : " + Convert.ToInt32(InTimeFreezeMode));
        ShowMessage("SwapGun", "Swapping Guns : " + Convert.ToInt32(isSwappingGuns));

        base.Update();
        if (IsUserControllable)
        {
            float prevZoom = Zoom;


            RotationControls(true, true);


            if (isFocusing || isFocusingEnding)
            {
                zoomTime += Time.deltaTime / 10.0f;
                float targZoom = 1;

                if (!isFocusingEnding)
                    ScopeCamera.fieldOfView = Mathf.Lerp(sldZoom.value, targZoom, zoomTime);
                else
                    ScopeCamera.fieldOfView = Mathf.Lerp(ScopeCamera.fieldOfView, sldZoom.value, .1f);

                if (sldZoom.value == MaxZoom)
                {
                    ViewingScope = false;
                }

            }
            else
            {
                ZoomSlideControls();

            }


            //Switch to scope view now
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
