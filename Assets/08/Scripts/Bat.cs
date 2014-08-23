using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    [RequireComponent(typeof(Animator))]
    public class Bat : MonoBehaviour {

        private float attackSpeed = 7f;
        private Animator anim;

        private GameObject target;
        private float attackStart = -1f;

        public void Start() {
            anim = GetComponent<Animator>();
        }

        public void Update() {
            UpdateAttack();
            if(Input.GetKeyDown(KeyCode.Space)) {
                GameObject obj = GameObject.Find("Sphere");
                Debug.Log("+++ attacking " + obj);
                Attack(obj);
            }
        }

        public void UpdateAttack() {
            if(attackStart == -1f) {
                return;
            }
            // move us towards the target
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            transform.position += dir * attackSpeed * Time.deltaTime;
        }

        public void Attack(GameObject newTarget) {
            StartCoroutine(_attack(newTarget));
        }

        private IEnumerator _attack(GameObject newTarget) {
            anim.SetTrigger("attackStart");
            yield return new WaitForSeconds(.4f);
            target = newTarget;
            attackStart = Time.time;
        }

        public void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("+++ " + name + " hit " + other.name);
            if(other.gameObject.layer == LayerMask.NameToLayer("Attackable")) {
                Attack(other.gameObject);
            } else if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                attackStart = -1;
                anim.SetTrigger("attackStop");
                rigidbody2D.MovePosition(transform.position + new Vector3(0, 1, 0));
            }
        }
    }
}