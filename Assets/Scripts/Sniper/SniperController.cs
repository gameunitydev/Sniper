using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Possible powerup types
/// </summary>
public enum Powerup
{
    HealthRecharge,
    Supercharge,
    Track,
    Thermal,
    Focus
}

/// <summary>
/// Control modes based on primary hand
/// </summary>
public enum PrimaryHand
{
    LeftHanded,
    RightHanded
}


/// <summary>
/// Main Sniper Controller
/// </summary>
public class SniperController : MonoBehaviour
{


    #region Fields


    [Header("General")]

    ///How should this be displayed in the ui?
    public string DisplayName;

    /// <summary>
    /// Layer on which we can touch things that aren't gui buttons
    /// </summary>
    public string TouchableLayerName = "Touchable";

    /// <summary>
    /// Layer for scope colliders.
    /// Really on used by SniperArena
    /// </summary>
    public string ScopeLayerName = "Scope";

    public float ScopeTransitionSpeed = 1;

    /// <summary>
    /// Health. Doesn't apply to all snipers.
    /// </summary>
    public float Health = 100;


    /// <summary>
    /// Maximum possible health
    /// </summary>
    public float MaxHealth = 100;

    /// <summary>
    /// Autofire enabled?
    /// </summary>
    public bool CanAutoFire = false;

    /// <summary>
    /// Time between scope adjustment
    /// </summary>
    public float TimeDelayBetweenShots = 2;

    /// <summary>
    /// How long can we see through walls?
    /// </summary>
    public float InstinctDuration = 4;

    /// <summary>
    /// The public field for primary hand mode. 
    /// This is the public-facing, but will not actually change the UI mode
    /// until you use the SniperPrimaryHand property.
    /// </summary>
    [Tooltip("Changing this during runtime will have no effect. Only change this before running the game.")]
    public PrimaryHand PrimaryHandMode = PrimaryHand.RightHanded;

    [Header("Rotation and Control")]
    /// <summary>
    /// Can the character be controlled right now?
    /// </summary>
    public bool IsUserControllable = true;

    public bool CanFire = true;
    public bool CanUserSwitchViews = true;



    public bool RestrictXRotation = true;
    public bool RestrictYRotation = true;

    /// <summary>
    /// Min and Max X rotation values. (up and down)
    /// </summary>
    public float MinXRotation = -60, MaxXRotation = 60;

    /// <summary>
    /// Min and Max Y rotation (left and right)
    /// </summary>
    public float MinYRotation = 90, MaxYRotation = 270;

    /// <summary>
    /// Sensitivity for camera rotation
    /// </summary>
    public float RotationSensitivity = .2f;

    /// <summary>
    /// How much rotation speed should be multiplied when going from scope to standard view.
    /// </summary>
    public float RotationMultiplier = 1.5f;

    /// <summary>
    /// If you'd prefer to use independent sensitivites rather than relative ones, set these to anything but -1
    /// </summary>
    public float ScopeRotationSensitivity = -1f;
    public float StandardRotationSensitivity = -1f;

    /// <summary>
    /// Apply extra smoothing?
    /// </summary>
    public bool ExtraSmooth = false;

    /// <summary>
    /// Not used if -1
    /// </summary>
    public float RotationSmooth = -1;

    public float InitialRecoilSpeed = .2f;

    /// <summary>
    /// Max rotation difference during a recoil.
    /// </summary>
    protected float currentRecoilSpeed = .2f;

    /// <summary>
    /// How long shots take place during dead eye.
    /// </summary>
    public float DeadEyeDuration = 5;

    [Header("Zoom")]

    /// <summary>
    /// Rate at which camera zooms
    /// </summary>
    public float ZoomSpeed = 12f;

    /// <summary>
    /// Min and max zoom levels. (Truly just for camera's field of view)
    /// </summary>
    public float MinZoom = 30, MaxZoom = 60;

    /// <summary>
    /// The lowest multiplier for rotation to zoom relation.
    /// </summary>
    public float MinimumRotationModifier = .25f;

    /// <summary>
    /// How much to multiply bullet speed for dead eye
    /// </summary>
    public float DeadEyeMultiplier = 5;


    [Header("Object References")]
    /// <summary>
    /// A reference to the current, main camera
    /// </summary>
    public Camera Camera;

    /// <summary>
    /// A reference to the current, scope camera.
    /// Two are necessary to we can use the scope of the gun model without acutally zooming in on the scope frame.
    /// </summary>
    public Camera ScopeCamera;

    /// <summary>
    /// Locations for the camera in scoped position and standard position
    /// </summary>
    public Transform ScopeLocation, StandardLocation;

    /// <summary>
    /// Reference to the sniper's reticule.
    /// </summary>
    public Transform Reticule;

    /// <summary>
    /// This is where the bullet should spawn
    /// </summary>
    public Transform BulletSpawnLocation;

    /// <summary>
    /// The Bullet to instantiate when we fire
    /// </summary>
    public GameObject BulletPrefab;

    /// <summary>
    /// Handles scope cameras and stuff.
    /// </summary>
    public GameObject ScopeParent;

