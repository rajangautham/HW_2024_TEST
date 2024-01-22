using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private float speed;

    [System.Serializable]
    public class PlayerData
    {
        public float speed = 3f;
    }

    void Start()
    {
        StartCoroutine(LoadPlayerData());
    }

void Update()
{
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
    transform.position += movement * speed * Time.deltaTime;
}

    IEnumerator LoadPlayerData()
    {
        // Load JSON data from the URL
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Deserialize JSON into C# object
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(webRequest.downloadHandler.text);

                // Set the speed
                speed = playerData.speed;
            }
            else
            {
                Debug.LogError("Failed to load player data: " + webRequest.error);
            }
        }
    }
}
