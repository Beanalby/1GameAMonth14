using UnityEngine;
using System.Collections;

public class FlippyPlayer : MonoBehaviour {

    private const float VELOCITY_MAX_ANGLE = 8.5f;

    public AudioClip jumpSound, dieSound;

    float xSpeed = 3;
    private float jumpSpeed = 8.5f;
    private float gravity = 2f;

    private float deathStart = -1f;
    public float DeathStart {
        get { return deathStart; }
    }

    private Camera cam;
    private FlippyMusic music;

    public bool IsPlaying {
        get { return deathStart == -1f; }
    }

    // Use this for initialization
    void Start () {
        bool debug = false;
        if(debug) {
            GetComponent<Collider2D>().enabled = false;
            xSpeed = 9;
            gravity = 0f;
        }
        cam = Camera.main;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        music = GameObject.FindObjectOfType<FlippyMusic>();
        Jump();
    }
    
    // Update is called once per frame
    void Update () {
        if(IsPlaying) {
            if(Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
                Jump();
            }
            // adjust the rotation based on velocity
            float v = GetComponent<Rigidbody2D>().velocity.y;
            v += VELOCITY_MAX_ANGLE;
            float angle = Mathf.Lerp(360, 0, v / (VELOCITY_MAX_ANGLE * 2));
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void Jump() {
        AudioSource.PlayClipAtPoint(jumpSound, cam.transform.position);
        GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, jumpSpeed);
    }

    public void OnCollisionEnter2D(Collision2D col) {
        if(deathStart == -1) {
            deathStart = Time.time;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector3(-4, 10, 0);
            AudioSource.PlayClipAtPoint(dieSound, cam.transform.position);
            music.PlayerDied();
        }
    }
}
