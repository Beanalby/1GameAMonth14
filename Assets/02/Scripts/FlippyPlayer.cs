using UnityEngine;
using System.Collections;

public class FlippyPlayer : MonoBehaviour {

    private const float VELOCITY_MAX_ANGLE = 8.5f;

    public AudioClip jumpSound;

    float xSpeed = 3;
    private float jumpSpeed = 8.5f;
    private float gravity = 2f;

    // Use this for initialization
    void Start () {
        rigidbody2D.gravityScale = gravity;
        rigidbody2D.velocity = new Vector2(xSpeed, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
    }
    
    // Update is called once per frame
    void Update () {
        if(Input.GetButtonDown("Jump")) {
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            rigidbody2D.velocity = new Vector2(xSpeed, jumpSpeed);
        }
        // adjust the rotation based on velocity
        float v = rigidbody2D.velocity.y;
        v += VELOCITY_MAX_ANGLE;
        float angle = Mathf.Lerp(360, 0, v / (VELOCITY_MAX_ANGLE * 2));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Player hit " + col.collider.name);
    }
}
