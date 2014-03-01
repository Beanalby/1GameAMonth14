using UnityEngine;
using System.Collections;

public class FlippyPlayer : MonoBehaviour {

    private const float VELOCITY_MAX_ANGLE = 8.5f;

    public AudioClip jumpSound, dieSound;

    float xSpeed = 3;
    private float jumpSpeed = 8.5f;
    private float gravity = 2f;

    private float deathStart = -1f;
    private Camera cam;

    public bool IsPlaying {
        get { return deathStart == -1f; }
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        rigidbody2D.gravityScale = gravity;
        rigidbody2D.velocity = new Vector2(xSpeed, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
    }
    
    // Update is called once per frame
    void Update () {
        if(IsPlaying) {
            if(Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
                AudioSource.PlayClipAtPoint(jumpSound, cam.transform.position);
                rigidbody2D.velocity = new Vector2(xSpeed, jumpSpeed);
            }
            // adjust the rotation based on velocity
            float v = rigidbody2D.velocity.y;
            v += VELOCITY_MAX_ANGLE;
            float angle = Mathf.Lerp(360, 0, v / (VELOCITY_MAX_ANGLE * 2));
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void OnCollisionEnter2D(Collision2D col) {
        if(deathStart == -1) {
            deathStart = Time.time;
            collider2D.enabled = false;
            rigidbody2D.velocity = new Vector3(-4, 10, 0);
            AudioSource.PlayClipAtPoint(dieSound, cam.transform.position);
        }
    }
}
