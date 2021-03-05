using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ContinuousInput : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    private Rigidbody characterBody;

    public OpenBCIInput Threshold;
    


    
    // Start is called before the first frame update
    void Start()
    {
        Threshold = Threshold.GetComponent<OpenBCIInput>();

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
        if (Threshold.thresholdActive == false)
            { 
             Vector3 force = new Vector3(0, value, 0);
             characterBody.AddForce(force, ForceMode.Impulse);
             Debug.Log(value.ToString());
            }
        else
            {
            if(Threshold.classificationThreshold < value)
                {
                float NormalizedForce = (value-Threshold.classificationThreshold)/(1-Threshold.classificationThreshold);
                Vector3 force = new Vector3(0, NormalizedForce, 0);
                characterBody.AddForce(force, ForceMode.Impulse);
                Debug.Log(value.ToString());
                Debug.Log(NormalizedForce.ToString());
            }
            }
    }

}