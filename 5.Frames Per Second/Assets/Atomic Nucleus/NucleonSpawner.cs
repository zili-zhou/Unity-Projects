using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
    public float timeBetweenSpawns;

    public float SpawnDistance;

    public Nucleon[] nucleonsPreferbs;

    float timeSinceLastSpawn;

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            SpawnNucleon();
        }
    }

    void SpawnNucleon()
    {
        Nucleon nucleon = nucleonsPreferbs[Random.Range(0, nucleonsPreferbs.Length)];
        Nucleon spawn = Instantiate<Nucleon>(nucleon);
        spawn.transform.localPosition = Random.onUnitSphere * SpawnDistance;
    }

}
