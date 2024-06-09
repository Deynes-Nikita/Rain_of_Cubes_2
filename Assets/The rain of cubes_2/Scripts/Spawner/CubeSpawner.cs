using System;
using UnityEngine;
using Item;

namespace Spawner
{
    [RequireComponent(typeof(BoxCollider))]
    public class CubeSpawner : Spawner<Cube>
    {
        [SerializeField] private float _repeatRate = 0.5f;

        private BoxCollider _spawnArea;
        private float _lastSpawnTime;

        public event Action<Vector3> CubeIsDead;

        protected override void SetParameters()
        {
            _spawnArea = GetComponent<BoxCollider>();
            InvokeRepeating(nameof(SpawnedCube), 0f, _repeatRate);
        }

        protected override Cube CreateFunc()
        {
            Cube cube = Instantiate(SpawnedObject);
            cube.Died += Release;

            base.CreateFunc();

            return cube;
        }

        protected override void ActionOnDestroy(Cube cube)
        {
            cube.Died -= Release;
            base.ActionOnDestroy(cube);
        }

        protected override void Release(Cube cube)
        {
            CubeIsDead?.Invoke(cube.transform.position);
            base.Release(cube);
        }

        protected override void GetSpawnedObject(Vector3 position)
        {
            Cube cube = Pool.Get();
            cube.transform.position = SelectSpawnPoint();

            base.GetSpawnedObject(position);
        }

        private void SpawnedCube()
        {
            GetSpawnedObject(SelectSpawnPoint());
        }

        private Vector3 SelectSpawnPoint()
        {
            Vector3 spawnPoint = new Vector3(
                 _spawnArea.transform.position.x + _spawnArea.center.x + UnityEngine.Random.Range(-1 * _spawnArea.bounds.extents.x, _spawnArea.bounds.extents.x),
                 _spawnArea.transform.position.y + _spawnArea.center.y + UnityEngine.Random.Range(-1 * _spawnArea.bounds.extents.y, _spawnArea.bounds.extents.y),
                 _spawnArea.transform.position.z + _spawnArea.center.z + UnityEngine.Random.Range(-1 * _spawnArea.bounds.extents.z, _spawnArea.bounds.extents.z));

            return spawnPoint;
        }
    }
}
