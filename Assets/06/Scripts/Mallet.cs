using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class Mallet : MonoBehaviour {
        public AudioClip swingSound;

        Animator anim;
        public void Start() {
            anim = GetComponent<Animator>();
            anim.Play("Idle");
        }

        public void SwingDown() {
            anim.SetTrigger("swingDown");
            StartCoroutine(_swingDown());
        }
        private IEnumerator _swingDown() {
            yield return new WaitForSeconds(.125f);
            Camera.main.SendMessage("Shake");
            AudioSource.PlayClipAtPoint(swingSound, Camera.main.transform.position);
        }

        public void SwingUp() {
            anim.SetTrigger("swingUp");
        }
    }
}