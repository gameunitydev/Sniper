using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBarrageController : MonoBehaviour {

    //list of rockets that are ready to fire
    public List<RocketController> rockets;
    //target for the rockets
    public Transform target;

    //keys used to launch and reload the rockets
    [Header("Keyboard Keys")]

    public KeyCode launchAll;
    public KeyCode launchSequentially;
    public KeyCode reload;

    void Update ()
    {
        if (Input.GetKeyUp(launchSequentially))
            Launch(false);

        if(Input.GetKeyUp(launchAll))
            Launch(true);

        if (Input.GetKeyUp(reload))
            Reload();
    }

    //call this method to launch the rockets
    public void Launch(bool allAtOnce)
    {
        if (allAtOnce)
        {
            //shoot rockets all at once
            for (int index = 0; index < rockets.Count; index++)
            {
                //check if the rocket is back in the launch pod
                if (!rockets[index].enabled)
                {
                    //check if the rocket was reloaded
                    if (rockets[index].Loaded)
                    {
                        //set the target and enable the rocket controller script
                        rockets[index].targetTransform = target;
                        rockets[index].enabled = true;

                        //play the launch audio
                        rockets[index].GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
        else
        {
            //shoot rockets sequentially
            StartCoroutine(LaunchCoroutine());
        }
    }

    //method used to reload
    public void Reload()
    {
        for (int index = 0; index < rockets.Count; index++)
        {
            if (!rockets[index].Loaded)
            {
                rockets[index].Loaded = true;
            }
        }
    }

    //coroutine used to sequentially launch the rockets
    private IEnumerator LaunchCoroutine()
    {
        for (int index = 0; index < rockets.Count; index++)
        {
            if (!rockets[index].enabled)
            {
                //check if the rocket was reloaded
                if (rockets[index].Loaded)
                {
                    //set the target and enable the rocket controller script
                    rockets[index].targetTransform = target;
                    rockets[index].enabled = true;

                    //play the launch audio
                    rockets[index].GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
}
