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
        private bool isSpawning = false;

        public void Awake() {
            numCities = GameObject.FindObjectsOfType<City>().Length;
            city = transform.parent.GetComponent<City>();
        }

        public void SpawnAfterDelay() {
            StartCoroutine(_SpawnAfterDelay());
        }

        private IEnumerator _SpawnAfterDelay() {
            isSpawning = true;
            yield return new WaitForSeconds(
                spawnDelay + Random.Range(-spawnRange, spawnRange));
            SpawnCargo();
        }

        public Cargo GetCargo() {
            if(cargo != null) {
                Cargo tmp = cargo;
                cargo = null;
                return tmp;
            } else {
                return null;
            }
        }
        public bool CanSpawn() {
            return !HasCargo() && !isSpawning;
        }
        public bool HasCargo() {
            return cargo != null;
        }

        public void SpawnCargo() {
            isSpawning = false;
            CityName randCity;
            do {
                randCity = (CityName)City.CityNames.GetValue(Random.Range(0, numCities));
            } while(randCity == city.city);

            cargo = (Instantiate(cargoPrefab.gameObject,
                SpawnPoint.position, Quaternion.identity) as GameObject)
                    .GetComponent<Cargo>();
            cargo.city = randCity;
        }
        public override string ToString() {
            return city + " pickup" + (HasCargo() ? " (Cargo)" : "(NoCargo)");
        }
    }
}