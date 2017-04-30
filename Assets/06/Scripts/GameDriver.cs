using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1406 {
    public class GameDriver : MonoBehaviour {
        public Texture2D imgReady, imgSet, imgWhack, imgGameOver, waveComplete, wavePerfect, speedUp;

        private Texture2D topImg;

        private static GameDriver _instance = null;
        public static GameDriver Instance {
            get {
                if(_instance == null) {
                    Debug.LogError("Accessing GameDriver instance before Awake");
                    Debug.Break();
                    return null;
                }
                return _instance;
            }
        }
        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Two GameDrivers, that shouldn't happen.");
                Debug.Break();
                return;
            }
            _instance = this;
        }


        public GUISkin skin;

        private float timeStart = -1, totalTime = 90f;
        private int score = 0;

        private GUIStyle timeStyle, scoreStyle;
        private float timeLeft {
            get { return Mathf.Max(0, totalTime - (Time.time - timeStart)); }
        }
        public bool IsRunning {
            get { return timeStart != -1 && timeLeft > 0; }
        }
        private bool showRetry = false;
        private int pad = 10;

        public void Start() {

            timeStyle = new GUIStyle(skin.label);
            timeStyle.alignment = TextAnchor.LowerRight;
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.LowerLeft;
            StartCoroutine(Intro());
        }

        public void Update() {
            if(timeStart != -1 && timeLeft <= 0) {
                EndGame();
            }
        }
        private IEnumerator Intro() {
            yield return new WaitForSeconds(1);
            ShowImage(imgReady);
            yield return new WaitForSeconds(1);
            ShowImage(imgSet);
            BoardDriver.Instance.SendWave();
            yield return new WaitForSeconds(1);
            ShowImage(imgWhack);
            yield return new WaitForSeconds(1);
            timeStart = Time.time;
        }

        private void EndGame() {
            StartCoroutine(_endGame());
        }
        private IEnumerator _endGame() {
            ShowImage(imgGameOver, Mathf.Infinity);
            MalletDriver.Instance.GameEnded();
            BoardDriver.Instance.GameEnded();
            yield return new WaitForSeconds(1.5f);
            showRetry = true;
        }

        public void OnGUI() {
            GUI.skin = skin;
            Vector3 shadowDir = new Vector3(-2, 2, 0);
            Rect scoreRect = new Rect(pad, Screen.height - (pad + 100), Screen.width / 2, 100);
            Rect timeRect = new Rect(Screen.width / 2 - pad, Screen.height - (pad + 100), Screen.width / 2, 100);
            GUIContent content = new GUIContent();

            content.text = "Score: " + score;
            ShadowAndOutline.DrawShadow(scoreRect, content, scoreStyle,
                Color.white, Color.black, shadowDir);
            content.text = "Time: ";
            if(timeStart == -1) {
                content.text += totalTime;
            } else {
                content.text += timeLeft.ToString("00.0");
            }
            ShadowAndOutline.DrawShadow(timeRect, content, timeStyle,
                Color.white, Color.black, shadowDir);

            if(topImg != null) {
                GUI.DrawTexture(new Rect(
                    (Screen.width / 2) - topImg.width / 2, 10,
                    topImg.width, topImg.height), topImg);
            }
            if(showRetry) {
                int width = 300, height = 100;
                Rect buttonRect = new Rect((Screen.width / 2) - (width/2), 
                    (Screen.height / 2) - (height/2), width, height);
                if(GUI.Button(buttonRect, "Play Again")) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }

        public void MoleHit() {
            score++;
        }
        public void WaveComplete() {
            ShowImage(waveComplete);
        }
        public void WavePerfect() {
            StartCoroutine(_wavePerfect());
        }
        private IEnumerator _wavePerfect() {
            ShowImage(wavePerfect);
            yield return new WaitForSeconds(1f);
            ShowImage(speedUp);
        }
        private void ShowImage(Texture2D img, float duration = 1f) {
            StartCoroutine(_showImage(img, duration));
        }
        private IEnumerator _showImage(Texture2D img, float duration) {
            topImg = img;
            yield return new WaitForSeconds(duration);
            if(topImg == img) {
                topImg = null;
            }
        }
    }
}