using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAniControl : MonoBehaviour
{
    public OpenBCIInput openBCIInput;
    public GameManager gameManager;
    public Animator anim;

    //float inputThreshold = 0; // Threshold for when input is accepted during task window (Should be removed when scene is setup to work with BCI)
    float latestValue = 0; // Used to keep track of value when user go above threshold and then back below
    float successThreshold = 0.7f; // This is the propotional normalized value for successful trial

    bool taskCompleted = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void AniControl(float currentValue)
    {
        float playTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime; //Value of 0 to 1 of the current playtime of animation
        if (!openBCIInput.useDiscreteInput)
        {
            if (currentValue > openBCIInput.classificationThreshold/*inputThreshold*/ && gameManager.inputWindow == InputWindowState.Open && !taskCompleted) // Change inputThreshold to openBCIInput.classificationThreshold
            {
                if (openBCIInput.thresholdActive)
                {
                    currentValue = (currentValue - openBCIInput.classificationThreshold) / (1 - openBCIInput.classificationThreshold);
                }
                latestValue = currentValue;
            }
            else
            {
                latestValue = 0.05f;
            }

            if (gameManager.inputWindow == InputWindowState.Open && !taskCompleted)
            {
                DoTaskContinuous(playTime, latestValue);
            }
        }
        else if (openBCIInput.useDiscreteInput)
        {
            if (currentValue > openBCIInput.classificationThreshold/*inputThreshold*/ && gameManager.inputWindow == InputWindowState.Open && !taskCompleted)
            {
                DoTaskDiscrete(playTime);
            }
        }

        if (gameManager.inputWindow == InputWindowState.Closed || taskCompleted)
        {
            TaskWindowClosed(playTime);
        }
    }

    public void OnInputWindowStateChanged(InputWindowState state)
    {
        if (state == InputWindowState.Closed)
        {
            UncompletedTask();
        }
    }

    private void CompletedTask()
    {
        taskCompleted = true;
    }
    private void UncompletedTask()
    {
        taskCompleted = false;
    }

    #region Window close or task complete
    private void TaskWindowClosed(float playTime)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)"))
        {
            anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
        }
    }

    #endregion

    #region continuous input
    private void DoTaskContinuous(float playTime, float latestValue)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle") && latestValue > 0.05f)
        {
            anim.SetBool("Open", true);
            anim.Play("HandSqueeze(Close)", 0);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)"))
        {
            if (playTime > successThreshold)
            {
                CompletedTask();
            }
            else if (playTime > latestValue)
            {
                anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Open)"))
        {
            if (playTime >= 1 - latestValue)
            {
                anim.Play("HandSqueeze(Close)", 0, 1 - playTime);
            }
        }
    }
    #endregion

    #region discrete input
    private void DoTaskDiscrete (float playTime)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
        {
            anim.Play("HandSqueeze(Close)", 0);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)") && playTime > successThreshold)
        {
            CompletedTask();
        }
    }
    #endregion
}
