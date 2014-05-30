using UnityEngine;
using System.Collections;

public class SinkBall : MonoBehaviour {
    const float VELOCITY_CUTOFF = .09f;
    public GameObject guide, trail;

    float startSpeed = 0f;
    float maxMouseDist = 2;
    float maxShotPower = 35;
    Rigidbody rb;

    private bool isMoving = false;
    public bool CanShoot {
        get { return !isMoving; }
    }

    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;
        Physics.gravity *= 10;
        
        rb.velocity = new Vector3(startSpeed, 0, startSpeed);
    }

    public void Update() {
        if (Input.GetButtonDown("Fire1") && CanShoot) {
            ShootBall();
        }

        // if it's going super-slow, just stop it to get on with the next shot
        if (rb.velocity.sqrMagnitude != 0 && rb.velocity.sqrMagnitude < VELOCITY_CUTOFF) {
            rb.velocity = Vector3.zero;
        }
        
        // if we've just stopped moving, let things know
        if (isMoving && rb.velocity.sqrMagnitude == 0) {
            isMoving = false;
            guide.SendMessage("EnableShot");
            trail.SendMessage("EnableShot");
        }
    }

    private void ShootBall() {
        Vector3 shotInfo = GetShotInfo();
        rb.velocity = shotInfo * maxShotPower;
        isMoving = true;
        guide.SendMessage("DisableShot");
        trail.SendMessage("DisableShot");
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
