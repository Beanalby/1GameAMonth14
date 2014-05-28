using UnityEngine;
using System.Collections;

public class SinkBall : MonoBehaviour {
    float startSpeed = 15f;
    Rigidbody rb;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        Physics.gravity *= 10;
        
        rb.velocity = new Vector3(startSpeed, 0, startSpeed);
    }
}
