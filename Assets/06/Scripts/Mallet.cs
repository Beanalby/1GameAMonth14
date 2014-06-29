using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class Mallet : MonoBehaviour {

        Animator anim;
        public void Start() {
            anim = GetComponent<Animator>();
            anim.Play("Idle");
        }

        public void Swing() {
             anim.SetTrigger("swing");
        }
    }
}