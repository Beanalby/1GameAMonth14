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

        Vector3 pos;
        speechObj = (GameObject)GameObject.Instantiate(speechPrefab);
        pos = speechObj.transform.position;
        speechObj.transform.parent = transform;
        speechObj.transform.localPosition = pos;
        speechObj.GetComponentInChildren<TextMesh>().text = speech;

        thoughtObj = (GameObject)GameObject.Instantiate(thoughtPrefab);
        pos = thoughtObj.transform.position;
        thoughtObj.transform.parent = transform;
        thoughtObj.transform.localPosition = pos;
        thoughtObj.GetComponentInChildren<TextMesh>().text = thoughts;
        thoughtObj.SetActive(false);
        speechObj.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        thoughtObj.SetActive(true);
        speechObj.SetActive(true);
    }
    public void OnTriggerExit2D(Collider2D other) {
        thoughtObj.SetActive(false);
        speechObj.SetActive(false);
    }
}
