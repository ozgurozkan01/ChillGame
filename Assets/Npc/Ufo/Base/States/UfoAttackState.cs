using System.Collections;
using System.Collections.Generic;
using Npc.Ufo.Base.States.Base;
using Npc.Ufo.Base.States.Base;
using Npc.Zombie.Weapon.Rocket;
using UnityEngine;

namespace Npc.Ufo.Base.States
{
    [System.Serializable]
    public class UfoAttackState : UfoState
    {
        private UfoSubAttackState _ufoSubAttackState;
        public UfoSpawnAlien ufoSpawnAlien;
        public UfoShootBlastAlien ufoShootBlastAlien;
        
        public override void Init(UfoBase ufoInGame)
        {
            base.Init(ufoInGame);
            ufoSpawnAlien.Init(this);
            ufoShootBlastAlien.Init(this);
            
        }

        public override void Enter()
        {
            Debug.Log("Attack State");
            SetState(ufoSpawnAlien);
        }

        public override void Update()
        {
            _ufoSubAttackState.Update();
        }

        public override void Exit()
        {
            _ufoSubAttackState.Exit();
        }
        
        public void SetState(UfoSubAttackState newState)
        {
            _ufoSubAttackState?.Exit();
            _ufoSubAttackState = GetASubAttackState(newState);
            _ufoSubAttackState.Enter();
        }

        private UfoSubAttackState GetASubAttackState(UfoSubAttackState newState) { // change this shit
            
            if (_ufoSubAttackState == null)
            {
                // First time entering the state
                return newState;
            }
            
            if (_ufoSubAttackState == ufoSpawnAlien)
            {
                return ufoShootBlastAlien;
            }
            if (_ufoSubAttackState == ufoShootBlastAlien)
            {
                return ufoSpawnAlien;
            }
            return null;
        }
    }

    public class UfoSubAttackState
    {
        private UfoAttackState _ufoAttackState;

        public virtual void Init(UfoAttackState ufoAttackState)
        {
            _ufoAttackState = ufoAttackState;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }

        public UfoAttackState UfoAttackState
        {
            get => _ufoAttackState;
            set => _ufoAttackState = value;
        }
    }

    [System.Serializable]
    public class UfoSpawnAlien : UfoSubAttackState
    {
        private int spawnCount = 0;
        private int MaxSpawns = 3;

        public override void Enter()
        {
            spawnCount = 0;
            Debug.Log("Entering Spawn Alien State");
            UfoAttackState.ufo.StartCoroutine(SpawnAliensCoroutine());
        }

        public override void Exit()
        {
            Debug.Log("Exiting Spawn Alien State");
            UfoAttackState.ufo.StopAllCoroutines();
        }

        private IEnumerator SpawnAliensCoroutine()
        {
            while (spawnCount < MaxSpawns)
            {
                SpawnAlien();
                spawnCount++;
                yield return new WaitForSeconds(1f); // Optional delay between spawns
            }

            // After spawning aliens, transition to cooldown state
            UfoAttackState.ufo.SetState(UfoAttackState.ufo.ufoCoolDownState);
        }

        private void SpawnAlien()
        {
            var alienList = UfoAttackState.ufo.npcFactoryManager.alienInSave;
            
            var alienPrefab = alienList[Random.Range(0, alienList.Count)].gameObject;
        
            var spawnPosition = UfoAttackState.ufo.agent.transform.position + new Vector3(0, 1, 0); // Example position adjustment
            var newAlien = Object.Instantiate(alienPrefab, spawnPosition, Quaternion.identity);
            newAlien.SetActive(true);

            Debug.Log("Spawned an alien: " + newAlien.name);
        }

    }

    [System.Serializable]
    public class UfoShootBlastAlien : UfoSubAttackState
    {
        public Transform laserStartPoint;
        public LineRenderer laserLineRenderer;

        private Vector3 _playerPosition;

        public override void Enter()
        {
            Debug.Log("Entering Blasting State");
            _playerPosition = UfoAttackState.ufo.player.position;
            Transform transform;
            (transform = UfoAttackState.ufo.transform).LookAt(_playerPosition);
            UfoAttackState.ufo.transform.Rotate(-transform.rotation.eulerAngles.x*2, 0, 0);
            
            laserLineRenderer.enabled = true;
            laserLineRenderer.positionCount = 2;
            laserLineRenderer.SetPosition(0, laserStartPoint.position);
            laserLineRenderer.SetPosition(1, _playerPosition);
            
            UfoAttackState.ufo.StartCoroutine(BlastingAttackCoroutine());
        }

        public override void Update()
        {
            if (laserLineRenderer.enabled)
            {
                laserLineRenderer.SetPosition(0, laserStartPoint.position);
                laserLineRenderer.SetPosition(1, _playerPosition);
            }
        }

        public override void Exit()
        {
            laserLineRenderer.enabled = false;
            
            var ufoTransform = UfoAttackState.ufo.transform;
            Vector3 eulerRotation = ufoTransform.rotation.eulerAngles;
            eulerRotation.x = 0; // Adjust the pitch (X-axis) as needed
            ufoTransform.rotation = Quaternion.Euler(eulerRotation);
            
            UfoAttackState.ufo.StopAllCoroutines();

        }
        private IEnumerator BlastingAttackCoroutine()
        {

            yield return new WaitForSeconds(2f);

            PerformBlastAttack();

            UfoAttackState.ufo.SetState(UfoAttackState.ufo.ufoCoolDownState);
        }

        private void PerformBlastAttack()
        {
            var direction = _playerPosition - laserStartPoint.position;

            if (Physics.Raycast(laserStartPoint.position, direction, out var hit))
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                    Debug.Log("Player hit by blast attack!");
                
                Debug.Log("Rocket shot towards: " + hit.point);
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }

    }
}