    /// <summary>
    /// A reference to the Root of the controller. Rotations should be applied to this.
    /// </summary>
    public Transform Root;

    /// <summary>
    /// The current sniper panel being displayed, based on SniperMode
    /// </summary>
    public GameObject CurrentSniperPanel;





    [Header("Time Freeze Settings")]
    /// <summary>
    /// Are we in time freeze mode right now?
    /// </summary>
    public bool InTimeFreezeMode = false;

    /// <summary>
    /// How long does time freeze last?
    /// </summary>
    public float TimeFreezeLength = 3;

    /// <summary>
    /// Value to divide Enemies' speed by
    /// </summary>
    public float TimeFreezeModifier = 4;


    [Header("Autofire")]
    public float TimeBetweenShots = .1f;
    /// <summary>
    /// Is the character auto-firing now?
    /// </summary>
    public bool IsFiring = false;

    [Header("Bullet Cam Settings")]
    public bool InBulletCam = false;
    /// <summary>
    /// How slow should bullet cam time scale be?
    /// </summary>
    public float BulletCamTimeScale = .4f;





    [Header("UI")]
    public bool DisplayVariables = true;


    /// <summary>
    /// Text to display for the current sniper mode
    /// </summary>
    public Text txtSniperMode;


    /// <summary>
    /// Slider for zooming in and out.
    /// This is required for some sniper modes!
    /// </summary>
    public Slider sldZoom;

    /// <summary>
    /// The panel to output debug info
    /// </summary>
    public GameObject OutputPanel;

    /// <summary>
    /// Template for text to output
    /// </summary>
    public Text OutputTextPrefab;

    /// <summary>
    /// The slider to display health on.
    /// </summary>
    public Slider HealthSlider;

    public GameObject ScopeRenderUI;

    //UI panels
    public GameObject NormalPanel, ScopePanel, BothPanel;


    public Button btnThermal;


    [Header("Thermals")]
    //Thermal
    public float ThermalLength = 4;
    public Material DarkSkybox;
    protected bool thermalsActive = false;
    #endregion

    #region Private Fields
    /// <summary>
    /// Are we currently swapping between scope and standard view, or vice versa?
    /// </summary>
    protected bool isTransitioningViews = false;

    /// <summary>
    /// Used for Lerping
    /// </summary>
    private float time = 0; private float recoilTime = 0;

    //Recoil settings
    protected bool isRecoiling = false;
    protected float recoilDuration = 0;
    protected float maxRecoilXRotation = 0;


    /// <summary>
    /// Time to calculate firing
    /// </summary>
    protected float fireTime = 0;

    /// <summary>
    /// Must keep track of this outside update methods
    /// </summary>
    protected float xRot = 0, yRot = 0;

    /// <summary>
    /// How long were we rotating for? 
    /// Used to determine if a pinch-zoom is valid.
    /// </summary>
    protected float timeSingle = 0;


    protected Transform bulletCamTarget;

    private Dictionary<string, string> variableMessages;
    private Dictionary<string, Text> variableTexts;

    protected bool isZooming = false;

    protected Color startLightColor;
    protected Material startSkybox;

    protected bool isReloading = false;
    protected bool isMiniReloading = false;

    protected float startRotSens;

    //When instantiating bullets, what to multiply their initial force by
    protected float bulletSpeedMultiplier = 1;

    protected float initBulletDamage = 0;


    protected int enemiesRemaining = 0;

    /// <summary>
    /// Was the player user controllable at the time of switching scope modes? 
    /// </summary>
    protected bool wasUserControllable = true;

    //Used to determine if we've fired callback methods for transitions during time periods
    protected bool calledBeginTransition = false;
    protected bool calledMiddleTransition = false;
    protected bool calledEndTransition = false;


    protected AS_ActionCamera ActionCam;

    //Faster shots
    protected bool inDeadEyeMode = false;
    protected float bulletSpeedMultiplierBeforeDeadEye = 0;

    #endregion


    #region Properties
    /// <summary>
    /// Current zoom level, as a percentage. (100 is max zoom, 0 is min)
    /// </summary>
    public int Zoom
    {
        get
        {
            if (MaxZoom == MinZoom)
                return 100;

            float tempZoom = MaxZoom - ScopeCamera.fieldOfView;

            float zoomDecimal = tempZoom / (MaxZoom - MinZoom);

            float zoom = zoomDecimal * 100;

            return (int)zoom;
        }
        set
        {
            float zoomDecimal = value / 100.0f;
            float tempZoom = zoomDecimal * (MaxZoom - MinZoom);
            ScopeCamera.fieldOfView = MaxZoom - tempZoom;
        }
    }

    /// <summary>
    /// Camera's rotation, excluding the Z-component, because it's always zero.
    /// </summary>
    public Vector2 CameraRotation
    {
        get
        {
            if (ViewingScope)
            {
                return new Vector2(ScopeCamera.transform.eulerAngles.x, ScopeCamera.transform.eulerAngles.y);
            }
            else
            {
                return new Vector2(Root.transform.eulerAngles.x, Root.transform.eulerAngles.y);

            }
        }
    }




