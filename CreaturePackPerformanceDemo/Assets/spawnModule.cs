using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnModule : MonoBehaviour
{
    public List<Transform> spawnObjs;
    public int spawnNum;
    public float spawnRadius;
    public int spawnDelay;
    public int maxObjs;
    private int spawnCntdown;
    private List<Transform> objectPool = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        spawnCntdown = 0;
    }

    private void runSpawn()
    {
        if(spawnObjs.Count == 0)
        {
            return;
        }

        var selfXform = gameObject.transform;
        for (int i = 0; i < spawnNum; i++)
        {
            Vector2 perturbVec = new Vector2(Random.Range(-spawnRadius, spawnRadius), Random.Range(0, spawnRadius));
            Vector2 spawnPos = new Vector2(selfXform.position.x, selfXform.position.y) + perturbVec;

            if (objectPool.Count <= maxObjs)
            {
                // Create a new character
                int spawnIdx = Random.Range(0, spawnObjs.Count);
                var spawnChar = spawnObjs[spawnIdx];

                var newObj = Instantiate(spawnChar, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                newObj.gameObject.layer = 9;
                newObj.GetComponent<pigletAgent>().lifetime = 300;
                objectPool.Add(newObj);
            }
            else
            {
                // Re-use character from objectPool
                foreach(var curObj in objectPool)
                {
                    var curAgent = curObj.GetComponent<pigletAgent>();
                    if (curAgent.lifetime <= 0)
                    {
                        curAgent.lifetime = 300;
                        curObj.position = spawnPos;
                        curObj.gameObject.SetActive(true);
                        curAgent.ResetCharState();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCntdown <= 0)
        {
            spawnCntdown = spawnDelay;
            runSpawn();
        }
        spawnCntdown--;
    }
}
