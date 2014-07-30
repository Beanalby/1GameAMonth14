using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1407 {
    public class CargoPickup : MonoBehaviour {
        public Transform SpawnPoint;
        public Cargo cargoPrefab;
        private int numCities;
        City city;

        private float spawnDelay = 4f;
        private float spawnRange = 1f;
        private Cargo cargo;

        public void Start() {
            numCities = GameObject.FindObjectsOfType<City>().Length;
            city = transform.parent.GetComponent<City>();
            SpawnCargo();
        }

        private IEnumerator SpawnAfterDelay() {
            yield return new WaitForSeconds(
                spawnDelay + Random.Range(-spawnRange, spawnRange));

            SpawnCargo();

        }

        public Cargo GetCargo() {
            if(cargo != null) {
                Cargo tmp = cargo;
                cargo = null;
                StartCoroutine(SpawnAfterDelay());
                return tmp;
            } else {
                return null;
            }
        }
        public bool HasCargo() {
            return cargo != null;
        }

        void SpawnCargo() {
            CityName randCity;
            do {
                randCity = (CityName)City.CityNames.GetValue(Random.Range(0, numCities));
            } while(randCity == city.city);

            cargo = (Instantiate(cargoPrefab.gameObject,
                SpawnPoint.position, Quaternion.identity) as GameObject)
                    .GetComponent<Cargo>();
            cargo.city = randCity;
        }
    }
}