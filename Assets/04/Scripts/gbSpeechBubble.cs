using UnityEngine;
using System.Collections;

public class gbSpeechBubble : MonoBehaviour {

    public GameObject speechPrefab, thoughtPrefab;
    public string speech;
    public string thoughts;

    private GameObject speechObj, thoughtObj;

    public void Start() {
        speech = speech.Replace("\\n", "\n");
        thoughts = thoughts.Replace("\\n", "\n");

        if(speech != "") {
            speechObj = (GameObject)GameObject.Instantiate(speechPrefab);
            speechObj.transform.position += transform.position;
            speechObj.GetComponentInChildren<TextMesh>().text = speech;
            speechObj.SetActive(false);
        }

        if(thoughts != "") {
            thoughtObj = (GameObject)GameObject.Instantiate(thoughtPrefab);
            thoughtObj.transform.position += transform.position;
            thoughtObj.GetComponentInChildren<TextMesh>().text = thoughts;
            thoughtObj.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(thoughtObj) {
            thoughtObj.SetActive(true);
        }
        if(speechObj) {
            speechObj.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D other) {
        if(thoughtObj) {
            thoughtObj.SetActive(false);
        }
        if(speechObj) {
            speechObj.SetActive(false);
        }
    }
}
