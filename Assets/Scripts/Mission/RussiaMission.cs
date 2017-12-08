using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gaia;
using UnityEngine.UI;

public class RussiaMission : Singleton<RussiaMission>, IMission
{
    #region Fields

    #region Public
    [Header("Mission settings")]
    public float missionId;
    public string missionName;

    public float missionDuration; // Timer for mission duration in seconds

    public float missionStartDelay;
    public float pickupPathDuration;
    public float pickupSinkSpeed;
    public float pickupSinkYDepth;

    [Space]
    public int maximumBarrelsToDestroy;

    [Space]
    public DOTweenPath pickupMovementPath;
    public GameObject pickupGameObject;

    [Space]
    [Header("Mission UI")]
    public Text missionTimerUI;
    public CanvasGroup missionPanelCG; // Mission panel canvas group component.
    public Text missionMessageUI; // Show mission message

    public GraphicRaycaster sniperPanelCG; // Block user input

    [Space]
    public GameObject explosionEffect;

    #endregion

    private MissionTimer missionTimer;
    private int numberOfDestroyedBarrels;

    #endregion

    #region Unity Methods

    private void Start()
    {
        pickupMovementPath.duration = pickupPathDuration;
        numberOfDestroyedBarrels = maximumBarrelsToDestroy;



        missionTimer = new MissionTimer();
        missionTimer.Reset(missionDuration);


        EnableSniperCanvas(false);

    }

    private void Update()
    {
        missionTimer.UpdateTimer(missionTimerUI);
    }

    #endregion

    #region Public Methods

    public void EnableSniperCanvas(bool enable)
    {
        sniperPanelCG.enabled = enable;
    }


    public void StartMission()
    {
        StartCoroutine(StartMissionDelayedIE(missionStartDelay));
    }


    public void StartSinking()
    {
        pickupGameObject.transform.DOMoveY(pickupSinkYDepth, pickupSinkSpeed).OnComplete(OnMissionFail);
    }

    public void CheckIsMissionFinished()
    {
        numberOfDestroyedBarrels--;


        if (numberOfDestroyedBarrels == 0)
        {
            OnMissionSuccess();
        }
    }

    public void ShowMissionMessage(string message)
    {
        missionMessageUI.text = message;
    }

    #endregion

    #region IMission


    public float MissionID
    {
        get
        {
            return missionId;
        }
    }

    public string MissionName
    {
        get
        {
            return missionName;
        }
    }

    public void OnMissionFail()
    {
        missionMessageUI.text = "Mission fail!";

        // Stop mission timer
        missionTimer.Stop();

        // Print debug error

        Debug.Log("Mission Failed!");
    }

    public void OnMissionStart()
    {

        // missionMessageUI.text = "Mission started!";

        // Start moving pickup
        pickupMovementPath.DOPlay();
        // Start mission timer
        missionTimer.Start();
        // Show mission panel canvas group
        missionPanelCG.alpha = 1;
        // Disable explosion effect
        explosionEffect.SetActive(false);

        // Enable picku (car object)
        pickupGameObject.SetActive(true);
    }

    public void OnMissionSuccess()
    {

        missionMessageUI.text = "Mission success!";

        // Stop mission timer
        missionTimer.Stop();

        // Play explosion effect
        explosionEffect.SetActive(true);
        explosionEffect.transform.position = pickupGameObject.transform.position;

        // Disable pickup (car) object
        pickupGameObject.SetActive(false);


        // Mission success message
        Debug.Log("Mission success");
    }

    public void RestartMission()
    {
        missionMessageUI.text = "";

        // Reset mission timer
        missionTimer.Reset(missionDuration);
        // Hide mission panel canvas group
        missionPanelCG.alpha = 0;
        // Disabe explosion effect
        explosionEffect.SetActive(false);


        numberOfDestroyedBarrels = maximumBarrelsToDestroy;

        // Enable picku (car object)
        pickupGameObject.SetActive(true);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start current mission with delay
    /// </summary>
    /// <param name="delay">Delay before mission is started</param>
    /// <returns></returns>
    private IEnumerator StartMissionDelayedIE(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        OnMissionStart();
    }




    #endregion

}


/// <summary>
/// Mission timer (count remaining time)
/// </summary>
public class MissionTimer
{
    #region Fields

    #region Private
    private float currentTime = 120f;
    private float minutes;
    private float seconds;
    private bool stop = true;
    #endregion

    #endregion

    #region Public Methods

    public void Stop()
    {
        stop = true;
    }

    public void Start()
    {
        stop = false;
    }

    public void Reset(float initialTime)
    {
        currentTime = initialTime;
        stop = true;
    }

    /// <summary>
    /// Update mission timer and display mission timer on UI text component
    /// </summary>
    /// <param name="timerText"></param>
    public void UpdateTimer(Text timerText)
    {
        UpdateTimer();

        if (timerText != null)
        {
            timerText.text = GetCurrentTime();
        }

    }

    /// <summary>
    /// Update mission timer
    /// </summary>
    public void UpdateTimer()
    {
        if (stop) return;

        currentTime -= Time.deltaTime;

        minutes = Mathf.Floor(currentTime / 60);
        seconds = currentTime % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            stop = true;
            minutes = 0;
            seconds = 0;

            RussiaMission.Instance.OnMissionFail();
        }

    }

    /// <summary>
    /// Current time
    /// </summary>
    /// <returns>Formated mission time remaining</returns>
    public string GetCurrentTime()
    {
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    #endregion

}
