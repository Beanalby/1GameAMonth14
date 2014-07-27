using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class CargoPickup : MonoBehaviour {
        public Transform SpawnPoint;
        public Cargo[] cargoPrefabs;
        private int numCities;

        private float spawnDelay = 4f;
        private float spawnRange = 1f;
        private Cargo cargo;

        public void Start() {
            numCities = GameObject.FindObjectsOfType<City>().Length;
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn() {
            yield return new WaitForSeconds(
                spawnDelay + Random.Range(-spawnRange, spawnRange));
            SpawnCargo();

        }

        public Cargo GetCargo() {
            if(cargo != null) {
                Cargo tmp = cargo;
                cargo = null;
                StartCoroutine(Spawn());
                return tmp;
            } else {
                return null;
            }
        }
        public bool HasCargo() {
            return cargo != null;
        }

        void SpawnCargo() {
            int randCity = Random.Range(0, numCities);
            cargo = (Instantiate(cargoPrefabs[randCity].gameObject,
                SpawnPoint.position, Quaternion.identity) as GameObject)
                    .GetComponent<Cargo>();
        }
    }
}