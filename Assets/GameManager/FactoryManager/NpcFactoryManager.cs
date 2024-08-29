using System.Collections.Generic;
using GameManager.Base;
using Npc.Aliens.Base;
using Npc.Base;
using Npc.Zombie.Base;
using Npc.Zombie.Boss.Base;
using UnityEngine;

namespace GameManager.FactoryManager
{
    public class NpcFactoryManager : ManagerBase
    {
        
        [Header("Zombies In Save")]
        public List<ZombieBase> zombiesInSave;
        public List<BossZombieBase> bossZombiesInSave;
        
        [Header("Aliens")]
        public List<AlienBase> alienInSave;

        [Header("Ufo Items In Save")] 
        public List<NpcBase> ufoItems;

        public GameObject SpawnRandomZombieBase(Vector3 spawnPoint)
        {
            var randomIndex = Random.Range(0, zombiesInSave.Count);
            var selectedZombie = zombiesInSave[randomIndex];
            
            spawnPoint += Vector3.forward; 
            var created = Instantiate(selectedZombie, spawnPoint, Quaternion.identity);
            created.gameObject.SetActive(true);

            var zombieSave = created.GetComponent<ZombieBase>();
            
            zombieSave.player = gameManager.player;
            zombieSave.npcFactoryManager = this;

            zombieSave.Init();
            return created.gameObject;
        }

        public GameObject SpawnAlien(Vector3 spawnPoint)
        {
            var alienList = alienInSave;
            
            var alienPrefab = alienList[Random.Range(0, alienList.Count)].gameObject;
            
            var newAlien = Instantiate(alienPrefab, spawnPoint, Quaternion.identity);
            newAlien.SetActive(true);

            Debug.Log("Spawned an alien: " + newAlien.name);

            var alienBase = newAlien.GetComponent<AlienBase>();
            alienBase.chasingState.SetChaseRegardlessOfRange(true);
            
            alienBase.player = gameManager.player;
            alienBase.npcFactoryManager = this;
            
            alienBase.Init();
            alienBase.TransitionToState(alienBase.chasingState);

            return newAlien;
        }
    }
}