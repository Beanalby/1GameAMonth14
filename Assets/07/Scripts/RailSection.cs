using UnityEngine;
using System.Collections;

namespace onegam_1407 {

    public abstract class RailSection : MonoBehaviour {
        public abstract Vector3 GetPosition(float distance);
        public abstract float GetRemainder(float distance);
    }
}