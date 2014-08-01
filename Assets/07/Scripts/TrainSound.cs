using UnityEngine;
using System.Collections;


namespace onegam_1407 {
    [RequireComponent(typeof(AudioSource))]
    public class TrainSound : MonoBehaviour {

        public void Update() {
            if(GameDriver.Instance.IsRunning && !audio.isPlaying) {
                // start the audio at a random position
                audio.time = Random.Range(0, audio.clip.length);
                audio.Play();
            }
            if(audio.isPlaying && !GameDriver.Instance.IsRunning) {
                audio.Stop();
            }
        }
    }
}