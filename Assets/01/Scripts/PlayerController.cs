using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public  float speed = 5;
    public float turnSpeed = 75;
    private float throwStrength = 5f;

    private WorldControl wc;
    private bool needsCentered = true;

    public bool canControl = true;
    public bool faceVelocity = false;

    WorldState state;

    public bool CanControl {
        get { return canControl; }
    }
    private float currentSpeed = 0;
    private Animator animator;

    public void Start() {
        state = GameObject.Find("WorldState").GetComponent<WorldState>();
        animator = GetComponentInChildren<Animator>();
        wc = GameObject.Find("WorldState").GetComponent<WorldControl>();
    }
    public void Update() {
        HandleMoving();
        if(animator) {
            animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
        }
    }

    void HandleMoving() {
        currentSpeed = 0;
        if (!canControl) {
            if (faceVelocity) {
                GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(GetComponent<Rigidbody>().velocity));
            }
            return;
        }
        if (wc.showingMenu) {
            needsCentered = true;
            return;
        }
        if (needsCentered) {
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
                needsCentered = false;
            } else {
                return;
            }
        }
        Vector3 rot = GetComponent<Rigidbody>().rotation.eulerAngles;
        float turnAmount = Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;
        if (state.world == GameWorld.race && Input.GetAxis("Vertical") < 0) {
            turnAmount = -turnAmount;
        }
        rot.y += turnAmount;
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rot));

        if(Input.GetAxis("Vertical") != 0) {
            currentSpeed = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            Vector3 pos = GetComponent<Rigidbody>().position;
            pos += transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * Time.deltaTime * speed;
            GetComponent<Rigidbody>().MovePosition(pos);
        }
    }

    public void GotHit(GameObject attacker) {
        Vector3 throwDir = attacker.transform.TransformDirection(Vector3.forward);
        throwDir.y = 1;
        throwDir *= throwStrength;
        Fling(throwDir, false);
    }

    public void Fling(Vector3 direction, bool face) {
        // first move us up a little, so we don't immediately collide with the ground
        Vector3 pos = transform.position;
        pos.y += .3f;
        transform.position = pos;

        // apply our new velocity
        GetComponent<Rigidbody>().velocity = direction;
        canControl = false;
        this.faceVelocity = face;
    }

    public void OnCollisionEnter(Collision collision) {
        if (!canControl) {
            canControl = true;
        }
    }
}
