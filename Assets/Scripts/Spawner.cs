using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipeGroupPrefab;
    public float spawnRate = 1f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        float randomY = Random.Range(-1f, 2f);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0f);
        Instantiate(pipeGroupPrefab, spawnPos, Quaternion.identity);
    }
}
