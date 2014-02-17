using UnityEngine;
using System.Collections;

public class FlippyPlayer : MonoBehaviour {

    float xSpeed = 3;
    private float jumpSpeed = 6f;

    // Use this for initialization
    void Start () {
        rigidbody2D.velocity = new Vector2(xSpeed, 0);
    }
    
    // Update is called once per frame
    void Update () {
        if(Input.GetButtonDown("Jump")) {
            rigidbody2D.velocity = new Vector2(xSpeed, jumpSpeed);
        }
    }

    public void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Player hit " + col.collider.name);
    }
}
