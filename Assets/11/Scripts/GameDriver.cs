using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1411 {
    public class GameDriver : MonoBehaviour {

        public GUISkin skin;
        public Texture2D missBox, missX;
        public GameObject scoreEffectPrefab;

        public static GameDriver Instance {
            get { return _instance; }
        }
        private static GameDriver _instance = null;

        private const float stageFinishedCheckDelay = .5f;
        private bool isGameOver = false;
        private bool isStageFinished = false;

        private int numMisses = 0, maxMisses = 3;
        private int score = 0, balloonScoreValue=10;

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
            StartCoroutine(WatchForStageFinished());

            player.CanControl = true; // todo: countdown/intro
        }

        public void BalloonMissed() {
            numMisses++;
            if(numMisses >= maxMisses) {
                GameOver();
            }
        }

        public void BalloonScored(Balloon balloon, int combo) {
            int amount = balloonScoreValue * combo;
            ScoreEffect effect = ((GameObject)Instantiate(scoreEffectPrefab)).GetComponent<ScoreEffect>();
            effect.Amount = amount;
            effect.transform.position = balloon.transform.position;

            score += amount;
        }

        private void GameOver() {
            Debug.Log("Game over!");
            // kill the balloons, and the player
            foreach(Balloon balloon in GameObject.FindObjectsOfType<Balloon>()) {
                Destroy(balloon.gameObject);
            }
            Destroy(launcher.gameObject);
            Destroy(player.gameObject);
            isGameOver = true;
        }

        private void StageFinished() {
            isStageFinished = true;
        }

        private IEnumerator WatchForStageFinished() {
            while(true) {
                yield return new WaitForSeconds(stageFinishedCheckDelay);
                if(isGameOver) {
                    yield break;
                }
                isStageFinished = CheckStageFinished();
                if(isStageFinished) {
                    StageFinished();
                    yield break;
                }
            }
        }

        private bool CheckStageFinished() {
            // easiest check - balloons to launch or still floating around?
            if(!launcher.AreBalloonsDone()) {
                return false;
            }

            // no more balloons floating around, is the player still
            // processing normal pops?  Need to let him process for combo
            if(catcher.IsPopping) {
                return false;
            }

            player.CanControl = false;
            // if it still has balloons, force them to pop
            if(catcher.HasBalloons) {
                catcher.ForcePop();
                return false;
            }

            // everything's done.
            return true;
        }

        public void OnGUI() {
            int pad = 10;
            GUI.skin = skin;
            Rect scoreRect = new Rect(pad, pad, 300, 40);
            Rect missLabelRect = new Rect(pad, 50, 100, 64);
            Rect missRect = new Rect(0, 50, missBox.width, missBox.height);

            string endMsg = null;
            Rect endRect = new Rect(0, 180, Screen.width, 100);
            Rect retryRect = new Rect(Screen.width / 2 - 100, 300, 200, 50);
            GUIStyle endStyle = new GUIStyle(skin.label);
            endStyle.alignment = TextAnchor.UpperCenter;
            endStyle.fontSize *= 2;


            GUIContent content = new GUIContent("Score: " + score);
            ShadowAndOutline.DrawShadow(scoreRect, content, skin.label, Color.white, Color.black, new Vector2(-3, -3));

            content = new GUIContent("Misses: ");
            ShadowAndOutline.DrawShadow(missLabelRect, content, skin.label, Color.white, Color.black, new Vector2(-3, -3));

            for(int i=0;i<maxMisses; i++) {
                missRect.x = pad + missLabelRect.width + (i * 64);
                GUI.DrawTexture(missRect, missBox);
                if(i < numMisses) {
                    GUI.DrawTexture(missRect, missX);
                }
            }

            if(isGameOver) {
                endMsg = "Game Over";
            } else if(isStageFinished) {
                endMsg = "Success!";
            }
            if(endMsg != null) {
                content = new GUIContent(endMsg);
                ShadowAndOutline.DrawShadow(endRect, content, endStyle, Color.white, Color.black, new Vector2(-3, -3));
                if(GUI.Button(retryRect, new GUIContent("Play Again"))) {
                    SceneManager.LoadScene("11title");
                }
            }
        }
    }
}