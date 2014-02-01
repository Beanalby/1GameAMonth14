using UnityEngine;
using System.Collections;

public class ShipJump : MonoBehaviour {

    private float jumpStrength = 3;

    private PlayerController pc;

    public void Start() {
        pc = GetComponent<PlayerController>();
    }

    public void Update() {
        if (pc.canControl && Input.GetButtonDown("Jump")) {
            Vector3 dir = transform.TransformDirection(Vector3.forward);
            dir.y = 1f;
            dir.Normalize();
            dir *= jumpStrength;
            pc.Fling(dir, true);
        }
    }
}
