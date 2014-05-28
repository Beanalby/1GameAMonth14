using UnityEngine;
using System.Collections;

public class gbSpeechBubble : MonoBehaviour {

    public GameObject speechPrefab, thoughtPrefab;
    public string speech;
    public string thoughts;
    public Vector3 bubbleOffset = Vector3.zero;
    public bool alwaysShow = false;
    public bool parent = false;

    private GameObject speechObj, thoughtObj;

    public void Start() {
        speech = speech.Replace("\\n", "\n");
        thoughts = thoughts.Replace("\\n", "\n");

        if(speech != "") {
            speechObj = (GameObject)GameObject.Instantiate(speechPrefab);
            if(parent) {
                Vector3 pos = speechObj.transform.position;
                speechObj.transform.parent = transform;
                speechObj.transform.localPosition = pos + bubbleOffset;
            } else {
                speechObj.transform.position += transform.position + bubbleOffset;
            }
            speechObj.GetComponentInChildren<TextMesh>().text = speech;
            speechObj.SetActive(false);
        }

        if(thoughts != "") {
            thoughtObj = (GameObject)GameObject.Instantiate(thoughtPrefab);
            if(parent) {
                Vector3 pos = thoughtObj.transform.position;
                thoughtObj.transform.parent = transform;
                thoughtObj.transform.localPosition = pos + bubbleOffset;
            } else {
                thoughtObj.transform.position += transform.position + bubbleOffset;
            }
            thoughtObj.GetComponentInChildren<TextMesh>().text = thoughts;
            thoughtObj.SetActive(false);
        }
        if(alwaysShow) {
            thoughtObj.SetActive(true);
            speechObj.SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(alwaysShow) {
            return;
        }
        if(thoughtObj) {
            thoughtObj.SetActive(true);
        }
        if(speechObj) {
            speechObj.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D other) {
        if(alwaysShow) {
            return;
        }
        if(thoughtObj) {
            thoughtObj.SetActive(false);
        }
        if(speechObj) {
            speechObj.SetActive(false);
        }
    }
}
