using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
    private GameObject target = null;
    private float distance = 3f;
    private float height = 1f;

    private float angleVelocity = 0;
    private float angularSmoothLag = 0.3f;
    private float heightVelocity = 0;
    private float heightSmoothLag = 0.3f;

    public void Update() {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target == null) {
                Debug.LogError("No player to follow");
                return;
            }
        }

        // adjust our rotation
        float targetAngle = target.transform.eulerAngles.y;
        float currentAngle = transform.eulerAngles.y;
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle,
            ref angleVelocity, angularSmoothLag);
        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        float targetHeight = target.transform.position.y + height;
        float currentHeight = transform.position.y;
        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight,
            ref heightVelocity, heightSmoothLag);

        Vector3 pos = target.transform.position;
        pos += currentRotation * Vector3.back * distance;
        pos.y = currentHeight;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
    }
}
