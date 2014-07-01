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
                return _instance;
            }
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

        private float currentDuration = 3;
        private float initialOffset = 2;
        bool lastWasRandom = false;

        private MoleHole[] moles,
            rowBottom, rowMid, rowTop,
            columnLeft, columnMid, columnRight,
            diagonalL, diagonalL1, diagonalL2,
            diagonalR, diagonalR1, diagonalR2,
            squareInner, squareOuter;

        private int waveTotal, waveMiss, waveHit;

        private bool isRunning = false;

        public void Start() {
            InitMoles();
        }

        public void SendWave(int index=-1) {
            isRunning = true;
            // do a random wave between every other one
            if(!lastWasRandom && index==-1) {
                lastWasRandom = true;
                SetupWaveIndividual(currentDuration);
            } else {
                lastWasRandom = false;
                int total = 6;
                if(index == -1) {
                    index = Random.Range(0, total);
                }
                switch(index) {
                    case 0: SetupWaveRandomGroup(currentDuration); break;
                    case 1: SetupWaveDiagonal(currentDuration); break;
                    case 2: SetupWaveZ(currentDuration); break;
                    case 3: SetupWaveLines(currentDuration); break;
                    case 4: SetupWaveChase(currentDuration); break;
                    case 5: SetupWaveSquare(currentDuration); break;
                }
            }
        }

        private void InitMoles() {
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
            squareInner = new MoleHole[] { moles[3], moles[4], moles[8], moles[9] };
            squareOuter = new MoleHole[] { moles[0], moles[2], moles[10], moles[12] };
        }
        private void InitWave() {
            waveTotal = waveMiss = waveHit = 0;
        }
        private void SetupWaveIndividual(float duration, int num = 10) {
            InitWave();
            float offset = initialOffset;
            while(num-- > 0) {
                SetPopRandom(duration, offset);
                offset += duration * .3f;
            }
        }
        private void SetupWaveRandomGroup(float duration, int num = 6) {
            InitWave();
            float offset = initialOffset;
            while(num-- > 0) {
                SetPopRandom(duration, offset, 3);
                offset += duration * 1.1f;
            }
        }
        private void SetupWaveDiagonal(float duration) {
            float offset = initialOffset;
            InitWave();
            SetPop(duration, offset, diagonalL, true);
            offset += 1.5f * duration;
            SetPop(duration, offset, diagonalR, true);
            offset += 1.5f * duration;
            SetPop(duration, offset, diagonalL1, false);
            offset += .5f * duration;
            SetPop(duration, offset, diagonalL2, false);
            offset += 1 * duration;
            SetPop(duration, offset, diagonalR1, false);
            offset += .5f * duration;
            SetPop(duration, offset, diagonalR2, false);
            offset += 1 * duration;
        }
        private void SetupWaveZ(float duration) {
            InitWave();
            float offset = initialOffset;
            SetPop(duration, offset, rowTop, true);
            offset += .5f * duration;
            SetPop(duration, offset, diagonalL, true, true);
            offset += .75f * duration;
            SetPop(duration, offset, rowBottom, true);
            offset += .75f * duration;
            SetPop(duration, offset, rowBottom, true);
            offset += .5f * duration;
            SetPop(duration, offset, diagonalR, true);
            offset += .75f * duration;
            SetPop(duration, offset, rowTop, true);
            offset += .5f * duration;
        }
        private void SetupWaveLines(float duration) {
            InitWave();
            float offset = initialOffset;
            SetPop(duration, offset, rowBottom, true);
            offset += .75f * duration;
            SetPop(duration, offset, rowMid, true);
            offset += .75f * duration;
            SetPop(duration, offset, rowTop, true);
            offset += .75f * duration;
            SetPop(duration, offset, columnLeft, true);
            offset += .75f * duration;
            SetPop(duration, offset, columnMid, true);
            offset += .75f * duration;
            SetPop(duration, offset, columnRight, true);
            offset += .75f * duration;
        }

        private void SetupWaveChase(float duration) {
            InitWave();
            float offset = initialOffset;
            SetPop(duration, offset, diagonalL, true);
            offset += .8f * duration;
            SetPop(duration, offset, rowTop);
            offset += .8f * duration;

            SetPop(duration, offset, diagonalR, true);
            offset += .8f * duration;
            SetPop(duration, offset, columnLeft);
            offset += .8f * duration;

            SetPop(duration, offset, diagonalR, true, true);
            offset += .8f * duration;
            SetPop(duration, offset, columnRight);
            offset += .8f * duration;

            SetPop(duration, offset, diagonalL, true, true);
            offset += .8f * duration;
            SetPop(duration, offset, rowBottom);
            offset += .8f * duration;
        }
        private void SetupWaveSquare(float duration) {
            InitWave();
            float offset = initialOffset;
            SetPop(duration, offset, moles[6]);
            offset += .5f;
            SetPop(duration, offset, squareInner, true);
            offset += 1.5f;
            SetPop(duration, offset, squareOuter, true);
            offset += 1.5f;
            int num = 6;
            while(num-- > 0) {
                SetPopRandom(duration, offset);
                offset += duration * .3f;
            }
            SetPop(duration, offset, squareOuter, true, true);
            offset += 1.5f;
            SetPop(duration, offset, squareInner, true, true);
            offset += 1.5f;
            SetPop(duration, offset, moles[6]);
            offset += .5f;
        }
        private void SetPopRandom(float duration, float offset, int num = 1) {
            while(num-- > 0) {
                SetPop(duration, offset, moles[Random.Range(0, moles.Length)]);
            }
        }
        private void SetPop(float duration, float offset, MoleHole mole) {
            SetPop(duration, offset, new MoleHole[] { mole }, false);
        }
        private void SetPop(float duration, float offset, MoleHole[] moles, bool stagger = false, bool reverse = false) {
            waveTotal += moles.Length;
            StartCoroutine(_setPop(duration, offset, moles, stagger, reverse));
        }

        private IEnumerator _setPop(float duration, float offset, MoleHole[] moles, bool stagger, bool reverse) {
            yield return new WaitForSeconds(offset);
            if(!isRunning) {
                yield break;
            }
            if(reverse) {
                for(int i=moles.Length-1; i>=0;i--) {
                    if(!moles[i].Raise(duration)) {
                        // raising failed, take it off the total count
                        waveTotal--;
                    }
                    if(stagger) {
                        yield return new WaitForSeconds(duration / 10);
                        if(!isRunning) {
                            yield break;
                        }
                    }
                }
            } else {
                foreach(MoleHole mole in moles) {
                    if(!mole.Raise(duration)) {
                        // raising failed, take it off the total count
                        waveTotal--;
                    }
                    if(stagger) {
                        yield return new WaitForSeconds(duration / 10);
                        if(!isRunning) {
                            yield break;
                        }
                    }
                }
            }
        }

        public void MoleHit(Mole mole) {
            if(!isRunning) {
                return;
            }
            waveHit++;
            CheckWave();
        }
        public void MoleMiss(Mole mole) {
            if(!isRunning) {
                return;
            }
            waveMiss++;
            CheckWave();
        }

        private void CheckWave() {
            if(waveHit + waveMiss >= waveTotal) {
                WaveDone();
            }
        }

        private void WaveDone() {
            if(!isRunning) {
                return;
            }
            Debug.Log("Wave is done, hit " + (((float)waveHit / waveTotal) * 100)
                + "%, missed " + (((float)waveMiss / waveTotal) * 100));
            if(waveHit == waveTotal) {
                GameDriver.Instance.WavePerfect();
                currentDuration *= .83f;
                Debug.Log("currentDuration recuced t= " + currentDuration);
            } else {
                GameDriver.Instance.WaveComplete();
            }
            SendWave();
        }

        public void GameEnded() {
            isRunning = false;
        }
   }
}