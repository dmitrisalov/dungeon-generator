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

    [Range(1, 128)]
    public int moduleLimit;
    public int seed;
    public bool randomize;

    private List<Transform> unconnectedExits;

    public void GenerateDungeon(bool randomize) {
        DestroyAllChildren();

        // Chooses a random seed if desired.
        if (randomize) {
            seed = Random.Range(-SEED_RANGE, SEED_RANGE);
        }

        // Set the seed for the randomizer.
        Random.InitState(seed);

        // Instantiate a starting room.
        GameObject module = Object.Instantiate(GetRandomPrefab(ROOM), Vector3.zero, 
            Quaternion.identity);
        module.transform.parent = gameObject.transform;
        
        // Instantiate a list to store exits to which modules may be connected.
        unconnectedExits = new List<Transform>();
        AddModuleUnconnectedExits(module);

        // Add modules until the module limit is hit.
        Vector3 position = new Vector3(25, 0, 0);
        int numModules = 1;
        while (numModules < moduleLimit) {
            // Create a copy of unconnectedExits to be able to loop through and add to the list.
            List<Transform> currentExits = new List<Transform>(unconnectedExits);

            // Go through each unconnected exit.
            foreach (Transform exit in currentExits) {
                module = InstantiateCorrectPrefab(exit);
                ConnectModuleToExit(exit, module);

                // Check for collisions with other modules.
                if (module.GetComponent<Module>().CollidesWithModules(exit.parent.gameObject)) {
                    // Destroy the module and continue to the next iteration.
                    GameObject.DestroyImmediate(module);
                    continue;
                }

                // Add the modules exits to the unconnected exits list.
                AddModuleUnconnectedExits(module);

                // Remove the exit from the list since it is now connected.
                unconnectedExits.Remove(exit);

                // Check if the module limit was hit.
                numModules++;
                if (numModules >= moduleLimit) {
                    break;
                }
            }
        }
    }

    private GameObject InstantiateCorrectPrefab(Transform exit) {
        // Get the tag for the module to which the exit is connected.
        string exitModuleType = exit.parent.gameObject.tag;

        GameObject prefab = null;

        if (exitModuleType == "Room") {
            // Spawn a hallway.
            prefab = GetRandomPrefab(HALLWAY);
        }
        else if (exitModuleType == "Hallway") {
            // Spawn either a room or a junction.
            if (Random.Range(0, 2) == 0) {
                prefab = GetRandomPrefab(ROOM);
            }
            else {
                prefab = GetRandomPrefab(JUNCTION);
            }

        }
        else if (exitModuleType == "Junction") {
            // Spawn a hallway.
            prefab = GetRandomPrefab(HALLWAY);
        }

        // Instantiate the prefab and set it as a child of the DungeonGenerator's game object.
        GameObject module = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        module.transform.parent = gameObject.transform;

        return module;
    }

    private void ConnectModuleToExit(Transform exit, GameObject module) {
        module.transform.rotation = exit.rotation;
        module.transform.position = exit.position;
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

    private void AddModuleUnconnectedExits(GameObject module) {
        // Go through each child object of the module.
        foreach (Transform child in module.transform) {
            // Check if the child is an exit.
            if (child.name == "Exit") {
                unconnectedExits.Add(child);
            }
        }
    }

    public void DestroyAllChildren() {
        // Create an array of all the children of the object (modules of the dungeon).
        Transform[] children = new Transform[gameObject.transform.childCount];
        int arrayIndex = 0;
        foreach (Transform child in gameObject.transform) {
            children[arrayIndex] = child;
            arrayIndex++;
        }

        // Destroy all the transforms in the array.
        for (int i = 0; i < children.GetLength(0); i++) {
            GameObject.DestroyImmediate(children[i].gameObject);
        }

        /*  
            Note: The reason we don't directly loop through the children of the transform and
            destroy them from there is because that alters the underlying list of children. When we
            iterate through the list and destroy elements of it, the list is shortened, which causes
            the index to increase by 1. This results in destroying only odd children, or only half.
        */
    }
}
