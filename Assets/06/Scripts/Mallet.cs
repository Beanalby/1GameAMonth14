using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class Mallet : MonoBehaviour {

        Animator anim;
        public void Start() {
            anim = GetComponent<Animator>();
            anim.Play("Idle");
        }

        public void SwingDown() {
             anim.SetTrigger("swingDown");
        }
        public void SwingUp() {
            anim.SetTrigger("swingUp");
        }
    }
}