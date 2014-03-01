using UnityEngine;
using System.Collections;

public class FlippyPipeMaker : MonoBehaviour {
    public GameObject[] pipePrefab;

    private float spaceX = 7;
    private float rangeY = 3.5f;

    private float nextPipe = 12;
    private float spawnPipeRange = 15;

    private GameObject player;
    private FlippyDriver driver;

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        driver = GameObject.FindObjectOfType<FlippyDriver>();
    }

    public void Update() {
        while(player.transform.position.x > nextPipe - spawnPipeRange) {
                SpawnPipe();
            RemovePipes();
        }
    }

    private void RemovePipes() {
        foreach(Transform t in transform) {
            if(t.position.x < player.transform.position.x - spawnPipeRange) {
                Destroy(t.gameObject);
            }
        }
    }

    public void SpawnPipe() {
        int pipeIndex = driver.Stage;
        if(nextPipe > driver.NextTransition) {
            pipeIndex = driver.NextStage;
        }
        GameObject newPipe = GameObject.Instantiate(pipePrefab[pipeIndex]) as GameObject;
        newPipe.transform.parent = transform;
        newPipe.transform.localPosition = new Vector2(nextPipe,
            Random.Range(-rangeY, rangeY));
        nextPipe += spaceX;
    }

}
