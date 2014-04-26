using UnityEngine;
using System.Collections;

public class MoveMask : MonoBehaviour {
    private float spinSpeed = -100;
    private float moveSpeed = -5f;

    private bool moving = false;


    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            moving = true;
        }
        if (moving) {
            Vector3 rot = transform.localRotation.eulerAngles;
            rot.z += spinSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(rot);

            Vector3 pos = transform.localPosition;
            pos.x += moveSpeed * Time.deltaTime;
            transform.localPosition = pos;
        }
    }
}
