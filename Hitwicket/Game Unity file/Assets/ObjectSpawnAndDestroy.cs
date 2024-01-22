using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ObjectSpawnAndDestroy : MonoBehaviour
{
    public GameObject currentPlatform;
    public GameObject spawnPrefab;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public GameObject spawnPoint4;

    public TextMeshPro countdownText; // Use TextMeshPro for 3D text

    private float minPulpitDestroyTime = 4f;
    private float maxPulpitDestroyTime = 5f;
    private float pulpitSpawnTime = 2.5f;
    private float nextPlatformSpawnTime;

    private float countdownTimer; // Timer for countdown display

    [System.Serializable]
    private class PulpitData
    {
        public float min_pulpit_destroy_time;
        public float max_pulpit_destroy_time;
        public float pulpit_spawn_time;
    }

    private int platformCount; // Track the number of platforms on the screen

    void Start()
    {
        nextPlatformSpawnTime = Time.time + pulpitSpawnTime;
        StartCoroutine(SpawnAndDestroyObjects());
        StartCoroutine(LoadPulpitData());
    }

    IEnumerator SpawnAndDestroyObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(pulpitSpawnTime);

            // Choose a random spawn point for object B, excluding the current platform's position
            GameObject spawnPointForB = GetRandomSpawnPoint(currentPlatform.transform.position);

            // Ensure that there are not more than two platforms on the screen
            while (platformCount >= 1)
            {
                yield return null;
            }

            // Increment the platform count
            platformCount++;

            // Spawn object B at the chosen spawn point
            Instantiate(spawnPrefab, spawnPointForB.transform.position, Quaternion.identity);

            // Calculate a random destroy time between min and max values
            // float destroyTime = Random.Range(minPulpitDestroyTime, maxPulpitDestroyTime);
            float destroyTime = Random.Range(4,5);

            // Reset the countdown timer
            countdownTimer = destroyTime;

            // Start countdown display
            StartCoroutine(CountdownDisplay(destroyTime));

            // Wait for the destroy time
            yield return new WaitForSeconds(destroyTime);

            // Decrement the platform count
            platformCount--;

            // Destroy object A
            Destroy(currentPlatform);

            // Update the next platform spawn time
            nextPlatformSpawnTime = Time.time + pulpitSpawnTime;
        }
    }

    IEnumerator CountdownDisplay(float destroyTime)
    {
        float step = 0.01f; // Adjust the step value as needed for your desired precision

        while (countdownTimer > 0)
        {
            countdownText.text = countdownTimer.ToString("F2"); // Update countdown display
            yield return new WaitForSeconds(step);
            countdownTimer -= step;
        }

        // Reset countdown text after the platform is destroyed
        countdownText.text = "0.00";
    }

    GameObject GetRandomSpawnPoint(Vector3 excludePosition)
    {
        // Choose a random index from 1 to 4
        int randomIndex = Random.Range(1, 5);

        // Get the chosen spawn point based on the random index, excluding the excludePosition
        GameObject chosenSpawnPoint = null;

        // Ensure that the new platform does not spawn in the same position as the current platform
        do
        {
            switch (randomIndex)
            {
                case 1:
                    chosenSpawnPoint = spawnPoint1;
                    break;
                case 2:
                    chosenSpawnPoint = spawnPoint2;
                    break;
                case 3:
                    chosenSpawnPoint = spawnPoint3;
                    break;
                case 4:
                    chosenSpawnPoint = spawnPoint4;
                    break;
            }

            // Ensure the new platform does not spawn in the same position as the current platform
        } while (Vector3.Distance(chosenSpawnPoint.transform.position, excludePosition) < 1f);

        return chosenSpawnPoint;
    }

    IEnumerator LoadPulpitData()
    {
        // Load JSON data from the URL
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Deserialize JSON into C# object
                PulpitData pulpitData = JsonUtility.FromJson<PulpitData>(webRequest.downloadHandler.text);

                // Set the values
                minPulpitDestroyTime = pulpitData.min_pulpit_destroy_time;
                maxPulpitDestroyTime = pulpitData.max_pulpit_destroy_time;
                pulpitSpawnTime = pulpitData.pulpit_spawn_time;

                // Update the next platform spawn time based on the loaded data
                nextPlatformSpawnTime = Time.time + pulpitSpawnTime;
            }
            else
            {
                Debug.LogError("Failed to load pulpit data: " + webRequest.error);
            }
        }
    }
}
