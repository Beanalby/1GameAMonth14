using UnityEngine;
using System.Collections;

namespace onegam_1406 {

    public class BoardDriver : MonoBehaviour {
        private static BoardDriver _instance = null;
        public static BoardDriver Instance {
            get {
                if(_instance == null) {
                    Debug.LogError("Accessing BoardDriver instance before Awake");
                    Debug.Break();
                    return null;
                }
            return _instance; }
        }
        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Two BoardDrivers, that shouldn't happen.");
                Debug.Break();
                return;
            }
            _instance = this;
        }

        public GameObject moleHolder;

        private MoleHole[] moles,
            rowBottom, rowMid, rowTop,
            columnLeft, columnMid, columnRight,
            diagonalL, diagonalL1, diagonalL2,
            diagonalR, diagonalR1, diagonalR2;

        private int waveTotal, waveMiss, waveHit;

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
            DoWaves();
        }

        private void UseWaves() {
            // never actually called, just stops warning messages about unused vars
            SetPop(0, 0, rowBottom);
            SetPop(0, 0, rowMid);
            SetPop(0, 0, rowTop);
            SetPop(0, 0,columnLeft);
            SetPop(0, 0,columnMid);
            SetPop(0, 0,columnRight);
            SetPop(0, 0, diagonalL);
            SetPop(0, 0, diagonalL1);
            SetPop(0, 0, diagonalL2);
            SetPop(0, 0, diagonalR);
            SetPop(0, 0, diagonalR1);
            SetPop(0, 0, diagonalR2);
        }
        private void InitWave() {
            waveTotal = waveMiss = waveHit = 0;
        }

        private void DoWaves() {
            InitWave();
            float offset = 1f;
            float duration;

            duration = 2f;
            InitDiagonalWave(duration, offset);
            offset += 3 * duration;

            duration = 1.5f;
            InitDiagonalWave(duration, offset);
            offset += 3 * duration;

            duration = 1f;
            InitDiagonalWave(duration, offset);
            offset += 3 * duration;

            duration = .75f;
            InitDiagonalWave(duration, offset);
            offset += 3 * duration;
        }

        private void InitDiagonalWave(float duration, float offset) {
            SetPop(duration, offset, diagonalL, true);
            offset += 1.5f * duration;
            SetPop(duration, offset, diagonalR, true);
        }

        private void SetPopRandom(float duration, float offset, int num = 1) {
            while(num-- > 0) {
                SetPop(duration, offset, moles[Random.Range(0, moles.Length)]);
            }
        }
        private void SetPop(float duration, float offset, MoleHole mole) {
            SetPop(duration, offset, new MoleHole[] { mole }, false);
        }
        private void SetPop(float duration, float offset, MoleHole[] moles, bool stagger = false) {
            waveTotal += moles.Length;
            StartCoroutine(_setPop(duration, offset, moles, stagger));
        }

        private IEnumerator _setPop(float duration, float offset, MoleHole[] moles, bool stagger = false) {
            yield return new WaitForSeconds(offset);
            foreach(MoleHole mole in moles) {
                mole.Raise(duration);
                if(stagger) {
                    yield return new WaitForSeconds(duration / 10);
                }
            }
        }

        public void MoleHit(Mole mole) {
            waveHit++;
            Debug.Log(mole.GetHole().name + " hit, now " + waveHit + "-" + waveMiss + " / " + waveTotal);
            CheckWave();
        }
        public void MoleMiss(Mole mole) {
            waveMiss++;
            Debug.Log(mole.GetHole().name + " miss, now " + waveHit + "-" + waveMiss + " / " + waveTotal);
            CheckWave();
        }

        private void CheckWave() {
            if(waveHit + waveMiss >= waveTotal) {
                WaveDone();
            }
        }

        private void WaveDone() {
            Debug.Log("Wave is done, hit " + (((float)waveHit / waveTotal) * 100)
                + "%, missed " + (((float)waveMiss / waveTotal) * 100));
        }
    }
}