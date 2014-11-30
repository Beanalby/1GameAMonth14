using UnityEngine;
using System.Collections;

namespace onegam_1411 {
    public class GameDriver : MonoBehaviour {

        public static GameDriver Instance {
            get { return _instance; }
        }
        private static GameDriver _instance = null;

        private const float gameDoneCheckDelay = .5f;

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Can't have to gamedrivers!");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private BalloonLauncher launcher;
        private Player player;
        private Catcher catcher;

        public void Start() {
            launcher = GameObject.FindObjectOfType<BalloonLauncher>();
            player = GameObject.FindObjectOfType<Player>();
            catcher = GameObject.FindObjectOfType<Catcher>();
            StartCoroutine(WatchForGameDone());

            player.CanControl = true; // todo: countdown/intro
        }

        public void BalloonMissed() {
            Debug.Log("BOOO, balloon missed!");
        }

        private void EndGame() {
            Debug.Log("END GAME!");
        }

        private IEnumerator WatchForGameDone() {
            while(true) {
                yield return new WaitForSeconds(gameDoneCheckDelay);
                if(IsGameDone()) {
                    EndGame();
                    yield break;
                }
            }
        }

        private bool IsGameDone() {
            // easiest check - balloons to launch or still floating around?
            if(!launcher.AreBalloonsDone()) {
                Debug.Log("Balloons aren't done!");
                return false;
            }

            // no more balloons floating around, is the player still
            // processing normal pops?  Need to let him process for combo
            if(catcher.IsPopping) {
                Debug.Log("Popping's going on!");
                return false;
            }

            player.CanControl = false;
            // if it still has balloons, force them to pop
            if(catcher.HasBalloons) {
                catcher.ForcePop();
                return false;
            }

            // everything's done.
            Debug.Log("All Done");
            return true;
        }
    }
}