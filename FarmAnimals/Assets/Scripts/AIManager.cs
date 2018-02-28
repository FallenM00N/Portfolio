using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    [SerializeField] GameObject m_pig = null;
    [SerializeField] GameObject m_chicken = null;
    [SerializeField] [Range(0, 100)] int m_numOfSpawns = 15;
    [SerializeField] List<Transform> m_spawnPoints = null;

    void Start()
    {
        SpawnAnimals();
    }

    void SpawnAnimals()
    {
        for (int i = 0; i < m_numOfSpawns; ++i)
        {
            int x = (Random.Range(0, m_spawnPoints.Count));
            Transform location = m_spawnPoints[x];
            int y = Random.Range(0, 2);
            GameObject animal = (y == 1) ? m_pig : m_chicken;

            SpawnAnimalAtLocation(animal, location);
        }
    }

    void SpawnAnimalAtLocation(GameObject animal, Transform location)
    {
        Vector3 direction = Vector3.forward * 10.0f - location.position;
        Quaternion rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        Vector3 offset = new Vector3(Random.value * 3.0f, 0.0f, Random.value * 3.0f);
        Instantiate(animal, location.position + offset, rotation, location);
    }
}