    //This is the protected field for the ViewingScope property below
    protected bool viewingScope = false;
    /// <summary>
    /// Is the character looking through the scope?
    /// </summary>
    public virtual bool ViewingScope
    {

        get
        {
            return viewingScope;
        }
        set
        {
            viewingScope = value;
            wasUserControllable = IsUserControllable;
            //IsControllable = false;
            calledBeginTransition = false;
            calledMiddleTransition = false;
            calledEndTransition = false;
            isTransitioningViews = true;
            if (CurrentSniperPanel)
            {
                //Activate standard panel if NOT viewing scope
                NormalPanel.SetActive(!viewingScope);
                //Activate scope panel if we ARE viewing scope
                ScopePanel.SetActive(viewingScope);
            }



            time = 0;
            if (viewingScope)
            {
                if (ScopeRotationSensitivity == -1f)
                    RotationSensitivity /= RotationMultiplier;
                else
                    RotationSensitivity = ScopeRotationSensitivity;

            }
            else
            {
                if (StandardRotationSensitivity == -1f)
                    RotationSensitivity *= RotationMultiplier;
                else
                    RotationSensitivity = StandardRotationSensitivity;

                if (thermalsActive)
                {
                    SetThermals(false);
                    StopCoroutine("ThermalDelay");
                }
            }


        }

    }

    /// <summary>
    /// This is the target x and y rotation values. Adjust this if you'll be editing the sniper's rotation manually.
    /// Note: Has no visible effect if the sniper cannot be controlled, but will resume effect the moment it can be controlled again.
    /// </summary>
    public Vector2 TargetXYRot
    {
        set
        {
            xRot = value.x;
            yRot = value.y;
        }
    }

    /// <summary>
    /// protected backing field for the SniperPrimaryHand property.
    /// </summary>
    protected PrimaryHand primaryHand = PrimaryHand.RightHanded;

    /// <summary>
    /// Change the sniper's control mode.
    /// Will adjust UI accordingly when set.
    /// </summary>
    public virtual PrimaryHand SniperPrimaryHand
    {
        get
        {
            return primaryHand;
        }
        set
        {
            //Don't do anything if controlmode won't be changed.
            if (primaryHand == value)
                return;
            primaryHand = value;
            //Go through children of the sniper panel, and flip appropriate ui elements.
            RectTransform[] uiChildren = CurrentSniperPanel.GetComponentsInChildren<RectTransform>(true);
            print(uiChildren.Length);
            foreach (RectTransform child in uiChildren)
            {
                //We'll flip if it's anchored left or anchored right, and not stretched in the x direction
                if ((child.anchorMin.x == 0 && child.anchorMax.x == 0) || (child.anchorMin.x == 1 && child.anchorMax.x == 1))
                {
                    FlipRectTransform(child);
                }
            }
        }
    }

    /// <summary>
    /// Basically gets you the camera that's currently active. 
    /// However, keep in mind that the scope camera is for a render texture.
    /// </summary>
    public Camera ActiveCamera
    {
        get
        {
            if (ViewingScope)
                return ScopeCamera;
            else
                return Camera;
        }
    }

    /// <summary>
    /// How much damage should the bullet do to enemies?
    /// </summary>
    public float BulletDamage { get; protected set; }


    #endregion

    /// <summary>
    /// Called when time freeze button is pressed or TimeFreeze wears off
    /// </summary>
    public event UnityAction<bool> TimeFreeze;

    /// <summary>
    /// Called when bullet cam started or ended
    /// </summary>
    public event UnityAction<bool> BulletCamEvent;

    /// <summary>
    /// Called when a powerup started to be used.
    /// </summary>
    public event UnityAction<Powerup> PowerupBegin;

    /// <summary>
    /// Called when a powerup ended.
    /// </summary>
    public event UnityAction<Powerup> PowerupEnd;

    //Instinct is code for see-through enemies
    public event UnityAction InstinctBegin;
    public event UnityAction InstinctEnd;
    protected bool isInInstinctMode = false;


