using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMission
{

    /// <summary>
    /// Unique mission id (for some internal settings)
    /// </summary>
    float MissionID { get; }
    /// <summary>
    /// Unique mission name (can be used as display name on canvas, etc..)
    /// </summary>
    string MissionName { get; }

    /// <summary>
    /// On mission start event
    /// </summary>
    void OnMissionStart();
    /// <summary>
    /// On mission success event
    /// </summary>
    void OnMissionSuccess();

    /// <summary>
    /// Restart current mission event
    /// </summary>
    void RestartMission();

    /// <summary>
    /// On mission fail event
    /// </summary>
    void OnMissionFail();



}
