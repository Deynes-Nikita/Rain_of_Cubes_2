using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Spawner
{
    public abstract class Spawner<T> : MonoBehaviour, IShowableCountObjects where T : MonoBehaviour
    {
        [SerializeField] protected T SpawnedObject;

        protected int CountCreatedObjects = 0;
        protected ObjectPool<T> Pool;
        protected Vector3 SpawnPosition;

        public event Action<int, int> ChangedCountsOfObjects;

        private void Awake()
        {
            CreatePool();

            SetParameters();
        }

        protected abstract void SetParameters();

        protected virtual T CreateFunc()
        {
            CountCreatedObjects++;
            CalculateObjects();

            return null;
        }

        protected virtual void ActionOnRelease(T spawnedObject)
        {
            CalculateObjects();

            spawnedObject.gameObject.SetActive(false);
        }

        protected virtual void ActionOnDestroy(T spawnedObject)
        {
            CalculateObjects();

            Destroy(spawnedObject.gameObject);
        }

        protected virtual void Release(T spawnedObject)
        {
            CalculateObjects();

            Pool.Release(spawnedObject);
        }

        protected virtual void GetSpawnedObject(Vector3 position)
        {
            CalculateObjects();
        }

        private void CreatePool()
        {
            Pool = new ObjectPool<T>
                   (
                    createFunc: () => CreateFunc(),
                    actionOnGet: (obj) => ActionOnGet(obj),
                    actionOnRelease: (obj) => ActionOnRelease(obj),
                    actionOnDestroy: (obj) => ActionOnDestroy(obj),
                    collectionCheck: false
                   );
        }

        private void ActionOnGet(T spawnedObject)
        {
            Rigidbody rigidbodySpawnedObject = spawnedObject.GetComponent<Rigidbody>();
            rigidbodySpawnedObject.velocity = Vector3.zero;
            rigidbodySpawnedObject.angularVelocity = Vector3.zero;
            spawnedObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            spawnedObject.gameObject.SetActive(true);
        }

        private void CalculateObjects()
        {
            ChangedCountsOfObjects?.Invoke(CountCreatedObjects, Pool.CountActive);
        }
    }
}
