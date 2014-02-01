using UnityEngine;
using System.Collections;

public class CarSpeed : MonoBehaviour {
    private float baseSpeed;
    private PlayerController pc;

    public void Start() {
        pc = GetComponent<PlayerController>();
        baseSpeed = pc.speed;
    }

    public void Update() {
        if (Input.GetButton("Jump")) {
            pc.speed = baseSpeed + 3;
        } else {
            pc.speed = baseSpeed;
        }
    }
}
