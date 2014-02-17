using UnityEngine;
using System.Collections;

public class FlippyPipeMaker : MonoBehaviour {
    public GameObject pipePrefab;

    private float spaceX = 7;
    private float rangeY = 3.5f;

    private float nextPipe = 22;
    private float spawnPipeRange = 15;

    private GameObject player;

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
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
        GameObject newPipe = GameObject.Instantiate(pipePrefab) as GameObject;
        newPipe.transform.parent = transform;
        newPipe.transform.localPosition = new Vector2(nextPipe,
            Random.Range(-rangeY, rangeY));
        nextPipe += spaceX;
    }

}
