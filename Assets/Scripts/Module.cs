using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    public GameObject mesh;
    public GameObject[] exits;

    // Checks if the module collides with any module with one exception.
    public bool CollidesWithModules(GameObject exception) {
        // Get all colliders in the overlap sphere.
        Collider[] collisions = Physics.OverlapSphere(mesh.transform.position, GetSphereRadius(), ~0);
        Collider exceptionCollider = exception.GetComponent<Module>().mesh.GetComponent<Collider>();

        //GameObject collisionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //collisionSphere.transform.position = mesh.transform.position;
        //collisionSphere.transform.localScale = new Vector3(GetSphereRadius(), GetSphereRadius(), GetSphereRadius());

        Debug.Log("Sphere of radius " + GetSphereRadius() + " at " + mesh.transform.position + " collides with " + collisions.GetLength(0));

        // Check if there is a collider other than the exception collider or this object's collider.
        foreach (Collider collider in collisions) {
            if (collider != exceptionCollider && collider != mesh.GetComponent<Collider>()) {
                Debug.Log(gameObject.name + " collided with a module: " + collider.gameObject.transform.parent.gameObject.name);
                return true;
            }
        }

        return false;
    }

    // Get the radius of the sphere that encapsulates the entire 3D model.
    private float GetSphereRadius() {
        // Get the vertices and center of the 3D model.
        Vector3[] vertices = mesh.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 center = mesh.transform.position;

        // Find the maximum distance between the center and a vertex of the model.
        float maxDistance = 0;
        foreach (Vector3 vertex in vertices) {
            float distance = Vector3.Distance(center, transform.TransformPoint(vertex));

            if (distance > maxDistance) {
                maxDistance = distance;
            }
        }

        // This maximum distance is the radius of the smallest sphere to encapsulate the model.
        return 10;
    }
}
