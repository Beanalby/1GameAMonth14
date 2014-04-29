using UnityEngine;
using System.Collections;

public class gbFly : MonoBehaviour {
    public Transform ships;
    public gbSpeechBubble bubble;
    public GameObject endPrefab;

    private float flyStart = -1;
    private float wiggleAmount = .125f;
    private float wigglePeriod = .125f;
    private float wiggleDuration = 4;
    private float flySpeed = .25f;
    private float endTrigger = 6f;
    private GameObject endObj;
    Vector3 basePos, velocity=Vector3.zero;

    private bool isFlying = false;

    public void Start() {
        flyStart = Time.time;
        basePos = ships.position;
    }

    public void Update() {
        float diff = Time.time - flyStart;
        if(diff < wiggleDuration) {
            float posX;
            float percent = diff % wigglePeriod / wigglePeriod;
            if(percent < .5f) {
                posX = Mathf.Lerp(basePos.x - wiggleAmount, basePos.x + wiggleAmount,
                    percent * 2);
            } else {
                posX = Mathf.Lerp(basePos.x + wiggleAmount, basePos.x - wiggleAmount,
                    (percent - .5f) * 2);
            }
            ships.position = new Vector3(posX, basePos.y, basePos.z);
        } else {
            if(!isFlying) {
                isFlying = true;
                Vector3 pos = bubble.transform.position;
                bubble.transform.parent = ships;
                bubble.transform.position = pos;
            }
            velocity.y += Time.deltaTime * flySpeed;
            ships.position = new Vector3(
                ships.position.x,
                ships.position.y + velocity.y,
                ships.position.z);
            if(endObj == null && (Time.time - flyStart) > endTrigger) {
                endObj = (GameObject)Instantiate(endPrefab);
                endObj.transform.position += basePos;
                GameObject.Find("player").GetComponent<gbPlayer>().SendMessage("DisableControl");
            }
        }
    }
}
