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

    //float inputThreshold = 0; // Threshold for when input is accepted during task window (Should be removed when scene is setup to work with BCI)

    private void Start()
    {
        handAnim = GetComponent<Animator>();
        balloonAnim = balloon.GetComponent<Animator>();
        balloonRend = balloon.GetComponentInChildren<Renderer>().material;
        balloonChild = balloon.transform.GetChild(0).gameObject;
    }

    private void Update()
    {

    }

    public void AniControl(float currentValue)
    {
        float playTime = handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime; //Value of 0 to 1 of the current playtime of animation
        if (!openBCIInput.useDiscreteInput && currentValue > openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open && balloonChild.activeSelf)
        {
            if (openBCIInput.thresholdActive)
            {
                currentValue = (currentValue - openBCIInput.classificationThreshold) / (1 - openBCIInput.classificationThreshold);
            }
            DoContinuousTask(playTime, currentValue);
        }
        else if (openBCIInput.useDiscreteInput && currentValue > openBCIInput.terminalThreshold && gameManager.inputWindow == InputWindowState.Open && balloonChild.activeSelf && handAnim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
        {
            DoDiscreteTask();
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
    /*if (playTime >= 1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
    {
        return;
    }
    Debug.Log("This is the first one: " + playTime);
    if (!openBCIInput.useDiscreteInput)
    {
        if (currentValue > openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open && !taskCompleted) // Change inputThreshold to openBCIInput.classificationThreshold
        {
            if (openBCIInput.thresholdActive)
            {
                currentValue = (currentValue - openBCIInput.classificationThreshold) / (1 - openBCIInput.classificationThreshold);
            }

            DoTaskContinuous(playTime, currentValue);
        }
        else if ((gameManager.inputWindow == InputWindowState.Closed || taskCompleted) /*&& !anim.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"))
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
                Debug.Log("Animation changed 1:" + (1 - playTime));
            }
        }
    }
    else if (openBCIInput.useDiscreteInput)
    {
        if (currentValue > openBCIInput.classificationThreshold && gameManager.inputWindow == InputWindowState.Open && !taskCompleted)
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
    if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Close)")) // This looks like a problem
    {
        if (balloonAnim.GetBool("Open"))
        {    
            balloonAnim.SetBool("Open", false);
            anim.SetBool("Open", false);
        }
        anim.Play("HandSqueeze(Open)", 0, 1 - playTime);
        balloonAnim.Play("AntiBalloonAni", 0, 1 - playTime);
        Debug.Log("Animation changed 2:" + (1 - playTime));
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
            Debug.Log("Animation changed 3:" + (1- playTime));
        }
    }
    else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandSqueeze(Open)"))
    {
        if (playTime >= 1 - currentValue)
        {
            anim.Play("HandSqueeze(Close)", 0, 1 - playTime);
            balloonAnim.Play("BalloonAni", 0, 1 - playTime);
            Debug.Log("Animation changed 4:" + (1 - playTime));
        }
    }
}
#endregion
*/
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

        if(playTime > openBCIInput.terminalThreshold)
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
    void DoDiscreteTask()
    {
        Debug.Log("Discrete Commenced");
        handAnim.Play("HandSqueeze(Close)", 0);
        balloonAnim.Play("BalloonAni", 0);
        StartCoroutine(DiscreteBalloonPop());
    }
    #endregion

    IEnumerator DiscreteBalloonPop()
    {
        yield return new WaitForSeconds(handAnim.GetCurrentAnimatorClipInfo(0).Length);
        BalloonPop();
    }
}
