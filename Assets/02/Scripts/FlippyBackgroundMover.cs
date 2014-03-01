using UnityEngine;
using System.Collections;

/// <summary>
/// Instantiates two copies of a background, and moves the later one ahead
/// once the player has 
/// </summary>
public class FlippyBackgroundMover : MonoBehaviour {

    public const int BackgroundWidth = 44;
    public GameObject[] bgPrefab;

    private Transform bgCurrent, bgOther, player;
    private FlippyDriver driver;

    public void Start() {
        driver = GameObject.FindObjectOfType<FlippyDriver>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // put the current bg just ahead of the player, to invoke the swap soon
        bgCurrent = (GameObject.Instantiate(bgPrefab[driver.Stage]) as GameObject).transform;
        bgCurrent.name = bgPrefab[driver.Stage].name;
        bgCurrent.transform.parent = transform;
        bgCurrent.transform.position = new Vector2(player.position.x + 1, 0);

        // bgOther will get placed ahead of bgCurrent in a second
        bgOther = (GameObject.Instantiate(bgPrefab[driver.Stage]) as GameObject).transform;
        bgOther.name = bgPrefab[driver.Stage].name;
        bgOther.transform.parent = transform;
    }

    public void Update() {
        // if the player is past the midpoint of the current background,
        // move the other one ahead of the current one
        if(player.position.x > bgCurrent.position.x) {
            bgOther.position = new Vector2(bgCurrent.position.x + BackgroundWidth, 0);
            // swap current & other
            Transform t = bgCurrent;
            bgCurrent = bgOther;
            bgOther = t;

            // check if we need to make a stage transition
            GameObject newBg = null;
            //Debug.Log("bg based on pos? " + (bgCurrent.position.x - (BackgroundWidth / 2)) + " vs " + driver.NextTransition);
            if(bgCurrent.position.x - (BackgroundWidth / 2) >= driver.NextTransition) {
                newBg = bgPrefab[driver.NextStage];
                //Debug.Log("transition to " + newBg.name + " because it's past NextTransition!");
            }
            // also transition if this is old
            //Debug.Log("bg based on name? " + bgCurrent.name + " vs " + bgPrefab[driver.Stage].name);
            if(bgCurrent.name != bgPrefab[driver.Stage].name) {
                //Debug.Log("Current transition because it doesn't match!");
                newBg = bgPrefab[driver.Stage];
            }
            if(newBg) {
                Vector3 pos = bgCurrent.position;
                Destroy(bgCurrent.gameObject);
                bgCurrent = (GameObject.Instantiate(newBg) as GameObject).transform;
                bgCurrent.name = newBg.name;
                bgCurrent.parent = transform;
                bgCurrent.position = pos;
            }
        }
    }
}
