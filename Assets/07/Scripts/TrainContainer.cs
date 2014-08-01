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
            if(cargo == null) {
                return; // no cargo to pickup
            }
            cargo.transform.parent = cargoPoint;
            cargo.transform.localPosition = Vector3.zero;
            GameDriver.Instance.CargoPicked(cargo);
        }
        private void TryDropoff(City city) {
            if(cargo == null  || cargo.city != city.city) {
                return;
            }
            GameDriver.Instance.CargoDelivered(cargo);
            Destroy(cargo.gameObject);
            cargo = null;
        }
    }
}