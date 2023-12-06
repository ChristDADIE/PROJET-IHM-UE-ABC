using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int id;
    TextAsset[] levels;

    string[] enemynames;
    public Enemy[] enemies;
    public float SpawnDistance = 15;
    List<Enemy> currentEnemies;

    void Start()
    {
        enemynames = new string[enemies.Length];
        for(int i = 0;i != enemies.Length;++i)
        {
            enemynames[i] = enemies[i].enemyName;
        }
        currentEnemies = new List<Enemy>();
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public void SetupAssets()
    {

    }
    
    public class Data
    {
        public string intro { get; set; }
        public List<Round> rounds { get; set; }
    }

    public class Round
    {
        public string type { get; set; }
        public int nb_pool { get; set; }
        public List<string> enemies { get; set; }
        public List<int> number { get; set; }
        public List<double> frequency { get; set; }
        public List<int> factor { get; set; }
        public string condition { get; set; }
        public int time { get; set; }
        public int? nb { get; set; }
    }
    int phase;
    Data data;
    float time;
    List<float> cooldown;
    int nb;
    int enemykilled;

    bool active;
    public void StartLevel()
    {
        SetupAssets();
        active = true;
        ResetVariables();
        data = JsonConvert.DeserializeObject<Data>(levels[Id].text);
    }

    void KillAll() // kill all enemies
    {
        foreach (Enemy enemy in currentEnemies)
        {
            Destroy(enemy);
        }
        currentEnemies = new List<Enemy>();
    }

    void ResetVariables()
    {
        time = 0;
        nb = 0;
        if (data.rounds.Count < phase)
        {
            cooldown = new List<float>(data.rounds[phase].enemies.Count);
            for (int enemyId = 0; enemyId != data.rounds[phase].enemies.Count; ++enemyId)
            {
                cooldown[enemyId] = 0;
            }
        }
        enemykilled = 0;
    }

    void SpawnEnemies(string type,int number,int factor)
    {
        int index = 0;
        while (index < enemynames.Length && enemynames[index] != type)
            index += 1;
        if(index == enemynames.Length)
        {
            Debug.Log("Nom d'ennemi non trouvé: " + type);
        }

        for(int i = 0;i != number;++i)
        {
            Enemy enemy = Instantiate<Enemy>(enemies[index]);
            enemy.Setup(this,factor, Random.insideUnitCircle * SpawnDistance);
        }

    }

    bool AreAllEnemiesDead()
    {
        return currentEnemies.Count == 0;
    }

    void LevelUpdate() // called approximately one time per fixed update
    {
        if(phase >= data.rounds.Count)
        {
            active = false;
            GetComponent<MainManager>().LevelEnded();
            return;
        }
        time += Time.fixedDeltaTime;
        for (int enemyId = 0; enemyId != data.rounds[phase].enemies.Count; ++enemyId)
        {
            cooldown[enemyId] += Time.fixedDeltaTime;
        }
        
        if (data.rounds[phase].condition == "time")
        {
            if (time > data.rounds[phase].time)
            {
                phase += 1;
                ResetVariables();
                KillAll();
                FixedUpdate();
                return;
            }
        }

        switch (data.rounds[phase].type)
        {
            case "pool":
                if(nb < data.rounds[phase].nb_pool)
                {
                    for(int enemyId = 0; enemyId != data.rounds[phase].enemies.Count;++enemyId)
                    {
                        if(cooldown[enemyId] > data.rounds[phase].frequency[enemyId])
                        {
                            cooldown[enemyId] = 0;
                            SpawnEnemies(data.rounds[phase].enemies[enemyId], data.rounds[phase].number[enemyId], data.rounds[phase].factor[enemyId]);
                            nb += 1;
                        }
                    }
                }

                if(nb >= data.rounds[phase].nb_pool)
                {
                    if(AreAllEnemiesDead())
                    {
                        phase += 1;
                        ResetVariables();
                        KillAll();
                        FixedUpdate();
                        return;
                    }
                }
                break;
            case "wave":

                for (int enemyId = 0; enemyId != data.rounds[phase].enemies.Count; ++enemyId)
                {
                    if (cooldown[enemyId] > data.rounds[phase].frequency[enemyId])
                    {
                        cooldown[enemyId] = 0;
                        SpawnEnemies(data.rounds[phase].enemies[enemyId], data.rounds[phase].number[enemyId], data.rounds[phase].factor[enemyId]);
                    }
                }
                if(enemykilled > data.rounds[phase].nb)
                {
                    phase += 1;
                    ResetVariables();
                    KillAll();
                    FixedUpdate();
                    return;
                }
                break;
            case "boss":
                if(time == 0)
                {
                    for (int enemyId = 0; enemyId != data.rounds[phase].enemies.Count; ++enemyId)
                    {
                        SpawnEnemies(data.rounds[phase].enemies[enemyId], data.rounds[phase].number[enemyId], data.rounds[phase].factor[enemyId]);
                    }
                }
                if (enemykilled > data.rounds[phase].nb)
                {
                    phase += 1;
                    ResetVariables();
                    KillAll();
                    FixedUpdate();
                    return;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        foreach(Enemy enemy in currentEnemies) // remove dead enemies
        {
            if(enemy.isDead)
            {
                currentEnemies.Remove(enemy);
                Destroy(enemy);
            }
        }


        if (active)
            LevelUpdate();
    }
}
