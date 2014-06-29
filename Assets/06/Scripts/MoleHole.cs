using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class MoleHole : MonoBehaviour {
        private Mole mole;
        public void Start() {
            mole = GetComponentInChildren<Mole>();
        }
        public void Raise(float duration) {
            mole.Raise(duration);
        }
    }
}