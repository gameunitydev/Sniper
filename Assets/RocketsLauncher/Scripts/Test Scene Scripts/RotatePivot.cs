using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script used only in the rockets test scene
public class RotatePivot : MonoBehaviour {    
    public Transform pivot;
    void Update()
    {
        pivot.eulerAngles += Vector3.up * Time.deltaTime * 30;
    }
}
