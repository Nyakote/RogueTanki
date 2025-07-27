using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] Transform levelGround;
    [SerializeField] int enemyCount = 10; 
    [SerializeField] GameObject enemyRoot;
    [SerializeField] string minMod;
    [SerializeField] string maxMod;
    EnemyAI EA;
    List<Vector3> takenPos = new List<Vector3>();
    private float Xpos;
    private float Zpos;
    Vector3 scale;
    EnemyCreator ec;

    string hullName;
    string turretName;

    void Start()
    {
        ec = GetComponent<EnemyCreator>();

        scale = levelGround.localScale;
        takenPos.Add(new Vector3(0, 0.5f, 0));
        EnemySummoner();

    }

    void EnemySummoner()
    {
        Debug.Log("Start spawning");
        for (int i = 0; i < enemyCount; i++ )
        {
            EnemyPlacer();
        }
    }

    void EnemyPlacer()
    {

        int modHull = UnityEngine.Random.Range(Convert.ToInt32(minMod.Substring(1)), Convert.ToInt32(maxMod.Substring(1)) + 1);
        int modTurret = UnityEngine.Random.Range(Convert.ToInt32(minMod.Substring(1)), Convert.ToInt32(maxMod.Substring(1)) + 1);

        Vector3 position;
        do
        {
            EnemyPosCacl();
            position = new Vector3(Xpos, 0.5f, Zpos);
        }
        while (takenPos.Any(pos => pos == position));
        EA = enemyRoot.GetComponent<EnemyAI>();
        takenPos.Add(position);
        Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        GameObject enemy = Instantiate(enemyRoot, position, rotation).gameObject;

        EnemyCreator enemyCreator = enemy.GetComponent<EnemyCreator>();
        enemyCreator.Initialize(levelGround, modHull, modTurret);

    }

    void EnemyPosCacl()
    { 
        Xpos = UnityEngine.Random.Range(-scale.x / 2, scale.x / 2);
        Zpos = UnityEngine.Random.Range(-scale.z / 2, scale.z / 2);
    }
    
}
