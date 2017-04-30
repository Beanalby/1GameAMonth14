using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    [RequireComponent(typeof(Animator))]
    public class Bat : MonoBehaviour {

        public AudioClip attackSound;

        private int damage = 10;
        private float attackSpeed = 10f;
        private float resetSpeed = 5f;
        private Animator anim;

        private Vector3 startPos;
        private GameObject target = null;
        private GameObject inRange = null;
        private float attackStart = -1f;
        private bool isResetting = false;

        private int layerAttackable, layerPlayer;

        public void Start() {
            startPos = transform.position;
            anim = GetComponent<Animator>();
            layerAttackable = LayerMask.NameToLayer("Attackable");
            layerPlayer = LayerMask.NameToLayer("Player");
        }

        public void Update() {
            UpdateAttack();
            UpdateReset();
        }

        private void UpdateAttack() {
            if(attackStart == -1f) {
                return;
            }
            // move us towards the target
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            transform.position += dir * attackSpeed * Time.deltaTime;
        }
        private void UpdateReset() {
            if(!isResetting) {
                return;
            }
            Vector3 dir = startPos - transform.position;
            if(dir.magnitude <= resetSpeed * Time.deltaTime) {
                GetComponent<Rigidbody2D>().MovePosition(startPos);
                isResetting = false;
                // if the target is still in range, hit 'em again
                if(inRange != null) {
                    Attack(inRange);
                }
            } else {
                dir.Normalize();
                GetComponent<Rigidbody2D>().MovePosition(transform.position + dir * resetSpeed * Time.deltaTime);
            }
        }

        public void Attack(GameObject newTarget) {
            StartCoroutine(_attack(newTarget));
        }

        private IEnumerator _attack(GameObject newTarget) {
            anim.SetTrigger("attackStart");
            target = newTarget;
            yield return new WaitForSeconds(.4f);
            AudioSource.PlayClipAtPoint(attackSound, Camera.main.transform.position);
            attackStart = Time.time;
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer == layerAttackable && attackStart==-1f) {
                Attack(other.gameObject);
                inRange = other.gameObject;
            } else if(other.gameObject.layer == layerPlayer) {
                Player.Instance.GotHit(damage);
                attackStart = -1;
                isResetting = true;
                anim.SetTrigger("attackStop");
            } else if(other.tag == "Portal") {
                attackStart = -1;
                isResetting = true;
                anim.SetTrigger("attackStop");
            }
        }
        public void OnTriggerExit2D(Collider2D other) {
            // if our target's out of range, don't launch another attack
            if(other.gameObject == inRange) {
                inRange = null;
            }
        }
    }
}