using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance = null;
        public static GameDriver Instance { get { return _instance; } }

        public GUISkin skin;

        private int score=0;
        public int Score {
            get { return score; }
        }

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Can't have multiple GameDrivers");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void CoinPicked(Coin coin) {
            score += coin.value;
       }

        public void OnGUI() {
            GUI.skin = skin;
            Rect scoreRect = new Rect(10, -10, 300, 100);
            ShadowAndOutline.DrawShadow(scoreRect, new GUIContent("Score: " + score),
                skin.label, Color.white, Color.black, new Vector2(3, 3));
        }
    }
}