    protected virtual void OnEnable()
    {
        StartCoroutine(DelayedEnable());
        //The following code relies on the fact that in the canvas,
        //the Ui panels are named in the following format:
        //pSniperMode, which has children --> pStandard, pScope, and pBoth, in that order
        //For example: panel has children pStandard, pScope, and pBoth

        CurrentSniperPanel.SetActive(true);
        BothPanel.SetActive(true);

        if (txtSniperMode)
            txtSniperMode.text = DisplayName;

        //For zoom slider
        if (sldZoom)
        {
            sldZoom.minValue = MinZoom;
            sldZoom.maxValue = MaxZoom;
            sldZoom.value = sldZoom.maxValue;
        }
        if (ScopeRenderUI)
            ScopeRenderUI.SetActive(false);

        xRot = Root.transform.eulerAngles.x;
        yRot = Root.transform.eulerAngles.y;

        ScopeCamera.fieldOfView = MaxZoom;
        variableMessages = new Dictionary<string, string>();
        variableTexts = new Dictionary<string, Text>();
        isReloading = false;
        isMiniReloading = false;
        isInInstinctMode = false;
        //For thermals
        if (startLightColor == default(Color))
        {
            if (GameObject.FindWithTag("Light"))
                startLightColor = GameObject.FindWithTag("Light").GetComponent<Light>().color;
        }
        else
        {
            if (GameObject.FindWithTag("Light"))
                GameObject.FindWithTag("Light").GetComponent<Light>().color = startLightColor;
        }
        if (startSkybox == null)
        {
            startSkybox = RenderSettings.skybox;
        }
        else
        {
            RenderSettings.skybox = startSkybox;
        }

        //reset bullet speed mult
        bulletSpeedMultiplier = 1;

        //Reset bullet damage
        if (BulletPrefab.GetComponent<Bullet>())
            BulletDamage = BulletPrefab.GetComponent<Bullet>().Strength;
        initBulletDamage = BulletDamage;

        //Reset recoil time
        recoilTime = 0;

        InTimeFreezeMode = false;
        InBulletCam = false;

        //For special bullet vfx
        ActionCam = GameObject.FindWithTag("ActionCam").GetComponent<AS_ActionCamera>();
        ActionCam.BeginAction += OnActionCamBegin;
        ActionCam.EndAction += OnActionCamEnd;


        ViewingScope = false;

        //Update the handmode property
        SniperPrimaryHand = PrimaryHandMode;


    }

    /// <summary>
    /// Called when the Action Camera (from Advanced Sniper) begins it's vfx
    /// </summary>
    protected virtual void OnActionCamBegin()
    {

    }
    /// <summary>
    /// Called when the Action Camera (from Advanced Sniper) ends it's vfx
    /// </summary>
    protected virtual void OnActionCamEnd()
    {

    }



    public void BeginDeadEye()
    {
        inDeadEyeMode = true;
        bulletSpeedMultiplierBeforeDeadEye = bulletSpeedMultiplier;
        bulletSpeedMultiplier = DeadEyeMultiplier;
        StartCoroutine(DeadEyeIE());
    }

    protected IEnumerator DeadEyeIE()
    {
        yield return new WaitForSeconds(DeadEyeDuration);
        EndDeadEye();
    }

    protected void EndDeadEye()
    {
        inDeadEyeMode = false;
        bulletSpeedMultiplier = bulletSpeedMultiplierBeforeDeadEye;
    }



    public virtual void OnInstinctBegin()
    {
        if (InstinctBegin != null && !isInInstinctMode)
        {
            InstinctBegin();
            isInInstinctMode = true;
            StartCoroutine(InstinctIE());
        }
    }

    protected virtual IEnumerator InstinctIE()
    {
        yield return new WaitForSeconds(InstinctDuration);
        OnInstinctEnd();
    }

    protected virtual void OnInstinctEnd()
    {
        if (InstinctEnd != null)
        {
            InstinctEnd();
            isInInstinctMode = false;
        }
    }


    /// <summary>
    /// Static method that will flip the orienation of a recttransform horizontally
    /// </summary>
    public static void FlipRectTransform(RectTransform rect)
    {
        Vector2 initAnchoredPos = rect.anchoredPosition;
        //If it's anchored to the left, so move it to the right
        if (rect.anchorMin.x == 0)
        {
            rect.anchorMin = new Vector2(1, rect.anchorMin.y);
            rect.anchorMax = new Vector2(1, rect.anchorMax.y);
            rect.anchoredPosition = new Vector2(-initAnchoredPos.x, rect.anchoredPosition.y);
        }
        //Otherwise must be anchored on the right, so move it to the left
        else
        {
            rect.anchorMin = new Vector2(0, rect.anchorMin.y);
            rect.anchorMax = new Vector2(0, rect.anchorMax.y);
            rect.anchoredPosition = new Vector2(-initAnchoredPos.x, rect.anchoredPosition.y);

        }

        //If we have text, swap its alignment
        if (rect.GetComponent<Text>())
        {
            Text t = rect.GetComponent<Text>();
            int alignment = (int)t.alignment;
            //Left alignments are multiples of 3. 
            if ((alignment / 3.0f) == (alignment / 3))
            {
                //To swap sides from left to right, just add 2
                t.alignment = (TextAnchor)(alignment + 2);

            }
            //Right alignments are multiples of 3, minus 1. So check if it's alignment + 1 is a multiple of 3
            else if ((alignment + 1) / 3.0f == (alignment + 1) / 3)
            {
                //To swap sides from right to left, just subtract 2
                t.alignment = t.alignment -= 2;
            }
            Debug.Log(t.alignment.ToString());
        }

    }

    /// <summary>
    /// Raise the powerup event. Done this way so subclasses can also raise it.
    /// </summary>
    /// <param name="power">name of power up</param>
    protected virtual void OnPowerupBegin(Powerup power)
    {
        if (PowerupBegin != null)
            PowerupBegin(power);
    }

    protected virtual void OnPowerupEnd(Powerup power)
    {
        if (PowerupEnd != null)
            PowerupEnd(power);
    }


