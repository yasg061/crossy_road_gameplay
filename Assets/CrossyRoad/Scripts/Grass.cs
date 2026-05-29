using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private Transform treePrefab;

    public HashSet<int> Init(float z)
    {
        // Implementation for initializing grass with trees at position z
        transform.position = new Vector3(0, 0, z);

        HashSet<int> locations = new() { -6, 6}; // Start with the edges occupied by trees   

        int numTrees = Random.Range(1, 5); // Randomly decide how many trees to place (1 to 5)

        for (int i = 0; i < numTrees; i++)
        {
            Transform tree = Instantiate(treePrefab, transform);
            int xPos = Random.Range(-5, 6); // Random x position for the tree
            tree.position = new Vector3(xPos, 0.2f, z); // Place the tree at the correct position
            locations.Add(xPos); // Add the tree's x position to the set of occupied locations
        }

        return locations;
    } 
}
