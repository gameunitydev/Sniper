using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontlineCommando : SniperController
{

    public override void OnFireBegin()
    {
        base.OnFireBegin();
        FireCentered();
    }

    protected override void Update()
    {
        base.Update();
        if (IsUserControllable)
        {
            RotationControls();
            ShowMessage("Fire", "Fire : " + Convert.ToInt32(IsFiring));
            if (IsFiring)
            {
                fireTime += Time.deltaTime;
                if (fireTime >= TimeBetweenShots)
                {
                    FireCentered();
                    fireTime = 0;
                }
            }
        }
    }
}
