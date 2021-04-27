using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLogs : MonoBehaviour
{
    public GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            gameManager.EndGame();
        }
    }
}
