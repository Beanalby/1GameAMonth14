using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class GameDriver : MonoBehaviour {
        public static GameDriver Instance {
            get { return _instance; }
        }
        private static GameDriver _instance = null;
        public void Awake() {
            if (_instance != null) {
                Debug.LogError("Can't have two GameDrivers");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public GUISkin skin;
        public GameObject foodPrefab, targetPrefab;

        private int foodCount = 4;
        private Vector2 foodRange = new Vector2(3,2);

        private GUIStyle sickStyle, scoreStyle;
        private int score = 0;

        private float foodSpawnDelay = 2f;

        public void Start() {
            sickStyle = new GUIStyle(skin.label);
            sickStyle.alignment = TextAnchor.UpperLeft;
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.UpperRight;

            // load up the scene with starting food
            for (int i = 0; i < foodCount; i++) {
                StartCoroutine(SpawnFood(true));
            }
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect sickRect = new Rect(10, 10, 200, 50);
            Rect scoreRect = new Rect(Screen.width - 210, 10, 200, 50);
            GUIContent content;

            content = new GUIContent("Sickness: " + Player.Instance.Sickness.ToString("N0"));
            ShadowAndOutline.DrawShadow(sickRect, content, sickStyle,
                Color.white, Color.black, new Vector2(-3, -3));

            content = new GUIContent("Score: " + score);
            ShadowAndOutline.DrawShadow(scoreRect, content, scoreStyle,
                Color.white, Color.black, new Vector2(-3, -3));

        }

        public void FoodEaten() {
            StartCoroutine(SpawnFood(false));
        }
        private IEnumerator SpawnFood(bool immediate) {
            if (!immediate) {
                yield return new WaitForSeconds(foodSpawnDelay);
            }
            Vector3 pos = new Vector3(
                Random.Range((int)-foodRange.x, (int)foodRange.x),
                Random.Range((int)-foodRange.y, (int)foodRange.y),
                0);
            Debug.Log("Spawning food @ " + pos);
            Instantiate(foodPrefab, pos, Quaternion.identity);
        }
    }
}