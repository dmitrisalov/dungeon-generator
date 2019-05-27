using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the map generator.
        DungeonGenerator dungeonGenerator = (DungeonGenerator)target;

        // Draw the regular inspector for public members.
        DrawDefaultInspector();

        // Add a reset button that deletes all children.
        if (GUILayout.Button("Reset")) {
            dungeonGenerator.DestroyAllChildren();
        }

        // Add a generate button that generates a new dungeon.
        if (GUILayout.Button("Generate")) {
            if (dungeonGenerator.randomize) {
                dungeonGenerator.GenerateDungeon(true);
            }
            else {
                dungeonGenerator.GenerateDungeon(false);
            }
        }
    }
}
