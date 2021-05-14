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

    public AudioSource pop;

    bool success = false;

    float playTime;

    GameObject balloonChild;

    private void Start()
    {
        handAnim = GetComponent<Animator>();
        balloonAnim = balloon.GetComponent<Animator>();
        balloonRend = balloon.GetComponentInChildren<Renderer>().material;
        balloonChild = balloon.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        playTime = handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime; //Value of 0 to 1 of the current playtime of animation
        if (playTime >= 0.99f && !openBCIInput.useDiscreteInput)
        {
            BalloonPop();
        }
        if (gameManager.inputWindow == InputWindowState.Closed || !balloonChild.activeSelf)
        {
            if (playTime > 0)
            {
                handAnim.SetFloat("Direction", -1);
                balloonAnim.SetFloat("Direction2", -1);
            }
            else
            {
                handAnim.SetFloat("Direction", 0);
                balloonAnim.SetFloat("Direction2", 0);
            }
        }
    }

    public void AniControl(float currentValue)
    {
        if (!openBCIInput.useDiscreteInput && currentValue > openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open && balloonChild.activeSelf)
        {
            //if (openBCIInput.thresholdActive)
            //{
                currentValue = (currentValue - openBCIInput.classificationThreshold) / (openBCIInput.terminalThreshold - openBCIInput.classificationThreshold);
                if (currentValue > 0)
                {
                    DoContinuousTask(playTime, currentValue);
                }
            //}
            //else
            //{
            //    DoContinuousTask(playTime, currentValue);
            //}
        }
        else if (!openBCIInput.useDiscreteInput && currentValue < openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open && balloonChild.activeSelf && playTime > 0)
        {
            handAnim.SetFloat("Direction", -1);
            balloonAnim.SetFloat("Direction2", -1);
        }
        else if (openBCIInput.useDiscreteInput && ((currentValue > openBCIInput.terminalThreshold && gameManager.inputWindow == InputWindowState.Open) || success) && balloonChild.activeSelf)
        {
            DoDiscreteTask(playTime);
        }
    }
    void DoContinuousTask(float playTime, float currentValue)
    {
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
        //handAnim.Play("HandSqueeze(Close)", 0);
        //balloonAnim.Play("BalloonAni", 0);
    }

    void BalloonPop()
    {
        balloonChild.SetActive(false);
        pop.Play(0);
    }

    #region discrete input
    void DoDiscreteTask(float playTime)
    {
        Debug.Log("Discrete Commenced");
        handAnim.SetFloat("Direction", 1);
        balloonAnim.SetFloat("Direction2", 1);

        success = true;

        if (playTime > 1)
        {
            BalloonPop();
            success = false;
        }
    }
    #endregion
}
