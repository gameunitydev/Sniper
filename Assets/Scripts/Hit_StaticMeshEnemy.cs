using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_StaticMeshEnemy : AS_BulletHiter
{


    public override void OnHit(RaycastHit hit, AS_Bullet bullet)
    {
        AddAudio(hit.point);
        //Halt the enemy, and have the enemy actually take damage after the cool vfx.
        //(The begin event for the action camera is called in the base class's OnHit)
        GetComponent<Enemy>().Speed = 0;
        ActionCam.EndAction += () =>
        {
            GetComponent<Enemy>().OnBulletHit(bullet.Damage);
        };
        base.OnHit(hit, bullet);
    }


}
