using UnityEngine;
using System.Collections;

public class AngryThing : MonoBehaviour {
    private float angryRange = 8f;
    public bool isX = false;

    private Animator animator;
    private float stunStart = -1, stunDuration = 4f;
    private float lastCheck = -1, checkCooldown = .25f;
    private int playerMask;

    private GameObject target = null;

    public void Start() {
        animator = GetComponentInChildren<Animator>();
        playerMask = 1 << LayerMask.NameToLayer("Player");
    }
    public void Update() {
        UpdateTarget();
        HandleMovement();
        HandleStun();
    }

    private void UpdateTarget() {
        if(stunStart != -1) {
            return;
        }
        if(Time.time - lastCheck > checkCooldown) {
            if (target == null) {
                // see if they're in range
                Collider[] hits;
                hits = Physics.OverlapSphere(transform.position, angryRange,
                    playerMask);
                if (hits.Length != 0) {
                    target = hits[0].gameObject;
                    animator.SetBool("HasTarget", true);
                }
            } else {
                // see if they've left range
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance > angryRange + 2) {
                    Debug.Log("dist=" + distance + " > " + angryRange + ", clearing target!");
                    target = null;
                    animator.SetBool("HasTarget", false);
                }
            }
        }
    }

    private void HandleMovement() {
        if(stunStart != -1) {
            return;
        }
        if (target == null) {
            return;
        }

        // move towards the player
        Vector3 dest = transform.position;
        if (isX) {
            dest.x = target.transform.position.x;
        } else {
            dest.z = target.transform.position.z;
        }
        transform.position = Vector3.Lerp(transform.position, dest, .2f);
    }

    private void HandleStun() {
        if(stunStart == -1) {
            return;
        }
        float percent = (Time.time - stunStart) / stunDuration;
        Debug.Log("stun percent=" + percent);
        if(percent >= 1) {
           stunStart = -1;
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if(stunStart != -1) {
            return;
        }
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("GotHit", gameObject);
        }
    }

    public void GotHit(PlayerAttack pa) {
        stunStart = Time.time;
        animator.SetTrigger("Stunned");
    }
}
