using UnityEngine;
using System.Collections;

public class SinkBall : MonoBehaviour {
    float startSpeed = 0f;
    float maxMouseDist = 2;
    float maxShotPower = 15;
    Rigidbody rb;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        Physics.gravity *= 10;
        
        rb.velocity = new Vector3(startSpeed, 0, startSpeed);
    }

    public void Update() {
        if (Input.GetButtonDown("Fire1")) {
            ShootBall();
        }
    }
    private void ShootBall() {
        Vector3 shotInfo = GetShotInfo();
        rb.velocity = shotInfo * maxShotPower;
    }

    public Vector3 GetShotInfo() {
        // Find the cursor's point projected onto the ball's plane
        Plane ballPlane = new Plane(Vector3.up, transform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (!ballPlane.Raycast(mouseRay, out dist)) {
            Debug.LogError("Mouse ray doesn't intersect ball plane!");
            Debug.Break();
            return Vector3.zero;
        }
        Vector3 pos = mouseRay.GetPoint(dist);
        // note how far away the curor is from the ball
        float mouseDist = (pos - transform.position).magnitude;
        float lineDist = Mathf.Min(maxMouseDist, mouseDist) / maxMouseDist;
        return (transform.position - pos).normalized * lineDist;
    }
}
