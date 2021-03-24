using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAniControl : MonoBehaviour
{
    public Animator anim;

    float lastValue = 0;
    float tempVal = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("Close", true);
        anim.SetBool("Open", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            tempVal = 0.8f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            tempVal = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            tempVal = 0.25f;
        }

        AniControl(tempVal);
        Debug.Log(tempVal);
    }

    public void AniControl(float currentValue)
    {
        // Make "tempThreshold" into "openBCIInput.classificationThreshold" when moving to scene with bci input
        float playTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime; //Value of 0 to 1 of the current playtime of animation
        // below 0.5 is hand closing ---- abouve 0.5 is hand closing

        float tempThreshold = 0.5f; //Threshold for when input is accepted
        currentValue = (currentValue - tempThreshold) / (1 - tempThreshold); //Proportional value in range of threshold to 1

        // If propotional value is 0.8 correspond to 0.4 in playTime value
        // Need to check if playTime is less then the propotional value, if it is then keep playing handclose ani, 
        // but if playTime is more then, then switch to other ani until playTime over 0.5 correspond with the proportional value

        /* 
         * Switch animation to playtime over 0.5 with 
         * anim.SetBool("Open", false);
         * anim.SetBool("Close", true);
         * 
         * Animation under 0.5 is
         * anim.SetBool("Open", true);
         * anim.SetBool("Close", false);
         */
        Debug.Log("Running: C " + currentValue + " t " + tempThreshold + " p " + playTime + " l " + lastValue);
        if (lastValue <= currentValue)
        {
            lastValue = currentValue;
            if (playTime < 0.5f)
            {
                if (playTime < currentValue / 2)
                {
                    anim.SetBool("Open", false);
                    anim.SetBool("Close", true);
                }
                else if (playTime > currentValue / 2)
                {
                    anim.SetBool("Open", true);
                    anim.SetBool("Close", false);
                }
            }
        }
        else
        {
            lastValue = currentValue;
            if (playTime < 0.5f)
            {
                if (playTime < lastValue / 2)
                {
                    anim.SetBool("Open", false);
                    anim.SetBool("Close", true);
                }
                else if (playTime > lastValue / 2)
                {
                    anim.SetBool("Open", true);
                    anim.SetBool("Close", false);
                }
            }
        }
    }
}
