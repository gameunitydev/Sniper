using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collection of UI Methods for button presses, etc.
/// </summary>
public class UIMethods : MonoBehaviour
{

    private bool timeFreeze = false;


    private SniperController sniper;

    private void Start()
    {
        sniper = GameObject.FindWithTag("Player").GetComponent<SniperController>();

    }


    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Called when time freeze button is pressed. 
    /// Calles the TimeFreeze event, which is handled by sniper and enemies
    /// </summary>
    public void OnTimeFreeze()
    {


        if (!sniper.InTimeFreezeMode)
            sniper.OnTimeFreezeBegin();
        else
            sniper.OnTimeFreezeEnd();
    }


    public void OnFireBegin()
    {
        sniper.OnFireBegin();
    }

    public void OnFireEnd()
    {
        sniper.OnFireEnd();
    }


    public void OnToggleScope()
    {
        sniper.ToggleScope();
    }

    public void BeginInstinct()
    {
        sniper.OnInstinctBegin();
    }

    public void BeginDeadEye()
    {
        sniper.BeginDeadEye();
    }

    public void BeginSatelliteScan()
    {
        SniperFury sf = (SniperFury)sniper;
        sf.OnSatelliteScan();
    }
}
