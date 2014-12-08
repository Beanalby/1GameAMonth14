using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Food : MonoBehaviour {
        public GUISkin skin;

        [HideInInspector]
        public int whichFood = -1;

        public Sprite[] foods;
        public string[] labels;

        private float sickValue = 30;
        public float SickValue {
            get { return sickValue; }
        }

        private string label;

        public void Start() {
            // if our instantiator didn't already chosen a food for us,
            // choose one randomly
            if(whichFood == -1) {
                whichFood = Random.Range(0, foods.Length);
            }
            GetComponent<SpriteRenderer>().sprite = foods[whichFood];
            label = labels[whichFood].Replace("\\n", "\n");
        }

        public void OnGUI() {
            GUI.skin = skin;

            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            pos.y = Screen.height - pos.y;
            pos.y += 32;
            //GUI.Label(new Rect(pos.x-100, pos.y, 200, 200), "TEST");
            ShadowAndOutline.DrawShadow(new Rect(pos.x-100, pos.y, 200, 200),
                new GUIContent(label), skin.label, Color.white, Color.black,
                new Vector2(-2, -2));
        }

        public void PickedUp() {
            // maybe do something fancier later.
            Destroy(gameObject);
        }
    }
    
}