    /// <summary>
    /// This is so we don't pick up stray touches on enable
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DelayedEnable()
    {
        yield return new WaitForSeconds(0.1f);
        //See how many enemies are alive
        if (GameObject.Find("Enemies"))
            enemiesRemaining = GameObject.Find("Enemies").transform.childCount;
        LeanTouch.OnFingerTap += OnFingerDown;
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
    }


    protected virtual void OnDisable()
    {
        LeanTouch.OnFingerTap -= OnFingerDown;

        ////Turn off the UI
        //foreach (Transform t in CurrentSniperPanel.transform)
        //    t.gameObject.SetActive(false);

        //Delete all children of output panel
        if (DisplayVariables && OutputPanel)
        {
            foreach (Transform t in OutputPanel.transform)
                Destroy(t.gameObject);
        }

        if (!ViewingScope && RotationMultiplier != -1)
        {
            RotationSensitivity /= RotationMultiplier;
        }

    }

    /// <summary>
    /// Provides a simple way to show a message to the gui.
    /// </summary>
    /// <param name="id">Unique id of the message</param>
    /// <param name="value">value of the message</param>
    public void ShowMessage(string id, string value)
    {
        if (!DisplayVariables)
            return;

        if (!variableMessages.ContainsKey(id))
        {
            variableMessages.Add(id, value);
            Text t = Instantiate<Text>(OutputTextPrefab);
            t.text = value;
            t.transform.SetParent(OutputPanel.transform, false);

            variableTexts.Add(id, t);
        }
        else
        {
            variableTexts[id].text = value;
        }
    }




    /// <summary>
    /// Event handler for finger down
    /// Handled by derived classes
    /// </summary>
    /// <param name="finger">finger that is down</param>
    protected virtual void OnFingerDown(LeanFinger finger)
    {
    }

    protected virtual void OnFingerSwipe(LeanFinger finger)
    {
    }
    /// <summary>
    /// Event handler for TimeFreeze event
    /// </summary>
    public virtual void OnTimeFreezeBegin()
    {
        print("Time freeze begin");
        if (!InTimeFreezeMode && !InBulletCam)
        {
            InTimeFreezeMode = true;
            //Call the event
            if (TimeFreeze != null)
                TimeFreeze(true);

            StartCoroutine("TimeFreezeDelay");
            //Other logic handled in Enemy.cs
        }

    }

    public virtual void OnTimeFreezeEnd()
    {
        if (InTimeFreezeMode)
        {
            InTimeFreezeMode = false;
            if (TimeFreeze != null)
                TimeFreeze(InTimeFreezeMode);
            Recoil(recoilDuration, InitialRecoilSpeed / 2);
            StopCoroutine("TimeFreezeDelay");

        }
    }

    IEnumerator TimeFreezeDelay()
    {
        yield return new WaitForSeconds(TimeFreezeLength);
        OnTimeFreezeEnd();
    }


    /// <summary>
    /// Called when the sniper should fire.
    /// </summary>
    public virtual void OnFireBegin()
    {
        if (CanFire)
            IsFiring = true;
    }

    public virtual void OnFireEnd()
    {
        if (CanFire)
        {
            IsFiring = false;
            fireTime = 0;
        }

    }

    public void OnBulletCamBegin()
    {
        InBulletCam = true;
        Time.timeScale = BulletCamTimeScale;
        Time.fixedDeltaTime *= BulletCamTimeScale;


        CurrentSniperPanel.gameObject.SetActive(false);

        OnTimeFreezeEnd();

        //Notice the use of the field, NOT the property
        //This is so we don't have double camera transitions
        viewingScope = false;


        xRot = ScopeCamera.transform.eulerAngles.x;
        yRot = ScopeCamera.transform.eulerAngles.y;

        if (BulletCamEvent != null)
            BulletCamEvent(true);
    }

    public virtual void OnBulletCamEnd(Enemy sender)
    {
        InBulletCam = false;
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime /= BulletCamTimeScale;
        }

        //Setting this will set IsViewingScope to false, which will move the camera back, for SniperFury
        if (sldZoom)
            sldZoom.value = sldZoom.maxValue;

        if (BulletCamEvent != null)
            BulletCamEvent(false);

