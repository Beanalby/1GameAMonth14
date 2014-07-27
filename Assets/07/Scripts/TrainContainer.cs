using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class TrainContainer : MonoBehaviour {

        public Transform cargoPoint;
        private Cargo cargo = null;
        private int layerPickup, layerCity;

        public void Start() {
            layerPickup = LayerMask.NameToLayer("Pickup");
            layerCity = LayerMask.NameToLayer("City");
        }
        public void OnTriggerEnter(Collider other) {
            if(other.gameObject.layer == layerPickup) {
                PickupCargo(other.GetComponent<CargoPickup>());
            }
            if(other.gameObject.layer == layerCity) {
                TryDropoff(other.GetComponent<City>());
            }
        }

        public void PickupCargo(CargoPickup pickup) {
            if(cargo != null) {
                return; // we already have cargo, don't pickup
            }
            cargo = pickup.GetCargo();
            Debug.Log("+++ " + name + " got " + cargo + " from pickup");
            if(cargo != null) {
                cargo.transform.parent = cargoPoint;
                cargo.transform.localPosition = Vector3.zero;
            }
        }
        private void TryDropoff(City city) {
            if(cargo == null  || cargo.city != city.city) {
                return;
            }
            Debug.Log("+++ " + name + " dropping off at " + city.city);
            GameDriver.Instance.CargoDelivered(cargo);
            Destroy(cargo.gameObject);
            cargo = null;
        }
    }
}