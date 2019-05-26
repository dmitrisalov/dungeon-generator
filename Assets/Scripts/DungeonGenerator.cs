using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
    private const int SEED_RANGE = 1000000;
    private const int HALLWAY = 0;
    private const int JUNCTION = 1;
    private const int ROOM = 2;

    public GameObject[] hallwayPrefabs;
    public GameObject[] junctionPrefabs;
    public GameObject[] roomPrefabs;

    [Range(1, 64)]
    public int moduleLimit;
    public int seed;

    public void GenerateDungeon(bool randomize) {
        // Chooses a random seed if desired.
        if (randomize) {
            seed = Random.Range(-SEED_RANGE, SEED_RANGE);
        }

        // Set the seed for the randomizer.
        Random.InitState(seed);

        // Instantiate a starting module, which should be a room.
        GameObject prefab = GetRandomPrefab(ROOM);

        for (int i = 0; i < moduleLimit - 1; i++) {

        }
    }

    private GameObject GetRandomPrefab(int type) {
        GameObject prefab = null;

        // Based on the type of prefab needed, return a random prefab from each list.
        if (type == HALLWAY) {
            prefab = hallwayPrefabs[Random.Range(0, hallwayPrefabs.GetLength(0))];
        }
        else if (type == JUNCTION) {
            prefab = junctionPrefabs[Random.Range(0, junctionPrefabs.GetLength(0))];
        }
        else if (type == ROOM) {
            prefab = roomPrefabs[Random.Range(0, roomPrefabs.GetLength(0))];
        }

        return prefab;
    }
}
