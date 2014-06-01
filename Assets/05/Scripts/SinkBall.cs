using UnityEngine;
using System.Collections;

public class SinkBall : MonoBehaviour {
    const float VELOCITY_CUTOFF = .09f;
    public GameObject guide, trail, shadow;
    public AudioClip[] cupSounds;
    public AudioClip shootSound;

    float startSpeed = 0f;
    float maxMouseDist = 4;
    float maxShotPower = 70;
    Rigidbody rb;

    private Vector3 shadowOffset;
    private Quaternion shadowRotation;

    private bool isMoving = false;
    private bool isControllable = true;
    public bool CanShoot {
        get { return !isMoving && isControllable; }
    }

    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1000;        
        rb.velocity = new Vector3(startSpeed, 0, startSpeed);
        shadowOffset = shadow.transform.position - transform.position;
        shadowRotation = shadow.transform.rotation;
    }

    public void Update() {
        // maintain the blob shadow's position and rotation above us
        shadow.transform.position = transform.position + shadowOffset;
        shadow.transform.rotation = shadowRotation;

        if (Input.GetButtonDown("Fire1") && CanShoot) {
            ShootBall();
        }

        // if it's going super-slow, just stop it to get on with the next shot
        if (rb.velocity.sqrMagnitude != 0 && rb.velocity.sqrMagnitude < VELOCITY_CUTOFF) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        // if we've just stopped moving, let things know
        if (isMoving && rb.velocity.sqrMagnitude == 0) {
            isMoving = false;
            guide.SendMessage("EnableShot");
            trail.SendMessage("EnableShot");
        }
    }

    private void ShootBall() {
        Vector3 shotInfo;
        float percent;
        GetShotInfo(out shotInfo, out percent);
        /// scale power .25-1 between distance .5-1, so the smaller
        /// .5 controls the lower .25 of power.  This gives more
        /// control at lower power.
        if (percent > .5f) {
            percent = Mathf.Lerp(.25f, 1, 2 * (percent - .5f));
        } else {
            percent = Mathf.Lerp(0, .25f, 2 * percent);
        }
        rb.velocity = shotInfo * percent * maxShotPower;
        isMoving = true;
        guide.SendMessage("DisableShot");
        trail.SendMessage("DisableShot");
        SinkDriver.instance.Stroked();
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
    }

    public void GetShotInfo(out Vector3 dir, out float percent) {
        // Find the cursor's point projected onto the ball's plane
        Plane ballPlane = new Plane(Vector3.up, transform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (!ballPlane.Raycast(mouseRay, out dist)) {
            Debug.LogError("Mouse ray doesn't intersect ball plane!");
            Debug.Break();
            dir = Vector3.zero; percent = 0;
            return;
        }
        Vector3 pos = mouseRay.GetPoint(dist);
        dir = (transform.position - pos).normalized;
        // note how far away the curor is from the ball
        float mouseDist = (pos - transform.position).magnitude;
        percent = Mathf.Min(maxMouseDist, mouseDist) / maxMouseDist;
    }

    public void OnTriggerEnter(Collider trig) {
        trig.SendMessage("OnHit", this);
    }
    public void HoleComplete() {
        isControllable = false;
        AudioSource.PlayClipAtPoint(cupSounds[Random.Range(0, cupSounds.Length)], Camera.main.transform.position);
    }
    public void OnCollisionEnter(Collision col) {
        col.transform.SendMessage("OnHit", this, SendMessageOptions.DontRequireReceiver);
    }
}