        sender.Alive = false;

    }

    /// <summary>
    /// Set various values for thermals
    /// </summary>
    /// <param name="inThermalMode">true for thermals, false for not</param>
    protected void SetThermals(bool value)
    {
        Light light = GameObject.FindWithTag("Light").GetComponent<Light>();
        if (value)
        {
            OnPowerupBegin(Powerup.Thermal);

            light.color = new Color32(0x64, 0x0B, 0xFF, 0xFF);
            RenderSettings.skybox = DarkSkybox;
            Camera.backgroundColor = Color.black;
        }
        else
        {
            OnPowerupEnd(Powerup.Thermal);

            light.color = startLightColor;
            RenderSettings.skybox = startSkybox;
            Camera.backgroundColor = Color.black;

        }
        thermalsActive = value;

    }

    /// <summary>
    /// Called when thermal visor button pressed
    /// </summary>
    public virtual void OnThermal()
    {
        //Adjust lighting to simulate thermals
        SetThermals(true);
        StartCoroutine("ThermalDelay");

    }

    protected virtual IEnumerator ThermalDelay()
    {
        yield return new WaitForSeconds(ThermalLength);
        //Adjust lighting to simulate thermals
        SetThermals(false);

    }



    /// <summary>
    /// To be called once scope transition is complete.
    /// </summary>
    protected virtual void OnFinishedScopeTransition()
    {
        if (Reticule)
            Reticule.gameObject.SetActive(ViewingScope);
    }

    protected virtual void OnFinishedScopeTransitionMiddle()
    {

    }

    /// <summary>
    /// Like the above, but called at the tail end
    /// </summary>
    protected virtual void OnFinishedScopeTransitionLate()
    {
    }

    public virtual void OnReload()
    {

    }
    /// <summary>
    /// Toggles scope viewing mode
    /// </summary>
    public void ToggleScope()
    {
        ViewingScope = !ViewingScope;
    }

    /// <summary>
    /// Generic Zoom Controls
    /// </summary>
    protected virtual void ZoomPinchControls(bool ignoreGUI = true)
    {

        if (timeSingle > .1f)
            return;

        List<LeanFinger> fingers = LeanTouch.GetFingers(ignoreGUI);


        if (ViewingScope)
        {
            float pinchRadius = LeanGesture.GetPinchRatio(fingers, -.1f) - 1;

            if (Mathf.Abs(pinchRadius) > 0)
                isZooming = true;
            else
                isZooming = false;

            ScopeCamera.fieldOfView += pinchRadius * ZoomSpeed;
            ScopeCamera.fieldOfView = Mathf.Clamp(ScopeCamera.fieldOfView, MinZoom, MaxZoom);
        }


    }

    /// <summary>
    /// Return the current Zoom value scaled linearly from the original (0 to 100) to the new (min to max).
    /// </summary>
    public float GetZoomFrom(int min, int max)
    {
        float divisor = 100.0f / (max - min);
        float newZoom = min + (Zoom / divisor);
        return newZoom;
    }

    /// <summary>
    /// Slider controls for zooming in and out
    /// </summary>
    protected void ZoomSlideControls()
    {

        float sliderVal = sldZoom.value;

        ScopeCamera.fieldOfView = sliderVal;


        // ShowMessage("Zoom", "Camera Zoom Level : " + Zoom + "%");
    }

    /// <summary>
    /// Touch rotation controls
    /// </summary>
    /// <param name="ignoreGUI">Should we ignore gui?</param>
    /// <param name="linear">Apply a decreasing linear relationship between rotation sensitivity and zoom?</param>
    protected virtual void RotationControls(bool ignoreGUI = true, bool linear = false)
    {

        if (isZooming)
            return;



        //For smooth rotation
        Vector3 prevEuler = new Vector3(xRot, yRot, 0);

        List<LeanFinger> fingers = LeanTouch.GetFingers(ignoreGUI);


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
        //Gun.transform.rotation = Quaternion.Euler();

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
    /// Fire a real bullet from the muzzle of the gun towards the center of the screen
    /// </summary>
    /// <returns>True if hit enemy, false otherwise</returns>
    protected bool FireBulletFromGunMuzzle()
    {
        float reach = 100;
        //Get the starting point
        Vector3 start = BulletSpawnLocation.position;
        //Calculate ending point
        Vector3 end;
        RaycastHit hit;

        //mask with only the gun visible
        int mask = ~(1 << 8);

        if (Physics.Raycast(new Ray(Camera.transform.position, Camera.transform.forward), out hit, float.MaxValue, mask))
        {
            end = hit.point;
        }
        else
        {
            end = Camera.transform.position + (reach * Camera.transform.forward);
        }
        //Use that info to calculate bullet direction
        Vector3 dir = (end - start).normalized;
        //Spawn a bullet
        GameObject bullet = Instantiate<GameObject>(BulletPrefab, start, Quaternion.LookRotation(dir));

        if (bullet.GetComponent<Bullet>())
        {
            bullet.GetComponent<Bullet>().ScaleForce(bulletSpeedMultiplier);
            //Set it here explicitly in case BulletDamage has changed
            bullet.GetComponent<Bullet>().Strength = BulletDamage;
        }


        //Determine if we hit an enemy
        bool hitEnemy = false;
        RaycastHit enemyHitInfo;
        if (Physics.Raycast(new Ray(start, end), out enemyHitInfo, float.MaxValue, mask))
        {
            if (enemyHitInfo.transform.tag == "Enemy")
            {
                hitEnemy = true;
            }
        }

        if (hitEnemy)
        {
            bulletCamTarget = bullet.transform;
            if (bullet.GetComponent<Bullet>())
                bullet.GetComponent<Bullet>().IsTheSlowMoBullet = true;
            enemiesRemaining--;
        }
        return hitEnemy;
    }


    /// <summary>
    /// Helper method to shoot from the center of the screen
    /// </summary>
    /// <param name="destructive">Should the bullet do damage to the enemy?</param>
    /// <returns>True if hit enemy, false otherwise</returns>
    protected bool FireCentered(bool destructive = true)
    {
        if (!CanFire)
            return false;
        //Figure out if casting from main camera results in a hit
        Vector3 targetPoint;
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit))
        {
            targetPoint = hit.point;
            if (hit.transform.tag == "Enemy")
            {
                if (destructive)
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    enemy.OnBulletHit(BulletDamage);
                    enemiesRemaining--;
                }
                return true;
            }
        }
        return false;
    }


    protected virtual bool FireCenteredWithBullet()
    {
        //Figure out if casting from main camera results in a hit
        Vector3 targetPoint;
        RaycastHit hit;
        bool hitEnemy = false;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit))
        {
            targetPoint = hit.point;
            if (hit.transform.tag == "Enemy")
                hitEnemy = true;
        }
        else
        {
            targetPoint = Camera.transform.position + Camera.transform.forward * 50;
        }
        //Spawn bullet from spawnlocation facing towards camera hit loc
        Vector3 direction = targetPoint - BulletSpawnLocation.position;

        GameObject bullet = Instantiate<GameObject>(BulletPrefab, BulletSpawnLocation.position, Quaternion.LookRotation(direction));
        if (bullet.GetComponent<Bullet>())
        {
            bullet.GetComponent<Bullet>().ScaleForce(bulletSpeedMultiplier);
            //Set it here explicitly in case BulletDamage has changed
            bullet.GetComponent<Bullet>().Strength = BulletDamage;
        }

        if (hitEnemy)
        {
            bulletCamTarget = bullet.transform;
            if (bullet.GetComponent<Bullet>())
                bullet.GetComponent<Bullet>().IsTheSlowMoBullet = true;
            enemiesRemaining--;
        }


        return hitEnemy;
    }

    /// <summary>
    /// Fire from the center of the UI scope centered on the screen 
    /// </summary>
    /// <param name="destructive">Should the bullet do damage to the enemy?</param>
    /// <returns>True if raycast hit an enemy, false otherwise</returns>
    protected virtual bool FireFromUIScope(bool destructive = true)
    {
        if (!CanFire)
            return false;
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
                    enemy.OnBulletHit(BulletDamage);
                    enemiesRemaining--;
                }

                return true;
            }
            else if (enemyHit.transform.tag == "Destructable")
            {
                if (destructive)
                {
                    if (enemyHit.transform.gameObject.GetComponent<IDamagable>() != null)
                    {
                        enemyHit.transform.gameObject.GetComponent<IDamagable>().ReciveDamage(BulletDamage);
                    }

                }
            }

        }


        return false;
    }
    /// <summary>
    /// Fire from the center of the UI scope centered on the screen with an actual bullet instead of a raycast
    /// </summary>
    /// <returns>True if raycast hit an enemy, false otherwise</returns>
    protected virtual bool FireFromUIScopeWithBullet()
    {
        if (!CanFire)
            return false;
        //Figure out if casting from main camera results in a hit
        Vector3 targetPoint;
        RaycastHit hit;
        bool hitEnemy = false;
        if (Physics.Raycast(ScopeCamera.transform.position, ScopeCamera.transform.forward, out hit))
        {
            targetPoint = hit.point;
            if (hit.transform.tag == "Enemy")
                hitEnemy = true;
        }
        else
        {
            targetPoint = ScopeCamera.transform.position + ScopeCamera.transform.forward * 50;
        }
        //Spawn bullet from spawnlocation facing towards camera hit loc
        Vector3 direction = targetPoint - BulletSpawnLocation.position;

        GameObject bullet = Instantiate<GameObject>(BulletPrefab, BulletSpawnLocation.position, Quaternion.LookRotation(direction));
        if (bullet.GetComponent<Bullet>())
        {
            bullet.GetComponent<Bullet>().ScaleForce(bulletSpeedMultiplier);
            //Set it here explicitly in case BulletDamage has changed
            bullet.GetComponent<Bullet>().Strength = BulletDamage;
        }
        else if (bullet.GetComponent<AS_Bullet>())
        {
            bullet.GetComponent<AS_Bullet>().MuzzleVelocity *= bulletSpeedMultiplier;
        }
        if (hitEnemy)
        {
            bulletCamTarget = bullet.transform;
            if (bullet.GetComponent<Bullet>())
                bullet.GetComponent<Bullet>().IsTheSlowMoBullet = true;
            enemiesRemaining--;
        }


        return hitEnemy;

    }



    /// <summary>
    /// Helper method to shoot relative to the scope
    /// </summary>
    /// <returns>True if raycast hit an enemy, false otherwise</returns>
    protected virtual bool FireFromScopeWithBullet()
    {
        if (!CanFire)
            return false;
        //Make sure that SelectBlocker is either disabled or on the Scope layer
        //Also confirm that ScopeRender has a Mesh Collider that is NOT convex
        //Lastly, ScopeRender, and not its children, should be on the default layer.


        //Fire a raycast straight into the render texture
        RaycastHit hitRd;
        Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitRd, 100.0f, LayerMask.NameToLayer(ScopeLayerName));

        print(hitRd.transform.name);
        //Determine what point on the render texture that it is in world space
        //Requires mesh collider
        Ray renderRay = ScopeCamera.ViewportPointToRay(hitRd.textureCoord);
        //Instantiate the bullet at the ray's origin and direction (plus an offset to account for zoom)

        GameObject bullet = Instantiate<GameObject>(BulletPrefab, renderRay.GetPoint(3.0f + (100 / ScopeCamera.fieldOfView)), Quaternion.LookRotation(renderRay.direction));
        if (bullet.GetComponent<Bullet>())
        {
            bullet.GetComponent<Bullet>().ScaleForce(bulletSpeedMultiplier);
            bullet.GetComponent<Bullet>().Strength = BulletDamage;
        }
        else if (bullet.GetComponent<AS_Bullet>())
        {
            bullet.GetComponent<AS_Bullet>().MuzzleVelocity *= bulletSpeedMultiplier;
        }


        //Use the raycast to determine if we're gonna hit an enemy.
        RaycastHit enemyHit;
        if (Physics.Raycast(renderRay, out enemyHit))
        {
            if (enemyHit.transform.tag == "Enemy")
            {
                bulletCamTarget = bullet.transform;
                if (bullet.GetComponent<Bullet>())
                    bulletCamTarget.GetComponent<Bullet>().IsTheSlowMoBullet = true; //Make sure it doesn't get destroyed after lifetime
                enemiesRemaining--;
                return true;
            }

        }


        return false;
    }


    /// <summary>
    /// Apply a recoil effect to the gun.
    /// </summary>
    /// <param name="duration"></param>
    protected virtual void Recoil(float duration, float speed)
    {
        isRecoiling = true;
        recoilDuration = duration;
        recoilTime = 0;
        currentRecoilSpeed = speed;
    }

    /// <summary>
    /// Updates health graphics
    /// </summary>
    public virtual void UpdateHealth()
    {
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        HealthSlider.value = Health;
    }


    /// <summary>
    /// Returns a modifier zoom from a linear decay function
    /// </summary>
    /// <param name="zoom">Zoom value</param>
    /// <param name="min">Minimum zoom modifier - ideally greater than zero and less than one</param>
    /// <returns>Zoom modifier, a float from min to 1</returns>
    protected float InvZoom(float zoom, float min)
    {
        //f(1) = 1
        //f(100) .01f

        float slope = (min - 1) / 99f;
        return (slope * (zoom - 1)) + 1; //Derived from point-slope form
    }


    protected void FixedUpdate()
    {
        if (InBulletCam)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, bulletCamTarget.transform.position, .1f);
        }
    }


    // Update is called once per frame
    protected virtual void Update()
    {

        //Transition animation between views
        if (isTransitioningViews)
        {
            //Set target position 
            Vector3 target = ViewingScope ? ScopeLocation.position : StandardLocation.position;

            //Lerp to the new position
            time += Time.deltaTime * ScopeTransitionSpeed;

            Camera.transform.position = Vector3.Lerp(Camera.transform.position, target, time);

            if (time >= .2f && !calledBeginTransition)
            {
                OnFinishedScopeTransition();
                ScopeParent.SetActive(ViewingScope);
                calledBeginTransition = true;
            }

            if (time >= .5f && !calledMiddleTransition)
            {
                OnFinishedScopeTransitionMiddle();
                calledMiddleTransition = true;
            }

            if (time >= .8f && !calledEndTransition)
            {
                OnFinishedScopeTransitionLate();
                calledEndTransition = true;
            }


            if (time >= 1)
            {
                isTransitioningViews = false;
                IsUserControllable = wasUserControllable;
            }

        }

        //Affect children too
        foreach (Transform child in ScopeCamera.transform)
        {
            child.GetComponent<Camera>().fieldOfView = ScopeCamera.fieldOfView;
        }


        List<LeanFinger> fingers = LeanTouch.GetFingers(false);

        //Used for ZoomPinch 
        if (fingers.Count == 1)
        {
            timeSingle += Time.deltaTime;
        }
        if (fingers.Count == 0)
        {
            timeSingle = 0;
        }


        //Recoil
        if (isRecoiling)
        {

            recoilTime += Time.deltaTime;
            float delta = 0;
            float upDivisor = 20.0f;
            //Quickly move up
            if (recoilTime < recoilDuration / upDivisor)
            {
                delta = -currentRecoilSpeed * upDivisor;
            }
            //Slowly move back down
            else if (recoilTime < recoilDuration)
            {
                delta = currentRecoilSpeed * (upDivisor / (upDivisor - 1));
            }
            else
            {
                isRecoiling = false;
                recoilTime = 0;
            }
            if (ViewingScope)
            {
                //To Adjust for zoom 
                delta *= InvZoom(Zoom, MinimumRotationModifier * 2);
            }


            xRot += delta;
        }
    }


    /// <summary>
    /// Wrap angle so it's between 0 and 360. 
    /// Written by TheBeardPhantom from http://answers.unity3d.com/questions/227736/clamping-a-wrapping-rotation.html
    /// </summary>
    public static float WrapAngle(float angle)
    {
        if (angle < 0f)
            return angle + (360f * (int)((angle / 360f) + 1));
        else if (angle > 360f)
            return angle - (360f * (int)(angle / 360f));
        else
            return angle;
    }


}
