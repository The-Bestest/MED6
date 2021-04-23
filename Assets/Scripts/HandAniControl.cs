using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAniControl : MonoBehaviour
{
    public OpenBCIInput openBCIInput;
    public GameManager gameManager;
    public Animator handAnim;
    public GameObject balloon;
    public Animator balloonAnim;

    public Material balloonRend;

    GameObject balloonChild;

    private void Start()
    {
        handAnim = GetComponent<Animator>();
        balloonAnim = balloon.GetComponent<Animator>();
        balloonRend = balloon.GetComponentInChildren<Renderer>().material;
        balloonChild = balloon.transform.GetChild(0).gameObject;
    }

    public void AniControl(float currentValue)
    {
        float playTime = handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime; //Value of 0 to 1 of the current playtime of animation
        if (!openBCIInput.useDiscreteInput && currentValue > openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open && balloonChild.activeSelf)
        {
            if (openBCIInput.thresholdActive)
            {
                currentValue = (currentValue - openBCIInput.classificationThreshold) / (openBCIInput.terminalThreshold - openBCIInput.classificationThreshold);
            }
            DoContinuousTask(playTime, currentValue);
        }
        else if (openBCIInput.useDiscreteInput && currentValue > openBCIInput.terminalThreshold && gameManager.inputWindow == InputWindowState.Open && balloonChild.activeSelf)
        {
            DoDiscreteTask(playTime);
        }
        else if(gameManager.inputWindow == InputWindowState.Closed || !balloonChild.activeSelf)
        {
            if (playTime > 0)
            {
                handAnim.SetFloat("Direction", -1);
                balloonAnim.SetFloat("Direction2", -1);
            }
            else if (playTime < 0)
            {
                handAnim.SetFloat("Direction", 0);
                balloonAnim.SetFloat("Direction2", 0);
            }
            else
            {
                handAnim.SetFloat("Direction", 0);
                balloonAnim.SetFloat("Direction2", 0);
            }
        }
    }
    void DoContinuousTask(float playTime, float currentValue)
    {
        Debug.Log("Continuous Started");
        if (playTime < currentValue)
        {
            handAnim.SetFloat("Direction", 1);
            balloonAnim.SetFloat("Direction2", 1);
        }
        else if(playTime > currentValue)
        {
            handAnim.SetFloat("Direction", -1);
            balloonAnim.SetFloat("Direction2", -1);
        }

        if(playTime > 1)
        {
            BalloonPop();
        }
        //handAnim.Play("HandSqueeze(Close)", 0);
        //balloonAnim.Play("BalloonAni", 0);
    }

    void BalloonPop()
    {
        balloonChild.SetActive(false);
    }

    #region discrete input
    void DoDiscreteTask(float playTime)
    {
        Debug.Log("Discrete Commenced");
        handAnim.SetFloat("Direction", 1);
        balloonAnim.SetFloat("Direction2", 1);

        if (playTime > 1)
        {
            BalloonPop();
        }
    }
    #endregion
}
