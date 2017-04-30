using UnityEngine;
using System.Collections;


namespace onegam_1407 {
    [RequireComponent(typeof(AudioSource))]
    public class TrainSound : MonoBehaviour {

        public void Update() {
            if(GameDriver.Instance.IsRunning && !GetComponent<AudioSource>().isPlaying) {
                // start the audio at a random position
                GetComponent<AudioSource>().time = Random.Range(0, GetComponent<AudioSource>().clip.length);
                GetComponent<AudioSource>().Play();
            }
            if(GetComponent<AudioSource>().isPlaying && !GameDriver.Instance.IsRunning) {
                GetComponent<AudioSource>().Stop();
            }
        }
    }
}