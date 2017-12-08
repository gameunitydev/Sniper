using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HitmanSniper : SniperController
{

    public Text txtReload;
    public Text txtZoom;
    public Text txtDistanceToTarget;

    [Header("Ammo")]
    //How much ammo before reload
    public int AmmoPerClip = 3;
    /// <summary>
    /// How long does reload last
    /// </summary>
    public float ReloadTime = 2;





    //Current ammo level
    private int currentAmmo;


    [Header("Quicktime Events")]
    /// <summary>
    /// For the quicktime events
    /// </summary>
    public Transform ArrowParent;
    /// <summary>
    /// Arrow display duration
    /// </summary>
    public float TimePerArrow = 2;

    /// <summary>
    /// How far along the arrow the user has to swipe to activate it.
    /// </summary>
    public float MinimumSlideDistance = 12;

    [Header("Targetting")]
    //Distance to this object will be displayed.
    public GameObject Target;

    private float distanceToTarget = 0;


    private int curArrow = 0;
    private List<GameObject> arrows;

    /// <summary>
    /// Used for determining when next arrow should be displayed
    /// </summary>
    private float reloadTime = 0;

    /// <summary>
    /// Was there a successful swipe recently?
    /// </summary>
    private bool wasSuccessfulSwipe = false;

    //Initial pointer location on drag
    private Vector2 startPointerLocation;
    private Vector2 endPointerLocation;

    //Did we start over the arrow?
    private bool overArrow;

    private bool panning = false;

    private float timeDown = 0;

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
        }



    }


    protected override void OnEnable()
    {
        base.OnEnable();
        currentAmmo = AmmoPerClip;
        distanceToTarget = 0;
        //Get permanent references to arrow children
        arrows = new List<GameObject>();
        foreach (Transform child in ArrowParent)
        {
            arrows.Add(child.gameObject);
            child.gameObject.GetComponent<Image>().color = Color.white;

            child.gameObject.GetComponent<Image>().enabled = false;

            child.GetComponent<Animator>().SetTrigger("Reset");

        }
        //Also add the 3D one
        Transform last = Root.transform.Find("Gun").transform.Find("Canvas").GetChild(0);
        arrows.Add(last.gameObject);
        last.gameObject.GetComponent<Image>().enabled = false;
        last.GetComponent<Animator>().SetTrigger("Reset");


        isReloading = false;
        isMiniReloading = false;

        //Setup the target enemy
        if (Target != null && Target.GetComponent<Enemy>())
            Target.GetComponent<Enemy>().IsTarget = true;


        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerSwipe += OnSwipe;

    }

    protected override void OnDisable()
    {
        arrows.Clear();
        base.OnDisable();

    }

    /// <summary>
    /// Does the provided list of RaycastResults contain a gameobject named name?
    /// </summary>
    private bool ContainsByName(List<RaycastResult> hits, string name)
    {
        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject.name == name)
                return true;
        }
        return false;
    }

    private bool ContainsByNameWithNoButtons(List<RaycastResult> hits, string name)
    {
        bool result = false;
        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject.name == name)
            {
                result = true;
                continue;
            }
            if (hit.gameObject.GetComponent<Button>())
                return false;
        }
        return result;
    }

    /// <summary>
    /// Does the provided list of RaycastResults contain a single gameobject named name?
    /// </summary>
    private bool ContainsByNameExclusive(List<RaycastResult> hits, string name)
    {
        if (hits.Count != 1)
            return false;
        return (hits[0].gameObject.name == name);
    }

    /// <summary>
    /// Fire on finger down if in scope area, otherwise exit scope mod
    /// </summary>
    protected override void OnFingerDown(LeanFinger finger)
    {
        base.OnFingerDown(finger);


        if (!ViewingScope && !finger.IsOverGui && !isReloading)
        {
            ViewingScope = true;
            return;
        }

        //Make a new PointerEventData
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        //Grab current finger position
        pointer.position = finger.ScreenPosition;
        //Prepare list of hits
        List<RaycastResult> hits = new List<RaycastResult>();
        //Do the raycast
        EventSystem.current.RaycastAll(pointer, hits);


        if (ContainsByNameWithNoButtons(hits, "ScopeView"))
        {
            OnFireBegin();
            IsFiring = true;
        }
        else if (ContainsByNameExclusive(hits, "Blur"))
        {
            print("Leaving now!");
            ViewingScope = false;
        }
    }

    /// <summary>
    /// Was the last click quick enough?
    /// </summary>
    /// <returns></returns>
    private bool ValidClick()
    {
        return (Time.time - timeDown) < 0.1f && (endPointerLocation - startPointerLocation) == Vector2.zero;
    }

    /// <summary>
    /// Called when clicked outside of scope, signaling leaving scope view
    /// </summary>
    public void OnLeaveScope()
    {
        /*  if (ValidClick())
          {
              print("Leaving now!");
              ViewingScope = false;

          }*/
    }

    /// <summary>
    /// Called when scope is clicked 
    /// </summary>
    public void OnFireByClickingScope()
    {
        /*if (ViewingScope && ValidClick())
        {
            OnFireBegin();
            IsFiring = true;
        }*/

    }




    protected void OnFingerUp(LeanFinger finger)
    {
        IsFiring = false;
        panning = false;
    }



    private void OnSwipe(LeanFinger finger)
    {

        if (!overArrow || !isReloading)
            return;

        Vector2 delta = finger.SwipeScreenDelta;
        //Find out if swiped in the correct direction
        bool correctDir = false;
        if (curArrow == 0 && delta.x < 0 && delta.y < 0)
            correctDir = true;
        if (curArrow == 1 && delta.x > 0 && delta.y > 0)
            correctDir = true;
        if (curArrow == 2 && delta.y > 0)
            correctDir = true;

        //See if we started on the arrow
        //EventSystem.current.

        if (correctDir)
            wasSuccessfulSwipe = true;

        overArrow = false;
    }


    public override void OnFireBegin()
    {
        if (isReloading || isMiniReloading)
            return;
        base.OnFireBegin();

        fireTime = 0;
        FireFromUIScopeWithBullet();

        currentAmmo--;
        if (currentAmmo <= 0)
        {
            OnReload();
        }
        else
        {
            isMiniReloading = true;
            StartCoroutine("FireDelay");
        }


    }


    private IEnumerator FireDelay()
    {
        Recoil(TimeDelayBetweenShots, InitialRecoilSpeed);
        yield return new WaitForSeconds(TimeDelayBetweenShots);
        isMiniReloading = false;

    }


    /// <summary>
    /// Called when reloading takes place.
    /// </summary>
    public override void OnReload()
    {
        if (ViewingScope)
            ViewingScope = false;
        isReloading = true;
        isMiniReloading = false;
        IsFiring = false;
        //Enable the arrows, causing the first one to just play, and prepare for quicktime
        ArrowParent.gameObject.SetActive(true);
        reloadTime = 0;
        curArrow = 0;


        arrows[0].GetComponent<Image>().raycastTarget = true;
        arrows[0].GetComponent<Image>().color = Color.white;
        arrows[0].GetComponent<Image>().enabled = true;



        arrows[0].GetComponent<Animator>().SetTrigger("Begin");

        //Assuming each arrow is enabled to start with, disable the non-first ones
        for (int i = 1; i < arrows.Count; i++)
        {
            arrows[i].GetComponent<Image>().color = Color.white;

        }
    }

    /// <summary>
    /// Show the arrows to allow swiping. Called from Update
    /// </summary>
    private void ReloadQuicktime()
    {

        reloadTime += Time.deltaTime;
        //If a swipe on that arrow occurs or time is greater than timeperarrow, or
        //there was a successful swipe (See OnSwipeBegin and OnSwipeEnd), setup next arrow.
        if (reloadTime >= TimePerArrow || wasSuccessfulSwipe)
        {

            //Fade arrow 
            arrows[curArrow].GetComponent<Animator>().SetTrigger("Fade");
            //We don't want to check for collisions with this anymore.
            arrows[curArrow].GetComponent<Image>().raycastTarget = false;
            //Change to green color if successful
            if (wasSuccessfulSwipe)
                arrows[curArrow].GetComponent<Image>().color = Color.green;
            else
                arrows[curArrow].GetComponent<Image>().color = Color.red;
            //Show next arrow
            curArrow++;

            if (curArrow < arrows.Count)
            {
                arrows[curArrow].GetComponent<Animator>().SetTrigger("Begin");
                //Make sure we do check for collisions with this
                arrows[curArrow].GetComponent<Image>().raycastTarget = true;
                arrows[curArrow].GetComponent<Image>().color = Color.white;
                arrows[curArrow].GetComponent<Image>().enabled = true;

            }
            else
            {
                StartCoroutine(ReloadEnd());

            }
            //Make sure we don't get repeat events
            wasSuccessfulSwipe = false;
            reloadTime = 0;
        }
    }

    private IEnumerator ReloadEnd()
    {
        yield return new WaitForSeconds(0.5f);
        currentAmmo = AmmoPerClip;
        isReloading = false;
        ViewingScope = true;
        curArrow = 0;
        //foreach (GameObject child in arrows)
        //    child.SetActive(false);

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

    protected override void ZoomPinchControls(bool ignoreGUI = true)
    {
        //if (isMiniReloading)
        //{
        //    isZooming = false;
        //    return;
        //}
        base.ZoomPinchControls(ignoreGUI);
        if (ViewingScope)
        {
            string zm = "X" + (GetZoomFrom(4, 10).ToString("F1"));
            txtZoom.text = zm;
        }
    }




    protected override void Update()
    {
        base.Update();
        txtReload.text = currentAmmo + "/" + AmmoPerClip;
        ShowMessage("Reloading", "Is Reloading: " + Convert.ToInt32(isReloading));
        ShowMessage("MiniReloading", "Is Adjusting Gun: " + Convert.ToInt32(isMiniReloading));

        if (Target != null && (Target.GetComponent<Enemy>() && Target.GetComponent<Enemy>().Alive))
        {
            if (Physics.Raycast(ScopeCamera.transform.position, ScopeCamera.transform.forward))
                distanceToTarget = Vector3.Distance(BulletSpawnLocation.position, Target.transform.position);
            else
                distanceToTarget = 0;
            txtDistanceToTarget.text = Mathf.Round(distanceToTarget) + "m";
        }
        else
        {
            txtDistanceToTarget.text = "";
        }


        if (IsUserControllable)
        {
            ZoomPinchControls(false);
            if (!isReloading)
                RotationControls(false, true);
            //Time Freeze output
            ShowMessage("TimeFreeze", "Time Freeze : " + Convert.ToInt32(InTimeFreezeMode));

            if (isReloading)
            {
                ReloadQuicktime();
            }


            List<LeanFinger> fingers = LeanTouch.GetFingers(false);

            if (fingers.Count > 0 && !isMiniReloading && CanAutoFire)
            {
                if (LeanGesture.GetScreenDelta().magnitude > 0 || isZooming)
                {
                    panning = true;
                }

                Ray ray = fingers[0].GetRay();
                RaycastHit hit = default(RaycastHit);
                if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer(ScopeLayerName)) && !panning)
                {
                    //Check if we should exit scope mode
                    if (hit.transform.gameObject.layer != LayerMask.NameToLayer(TouchableLayerName))
                    {
                        fireTime += Time.deltaTime;
                        if (fireTime >= TimeBetweenShots)
                        {
                            OnFireBegin();
                            fireTime = 0;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Called when you first touch down on an arrow
    /// </summary>
    /// <param name="eventData"></param>
    public void OnClickBeginArrow(BaseEventData eventData)
    {
        PointerEventData p = (PointerEventData)eventData;
        overArrow = (p.pointerCurrentRaycast.gameObject.name == arrows[curArrow].name);
    }
}
