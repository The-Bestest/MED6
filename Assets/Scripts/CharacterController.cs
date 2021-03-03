using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject character;

    private Rigidbody characterBody;

    // Start is called before the first frame update
    void Start()
    {
        if(character == null)
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
        Vector3 force = new Vector3(0, value, 0);
        characterBody.AddForce(force, ForceMode.Impulse);
        Debug.Log(value.ToString());
    }

}
