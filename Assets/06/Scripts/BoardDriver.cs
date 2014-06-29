using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class BoardDriver : MonoBehaviour {
        public GameObject moleHolder;

        private MoleHole[] moles,
            rowBottom, rowMid, rowTop,
            columnLeft, columnMid, columnRight,
            diagonalL, diagonalL1, diagonalL2,
            diagonalR, diagonalR1, diagonalR2;

        public void Start() {
            moles = moleHolder.GetComponentsInChildren<MoleHole>();
            System.Array.Sort(moles, (x, y) => x.name.CompareTo(y.name));
            // create some convience arrays for making patterns
            rowBottom = new MoleHole[] { moles[0], moles[1], moles[2] };
            rowMid = new MoleHole[] { moles[5], moles[6], moles[7] };
            rowTop = new MoleHole[] { moles[10], moles[11], moles[12] };
            columnLeft = new MoleHole[] { moles[0], moles[5], moles[10] };
            columnMid = new MoleHole[] { moles[1], moles[6], moles[11] };
            columnRight = new MoleHole[] { moles[2], moles[7], moles[12] };
            /// diagonals start at bottom, without number is "big", two numbers
            /// are smaller ones
            ///   L       L1      L2
            /// . . *   . . .   . * .
            ///  . *     . .     * .
            /// . * .   . . *   * . .
            ///  * .     . *     . .
            /// * . .   . * .   . . .
            diagonalL = new MoleHole[] { moles[0], moles[3], moles[6], moles[9], moles[12] };
            diagonalL1 = new MoleHole[] { moles[1], moles[4], moles[7] };
            diagonalL2 = new MoleHole[] { moles[5], moles[8], moles[11] };
            diagonalR = new MoleHole[] { moles[2], moles[4], moles[6], moles[8], moles[10] };
            diagonalR1 = new MoleHole[] { moles[1], moles[3], moles[5] };
            diagonalR2 = new MoleHole[] { moles[7], moles[9], moles[11] };
            InitPop();
        }

        private void InitPop() {
            float offset = 1f;
            SetPop(offset, diagonalL, .1f);
            offset += 2.5f;
            SetPop(offset, diagonalR);
        }

        private void SetPopRandom(float offset, int num = 1) {
            while(num-- > 0) {
                SetPop(offset, moles[Random.Range(0, moles.Length)]);
            }
        }
        private void SetPop(float offset, MoleHole mole) {
            StartCoroutine(_setPop(offset, new MoleHole[] { mole }, 0));
        }
        private void SetPop(float offset, MoleHole[] moles, float delay = 0) {
            StartCoroutine(_setPop(offset, moles, delay));
        }

        private IEnumerator _setPop(float offset, MoleHole[] moles, float delay) {
            yield return new WaitForSeconds(offset);
            foreach(MoleHole mole in moles) {
                mole.Raise();
                if(delay != 0) {
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}