using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BNG {
public class GameObjective : MonoBehaviour
{

    private Vector3 startPosition;
    private Quaternion startRotation;
    public GameObject networkedBall;
    public GameObject distanceThrown;
    public GameObject recordThrown;
    public float startPositionZ;
    public float distance = 0f;
    public float record = 0f;
    public bool newRecordInThisThrow = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = networkedBall.transform.position;
        startRotation = networkedBall.transform.rotation;
        startPositionZ = networkedBall.transform.position.z;
    }

    // Checks if the ball has been thrown
    void Update()
    {
        if (networkedBall.GetComponent<NetworkedGrabbable>().BeingHeld){
            startPositionZ = networkedBall.transform.position.z;
        }
        if (!networkedBall.GetComponent<NetworkedGrabbable>().BeingHeld){
            distance = networkedBall.transform.position.z - startPositionZ;
            distanceThrown.GetComponent<Text>().text = distance.ToString("F1");
        }
        if (distance > record && networkedBall.GetComponent<NetworkedGrabbable>().BeingHeld){
            record = distance;
            recordThrown.GetComponent<Text>().text = record.ToString("F1");
            if (!newRecordInThisThrow){
                recordThrown.GetComponent<AudioSource>().Play();
                newRecordInThisThrow = true;
                SuccessfulRecord();
            }
        }
    }

    // If the ball was thrown and lands on Grass or the ThrowArea it will be reset to its original place without velocity. If the thrown distance was bigger than the record it will be displayed.
    void OnTriggerEnter(Collider collider){
        if (!networkedBall.GetComponent<NetworkedGrabbable>().BeingHeld && collider.gameObject.layer == LayerMask.NameToLayer("Grass")){
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.transform.position = startPosition;
            this.transform.rotation = startRotation;
            startPositionZ = networkedBall.transform.position.z;
            distance = 0f;
            newRecordInThisThrow = false;
        }
    }

    void SuccessfulRecord() {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        startPositionZ = networkedBall.transform.position.z;
        distance = 0f;
        newRecordInThisThrow = false;  
    }
}
}
