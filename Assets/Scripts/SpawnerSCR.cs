using System;
using UnityEngine;

public class SpawnerSCR : MonoBehaviour
{
    public abstract class Spawner<T> : MonoBehaviour where T : Component
    {
        protected event Action<Transform> OnSpawn;

        [SerializeField] protected SpawnedObjectsManager _manager;
        [SerializeField] protected float[] _xAxisSpawnPositions;
        [SerializeField] protected float _distanceToCam;

        [SerializeField] protected Vector3 _startPosition;
        [SerializeField] protected float _startStep = 50f;
        private float _lastSpawnedPos;

        protected virtual void Start()
        {
            SpawnObjects(_startPosition.z, _distanceToCam, _startStep);
        }

        public void SpawnObjects(float startPos, float lastPos, float step, float xPos = float.MaxValue)
        {
            float zPos = startPos;
            for (; zPos < lastPos; zPos += _startStep)
            {
                if (xPos == float.MaxValue)
                {
                    SpawnObject(new Vector3(GetRandomXPos(), _startPosition.y, zPos));
                }
            }

            _lastSpawnedPos = zPos;
        }

        public abstract T GetObjectPool();

        protected virtual void InitObject(Transform gameObject)
        {
        }

        protected abstract bool IsSpawnedOnRoad { get; }

        private void SpawnObject(Vector3 position)
        {
            var spawnedObject = GetObjectPool().GetComponent<Transform>();

            InitObject(spawnedObject);
            spawnedObject.transform.position = position;

            spawnedObject.gameObject.SetActive(true);
            if (IsSpawnedOnRoad)
            {
                spawnedObject.transform.SetParent(_manager.transform);
            }

            OnSpawn?.Invoke(spawnedObject);
        }

        private float GetRandomXPos()
        {
            return _xAxisSpawnPositions[UnityEngine.Random.Range(0, _xAxisSpawnPositions.Length)];
        }
    }

    public class SpawnedObjectsManager : MonoBehaviour
    {
    }

    public class RoadSpawner : Spawner<Transform>
    {
        [SerializeField] private RoadPool _roadPool;

        private void OnEnable()
        {
            Car.passedHundredMeters += Spawn;
        }

        private void OnDisable()
        {
            Car.passedHundredMeters -= Spawn;
        }

        public override Transform GetObjectPool()
        {
            return _roadPool.transform;
        }

        private void Spawn()
        {
            SpawnObjects(0, -1, 0.2f, 3.0f);
        }

        protected override bool IsSpawnedOnRoad => true;
    }

    public class TerrainSpawner : Spawner<Transform>
    {
        [SerializeField] private TerrainPool _terrainPools;
        [SerializeField] private float _distToCover;

        private void OnEnable()
        {
            Car.passedHundredKilometers += Spawn;
        }

        private void OnDisable()
        {
            Car.passedHundredKilometers -= Spawn;
        }

        private void Spawn()
        {
            SpawnObjects(1, 100, _distToCover, 3.5f);
        }

        public override Transform GetObjectPool()
        {
            return _terrainPools.transform;
        }

        protected override bool IsSpawnedOnRoad => false;
    }

    public class TerrainPool : MonoBehaviour
    {
    }

    public class Car : MonoBehaviour
    {
        public static Action passedHundredMeters;
        public static Action passedHundredKilometers { get; set; }
    }

    public class RoadPool : Spawner<Transform>
    {
        public override Transform GetObjectPool()
        {
            throw new NotImplementedException();
        }

        protected override bool IsSpawnedOnRoad { get; }
    }

    // //////////////////////////////////////////////////
    public class Enemy : MonoBehaviour
    {
        public static Action passedHundredMeters;
        public static Action passedHundredKilometers { get; set; }
        public static Action enemyKill { get; set; }
    }

    public class EnemySpawner : Spawner<Transform>
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private float _distToCover;

        private bool canKillEnemies = false;

        private void OnEnable()
        {
            Enemy.passedHundredMeters += Spawn;
            if (canKillEnemies)
            {
                Enemy.enemyKill += Spawn;
            }
        }

        private void OnDisable()
        {
            Enemy.passedHundredMeters -= Spawn;
            Enemy.enemyKill -= Spawn;
        }

        private void Spawn()
        {
            SpawnObjects(1, 100, _distToCover, 3.5f);
        }

        public void SetCanKillEnemies(bool canKill)
        {
            canKillEnemies = canKill;
            if (canKill)
            {
                Enemy.enemyKill += Spawn;
            }
            else
            {
                Enemy.enemyKill -= Spawn;
            }
        }

        public override Transform GetObjectPool()
        {
            return _enemyPool.transform;
        }

        protected override bool IsSpawnedOnRoad => false;
    }
}

public class EnemyPool
{
    public Transform transform;
}