using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ContinuousInput : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    private Rigidbody characterBody;

    public OpenBCIInput openBCIInput;




    // Start is called before the first frame update
    void Start()
    {

        if (character == null)
        {
            Debug.LogError("Missing character to control in CharacterController");
        }

        characterBody = character.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBCIEvent(float value)
    {
        Debug.Log(openBCIInput);
        if (openBCIInput.useDiscreteInput == true) 
        {
            return;
        }
        if (openBCIInput.thresholdActive == false)
        { 
             Vector3 force = new Vector3(0, value, 0);
             characterBody.AddForce(force, ForceMode.Impulse);
             Debug.Log(value.ToString());
        }
        else
        {
            if(openBCIInput.classificationThreshold < value)
            {
                float NormalizedForce = (value- openBCIInput.classificationThreshold)/(1- openBCIInput.classificationThreshold);
                Vector3 force = new Vector3(0, NormalizedForce, 0);
                characterBody.AddForce(force, ForceMode.Impulse);
            }
        }
    }

}