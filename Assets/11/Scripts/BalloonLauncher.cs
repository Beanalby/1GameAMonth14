using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace onegam_1411 {
    public class BalloonLauncher : MonoBehaviour {

        public GameObject balloonPrefab;

        private int numColumns = 5;
        private int numRows = 4;
        private float launchDelay = 3f;
        private float xSpacer = 1.5f;
        private float ySpacer = 1.2f;
        private List<Balloon> balloons;
        private int nextBalloon = 0;
        private Color[] colors = new Color[3] {
            Color.red, Color.green, Color.blue };
        public void Start() {
            CreateBalloons();
            StartCoroutine(StartLaunching());
        }

        private void CreateBalloons() {
            balloons = new List<Balloon>(numColumns * numRows);
            List<int> numInColumn = new List<int>(numColumns);
            int xOffset = -(int)((numColumns - 1) / 2);
            for(int i = 0; i < numColumns; i++) {
                numInColumn.Add(0);
            }
            for(int i = 0; i < numColumns * numRows; i++) {
                // find a new column we haven't used
                int newColumn;
                while(true) {
                    newColumn = Random.Range(0, numColumns);
                    if(numInColumn[newColumn] < numRows) {
                        break;
                    }
                }
                Balloon newBalloon = ((GameObject)Instantiate(balloonPrefab)).GetComponent<Balloon>();
                newBalloon.transform.position = transform.position + new Vector3(xSpacer * (newColumn + xOffset), ySpacer * (numInColumn[newColumn] - (numRows-1)), 0);
                newBalloon.gameObject.name = " Balloon " + i.ToString("D2");
                newBalloon.color = colors[Random.Range(0, colors.Length)];
                balloons.Add(newBalloon);
                numInColumn[newColumn]++;
            }
        }

        private IEnumerator StartLaunching() {
            while(nextBalloon != balloons.Count) {
                yield return new WaitForSeconds(launchDelay);
                balloons[nextBalloon].Launch();
                nextBalloon++;
            }
       }
    }
}