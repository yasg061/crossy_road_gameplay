using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private float spawnTime = 2f;


    void Start()
    {
        InvokeRepeating("SpawnCar", 0f, spawnTime);
    }

    private void SpawnCar()
    {
        Instantiate(carPrefab, transform.position, transform.rotation);
    }

}
