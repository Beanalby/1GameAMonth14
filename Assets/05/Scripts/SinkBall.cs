using UnityEngine;
using System.Collections;

public class SinkBall : MonoBehaviour {
    public void Start() {
        Physics.gravity *= 10;
        GetComponent<Rigidbody>().velocity = new Vector3(3, 0, 3);
    }
}
