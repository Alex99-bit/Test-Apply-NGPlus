using UnityEngine;
using System.Collections;

public class NPCSpawnManager : MonoBehaviour
{
    [Header("NPC Settings")]
    public GameObject[] npcPrefabs;
    public Transform[] spawnPoints;
    
    [Header("Spawn Settings")]
    public int maxNPCs = 10;
    public float spawnInterval = 2f;
    public bool startSpawningOnStart = true;

    private int currentNPCCount = 0;
    private bool isSpawning = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep this instance across scenes
    }

    void Start()
    {
        if (startSpawningOnStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning && currentNPCCount < maxNPCs)
        {
            SpawnNPC();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNPC()
    {
        if (npcPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No NPC prefabs or spawn points assigned!");
            return;
        }

        // Get random NPC and spawn point
        GameObject randomNPC = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn the NPC
        GameObject spawnedNPC = Instantiate(
            randomNPC, 
            randomSpawnPoint.position, 
            randomSpawnPoint.rotation
        );

        currentNPCCount++;

        // Optional: Add event when NPC is destroyed to decrease count
        var destroyableComponent = spawnedNPC.AddComponent<DestroyableNPC>();
        destroyableComponent.OnNPCDestroyed += OnNPCDestroyed;
    }

    private void OnNPCDestroyed()
    {
        currentNPCCount--;
        
        // Restart spawning if we're below max and spawning is still enabled
        if (currentNPCCount < maxNPCs && isSpawning)
        {
            StartCoroutine(SpawnRoutine());
        }
    }
}

// Helper class to track NPC destruction
public class DestroyableNPC : MonoBehaviour
{
    public System.Action OnNPCDestroyed;

    private void OnDestroy()
    {
        OnNPCDestroyed?.Invoke();
    }
}