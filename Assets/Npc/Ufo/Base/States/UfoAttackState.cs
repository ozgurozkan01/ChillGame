using System.Collections;
using System.Collections.Generic;
using Npc.Ufo.Base.States.Base;
using Npc.Ufo.Base.States.Base;
using Npc.Zombie.Weapon.Rocket;
using UnityEngine;
using UnityEngine.Serialization;

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
            ufo.agent.SetDestination(ufo.agent.transform.position);
            SetState(ufoShootBlastAlien);
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
        
        public int maxSpawns = 4;
        public int spawnCount = 0;
        public int currentSpawnCycle = 1;
        public float duration;
        public float firstYPos;

        public override void Enter()
        {
            spawnCount = 0;
            Debug.Log("Entering Spawn Alien State");
            UfoAttackState.ufo.StartCoroutine(GoDown());

        }

        public override void Exit()
        {
            Debug.Log("Exiting Spawn Alien State");
            UfoAttackState.ufo.StopAllCoroutines();
        }
     
        private IEnumerator SpawnAliensCoroutine()
        {
            UfoAttackState.ufo.agent.enabled = false;
            while (spawnCount < currentSpawnCycle)
            {
                UfoAttackState.ufo.npcFactoryManager.SpawnAlien(UfoAttackState.ufo.transform.position);
                spawnCount++;
                yield return new WaitForSeconds(1f); // Optional delay between spawns
            }
            UfoAttackState.ufo.agent.enabled = true;
            
            UfoAttackState.ufo.StartCoroutine(GoUp());
            
            currentSpawnCycle++;
            if (currentSpawnCycle > maxSpawns)
            {
                currentSpawnCycle = 1;
            }
            
        }

        private IEnumerator GoDown()
        {
            var ufoTransform = UfoAttackState.ufo.transform;
            var startPosition = ufoTransform.position;
            firstYPos = startPosition.y;
            var targetPositionDown = new Vector3(startPosition.x, 10, startPosition.z);

            // Smoothly move down to y = 0
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                ufoTransform.position = Vector3.Lerp(startPosition, targetPositionDown, elapsedTime / duration);
                yield return null;
            }
            UfoAttackState.ufo.StartCoroutine(SpawnAliensCoroutine());
        }
        private IEnumerator GoUp()
        {
            var ufoTransform = UfoAttackState.ufo.transform;
            var startPosition = ufoTransform.position;
            var targetPositionDown = new Vector3(startPosition.x, firstYPos, startPosition.z);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                ufoTransform.position = Vector3.Lerp(startPosition, targetPositionDown, elapsedTime / duration);
                yield return null;
            }
            
            UfoAttackState.ufo.SetState(UfoAttackState.ufo.ufoCoolDownState);
        }
    }

    [System.Serializable]
    public class UfoShootBlastAlien : UfoSubAttackState
    {
        public Transform laserStartPoint;
        public LineRenderer laserLineRenderer;

        private Vector3 _playerPosition;
        public float duration = 1f;
        public float blastDuration = 0.5f; // Adjust this value to control the speed
        public override void Enter()
        {
            Debug.Log("Entering Blasting State");
            laserLineRenderer.positionCount = 2;
            UfoAttackState.ufo.StartCoroutine(SmoothLookAt()); // Adjust duration as needed
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
        
        private IEnumerator SmoothLookAt()
        {
            var ufoTransform = UfoAttackState.ufo.transform;
            _playerPosition = UfoAttackState.ufo.player.position;

            Quaternion startRotation = ufoTransform.rotation;
            Vector3 directionToPlayer = (_playerPosition - ufoTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                ufoTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
                yield return null;
            }
            ufoTransform.rotation = targetRotation;

            UfoAttackState.ufo.StartCoroutine(BlastingAttackCoroutine());
        }
        
        private IEnumerator BlastingAttackCoroutine()
        {
            yield return new WaitForSeconds(0.2f);

            laserLineRenderer.enabled = true;
            laserLineRenderer.SetPosition(0, laserStartPoint.position);

            Vector3 startPosition = laserStartPoint.position;
            Vector3 endPosition = _playerPosition;

            float elapsedTime = 0f;

            while (elapsedTime < blastDuration)
            {
                elapsedTime += Time.deltaTime;
                Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                laserLineRenderer.SetPosition(1, currentPos);
                yield return null;
            }
            laserLineRenderer.SetPosition(1, endPosition);
            
            PerformBlastAttack();
            
            yield return new WaitForSeconds(1f);
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