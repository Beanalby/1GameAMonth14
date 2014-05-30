using UnityEngine;
using System.Collections;

public class SinkBallTrail : MonoBehaviour {
    TrailRenderer trail;

    public void Start() {
        trail = GetComponent<TrailRenderer>();
    }

    public void EnableShot() {
        trail.enabled = false;
    }
    public void DisableShot() {
        trail.enabled = true;
    }
}
