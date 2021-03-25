using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAniControl : MonoBehaviour
{
    //public OpenBCIInput openBCIInput;
    public Animator anim;

    float lastValue = 0;
    float tempVal = 0.1f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    public void AniControl(float currentValue)
    {
        //currentValue = (currentValue - openBCIInput.classificationThreshold) / (1 - openBCIInput.classificationThreshold);
        float playTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime; //Value of 0 to 1 of the current playtime of animation

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
        {
            anim.Play("HandSqueeze(Close)", 0);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)"))
        {
            if (playTime > currentValue)
            {
                anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Open)"))
        {
            if (playTime >= 1 - currentValue)
            {
                anim.Play("HandSqueeze(Close)", 0, 1 - playTime);
            }
        }
    }
}
