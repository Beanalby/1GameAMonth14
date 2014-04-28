using UnityEngine;
using System.Collections;

public class gbPlayer : MonoBehaviour {

    private float runSpeed = 4f;
    private float groundDamping = 20f; // how fast do we change direction? higher means faster
    private float inAirDamping = 5f;
    private float jumpHeight = 3f;

    private float gravity; // comes from rigidbody2D
    CharacterController2D cc;
    Animator anim;
    gbXray xray;

    private int shipParts = 0;
    public int ShipParts {
        get { return shipParts; }
    }

    private bool didJump;

    public void Start() {
        gravity = -9.8f * rigidbody2D.gravityScale;
        cc = GetComponent<CharacterController2D>();
        cc.onTriggerEnterEvent += OnTriggerEnter2D;
        cc.onTriggerExitEvent += OnTriggerExit2D;
        anim = GetComponentInChildren<Animator>();
        xray = GetComponent<gbXray>();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            didJump = true;
        }
        if (Input.GetButtonDown("Jump")) {
            xray.DoXray();
        }
    }

    public void FixedUpdate() {
        Vector3 velocity = cc.velocity;
        float hSpeed = Input.GetAxis("Horizontal");

        if((hSpeed > 0 && transform.localScale.x < 0f)
                || (hSpeed < 0 && transform.localScale.x > 0f)) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        float smoothedMovementFactor = cc.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        velocity.x = Mathf.Lerp(velocity.x, hSpeed * runSpeed, Time.fixedDeltaTime * smoothedMovementFactor);
        if(cc.isGrounded) {
            if(didJump) {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                anim.SetTrigger("jump");
                //AudioSource.PlayClipAtPoint(JumpSound, transform.position);
            }
        }
        velocity.y += gravity * Time.fixedDeltaTime;
        anim.SetFloat("hSpeed", Mathf.Abs(velocity.x));
        anim.SetFloat("vSpeed", velocity.y);
        cc.move(velocity*Time.fixedDeltaTime);
        didJump = false;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.SendMessage("PlayerEntered", this, SendMessageOptions.DontRequireReceiver);
        if(other.tag == "Pickup") {
            if(!xray.IsXray) {
                Debug.Log("Not xray, skipping!");
            } else {
                Debug.Log("Picking up " + other.name);
                other.SendMessage("PickedUp");
                shipParts++;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D col) {
    }
}
