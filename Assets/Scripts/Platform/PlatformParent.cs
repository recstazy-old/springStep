using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns platforms and deactivates them

public class PlatformParent : GameSys
{
    GameObject[] platform = new GameObject[8];
    GameObject lastSpawned;

    Vector2 defaultPos;

    void Start()
    {
        CachePlatforms();

        lastSpawned = platform[0];
        defaultPos = lastSpawned.transform.position;

        Spawn5Platforms();
    }

    void CachePlatforms()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            platform[i] = transform.GetChild(i).gameObject;
        }
    }
    
    void Spawn5Platforms()
    {
        for(int i = 1; i < 6; i ++)
        {
            SpawnPlatform();
        }
    }

    public void SpawnPlatform() // Spawn a Platform relative to last platform to make them go forward
    {
        bool moving = GetRandomBool();
        
        float x = lastSpawned.transform.position.x + Random.Range(5, 10);
        float y = Random.Range(-3, 5);
        Vector2 position = new Vector2(x, y);
        bool rotate = GetRandomBool();
        
        lastSpawned = FindInactiveIn(platform);

        if (lastSpawned != null)
        {
            lastSpawned.transform.position = position;

            Platform platformScript = lastSpawned.GetComponent<Platform>();
            platformScript.Moving = moving;

            if (rotate)
            {
                lastSpawned.transform.up = Vector3.left;
            }
            else
            {
                lastSpawned.transform.up = Vector3.up;
            }

            lastSpawned.SetActive(true);
            platformScript.sprite.color = new Color(1f, 0.4575472f, 0.4575472f);
            platformScript.Used = false;
        }
    }

    GameObject FindInactiveIn(GameObject[] pool) // Finds deactivated object in pool
    {
        GameObject inActive = null;

        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeSelf)
            {
                inActive = pool[i];
                break;
            }
        }

        return inActive;
    }

    public override void Restart()
    {
        for(int i = 0; i < platform.Length; i++)
        {
            platform[i].SetActive(false);
        }

        platform[0].transform.position = defaultPos;
        platform[0].transform.up = Vector2.up;
        lastSpawned = platform[0];
        lastSpawned.SetActive(true);

        Spawn5Platforms();
    }

    public override void GameOver()
    {
        StopAllCoroutines();
        platform[0].transform.up = Vector2.up;
    }

    bool GetRandomBool() // Because there's no Random bool generator
    {
        bool result;

        if (Random.Range(0, 100) < 30)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }
}
