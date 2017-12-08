using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains methods for the demo of showcasing different sniper controls
/// </summary>
public class DemoUIMethods : MonoBehaviour
{

    /// <summary>
    /// Sibling index of this object
    /// </summary>
    int sibIndex;

    /// <summary>
    /// Parent of all enemies
    /// </summary>
    public GameObject enemies;


    private void Start()
    {
        sibIndex = transform.GetSiblingIndex();
    }

    /// <summary>
    /// Activate the sniper based on this object's sibiling index
    /// </summary>
    public void ActivateSniperFromButton()
    {
        GameObject sniper = GameObject.Find("Snipers").transform.GetChild(sibIndex).gameObject;
        sniper.SetActive(true);

        //Activate all enemies
        // enemies.SetActive(true);

        //Update the enemies' health. If sniper arena, give them more health to demo supercharge. Else, just make them have 1 health
        int eHealth = 1;
        if (sniper.GetComponent<SniperArenaPVPArmyShooter>())
            eHealth = 6;

        /*  foreach (Transform enemy in enemies.transform)
          {
              if (enemy.GetComponent<Enemy>())
                  enemy.GetComponent<Enemy>().ResetHealth(eHealth);
          }*/

        GameObject.Find("UICam").GetComponent<Camera>().enabled = false;

        //To prevent conflicts with Advanced Sniper
        transform.parent.parent.gameObject.SetActive(false);


        RussiaMission.Instance.StartMission();
    }


    /// <summary>
    /// Deactivate sniper based on object's sibling index
    /// </summary>
    public void DeactivateSniperFromBackButton()
    {
        GameObject.FindWithTag("Player").SetActive(false);


        //Deactivate all enemies
        //enemies.SetActive(false);

        //Turn on the panel again 
        transform.parent.GetChild(1).gameObject.SetActive(true);

        //To prevent conflicts with Advanced Sniper
        GameObject.Find("UICam").GetComponent<Camera>().enabled = true;

    }


    /// <summary>
    /// Set all sniper primary hands to left handed, if provided argument is true
    /// </summary>
    public void SetPrimaryHand(bool isLeftHanded)
    {
        foreach (SniperController sc in GameObject.Find("Snipers").transform.GetComponentsInChildren<SniperController>(true))
        {
            if (isLeftHanded)
                sc.PrimaryHandMode = PrimaryHand.LeftHanded;
            else
                sc.PrimaryHandMode = PrimaryHand.RightHanded;
        }
        //Also swap the back button's position
        SniperController.FlipRectTransform(transform.parent.parent.GetChild(0).GetComponent<RectTransform>());
    }
}
