using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class BoardDriver : MonoBehaviour {
        public GameObject moleHolder;

        private float moveDelay = 4f;

        private float lastMove = Mathf.NegativeInfinity;
        private MoleHole[] moles;

        public void Start() {
            moles = moleHolder.GetComponentsInChildren<MoleHole>();
            Debug.Log("Board has " + moles.Length + " moles");
        }

        public void Update() {
            if(Time.time > lastMove + moveDelay) {
                foreach(MoleHole mole in moles) {
                    mole.Raise();
                }
                lastMove = Time.time;
            }
        }
    }
}