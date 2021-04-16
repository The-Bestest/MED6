using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAniControl : MonoBehaviour
{
    public OpenBCIInput openBCIInput;
    public GameManager gameManager;
    public Animator anim;
    public GameObject balloon;
    public Animator balloonAnim;

    //float inputThreshold = 0; // Threshold for when input is accepted during task window (Should be removed when scene is setup to work with BCI)

    bool taskCompleted = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        balloonAnim = balloon.GetComponent<Animator>();
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

                DoTaskContinuous(playTime, currentValue);
            }
            else if ((gameManager.inputWindow == InputWindowState.Closed || taskCompleted) /*&& !anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle")*/)
            {
                TaskWindowClosed(playTime);
                if (taskCompleted && !balloonAnim.GetCurrentAnimatorStateInfo(0).IsName("BalloonAni"))
                {
                    balloon.SetActive(false);
                }
            }
            else if (currentValue < openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)"))
                {
                    anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
                    balloonAnim.Play("AntiBalloonAni", 0, 1 - playTime);
                }
            }
        }
        else if (openBCIInput.useDiscreteInput)
        {
            if (currentValue > openBCIInput.classificationThreshold/*inputThreshold*/ && gameManager.inputWindow == InputWindowState.Open && !taskCompleted)
            {
                DoTaskDiscrete(playTime);
            }
            else if (taskCompleted && !balloonAnim.GetCurrentAnimatorStateInfo(0).IsName("BalloonAni"))
            {
                balloon.SetActive(false);
            }
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
        Debug.Log("Popped");
        taskCompleted = true;
    }
    private void UncompletedTask()
    {
        balloon.SetActive(true);
        taskCompleted = false;
    }

    #region Window close or task complete
    private void TaskWindowClosed(float playTime)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)"))
        {
            balloonAnim.SetBool("Open", false);
            anim.SetBool("Open", false);
            anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
            balloonAnim.Play("AntiBalloonAni", 0, 1 - playTime);
        }
    }

    #endregion

    #region continuous input
    private void DoTaskContinuous(float playTime, float currentValue)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
        {
            anim.SetBool("Open", true);
            //anim.Play("HandSqueeze(Close)", 0);
            balloonAnim.SetBool("Open", true);
            //balloonAnim.Play("BalloonAni", 0);

            Debug.Log("Hand is closing, opening hand at: " + playTime);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)"))
        {
            if (playTime > openBCIInput.terminalThreshold)
            {
                balloonAnim.SetBool("Open", false);
                anim.SetBool("Open", false);
                CompletedTask();
            }
            else if (playTime > currentValue)
            {
                anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
                balloonAnim.Play("AntiBalloonAni", 0, 1 - playTime);
            }
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Open)"))
        {
            if (playTime >= 1 - currentValue)
            {
                anim.Play("HandSqueeze(Close)", 0, 1 - playTime);
                balloonAnim.Play("BalloonAni", 0, 1 - playTime);
            }
        }
    }
    #endregion

    #region discrete input
    private void DoTaskDiscrete (float playTime)
    {
        if (!taskCompleted)//anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
        {
            Debug.Log("Testing");
            anim.Play("HandSqueeze(Close)", 0);
            balloonAnim.Play("BalloonAni", 0);
            CompletedTask();
        }
    }
    #endregion
}
