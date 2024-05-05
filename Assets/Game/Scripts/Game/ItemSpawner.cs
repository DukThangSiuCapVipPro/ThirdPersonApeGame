using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform minX, maxX, minZ, maxZ;

    float width, height;
    Dictionary<string, bool> itemPoses = new Dictionary<string, bool>();

    void Awake()
    {
        width = maxX.position.x - minX.position.x;
        height = maxZ.position.z - minZ.position.z;
    }

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            for (float x = minX.position.x; x < maxX.position.x; x += 3f)
            {
                for (float z = minZ.position.z; z < maxZ.position.z; z += 3f)
                {
                    string pos = $"{x}-{z}";
                    if (itemPoses.ContainsKey(pos))
                    {
                        if (!itemPoses[pos])
                        {
                            itemPoses[pos] = true;
                            SpawnItemAt(x, z);
                        }
                    }
                    else
                    {
                        itemPoses.Add(pos, true);
                        SpawnItemAt(x, z);
                    }
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    void SpawnItemAt(float x, float z)
    {
        GameObject obj = SimplePool.Spawn(GameAssetManager.Instance.items[0], new Vector3(x, 1, z), Quaternion.identity);
        obj.transform.SetParent(transform);
    }
}
