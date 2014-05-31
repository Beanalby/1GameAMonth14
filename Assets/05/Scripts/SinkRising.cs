using UnityEngine;
using System.Collections;

public class SinkRising : MonoBehaviour {
    public Vector3 direction = Vector3.up;

    public float risingDistance = 5;
    public float risingDuration = 1f;

    private float risingStart = -1;
    private bool isTriggered = false;

    private Vector3 startPos, endPos;

    public void Start() {
        startPos = transform.position;
        endPos = startPos + (direction * risingDistance);
    }

    public void Update() {
        if (risingStart != -1) {
            float percent = (Time.time - risingStart) / risingDuration;
            if (percent >= 1) {
                transform.position = endPos;
                risingStart = -1;
            } else {
                transform.position = Vector3.Lerp(startPos, endPos, percent);
            }
        }
    }
    public void Triggered() {
        if (!isTriggered) {
            risingStart = Time.time;
            isTriggered = true;
        }
    }
}
