using UnityEngine;
using Item;

namespace Spawner
{
    [RequireComponent(typeof(CubeSpawner))]
    public class BombSpawner : Spawner<Bomb>
    {
        private CubeSpawner _cubeSpawner;

        private void OnEnable()
        {
            _cubeSpawner.CubeIsDead += GetSpawnedObject;
        }

        private void OnDisable()
        {
            _cubeSpawner.CubeIsDead -= GetSpawnedObject;
        }

        protected override void SetParameters()
        {
            _cubeSpawner = GetComponent<CubeSpawner>();
        }


        protected override Bomb CreateFunc()
        {
            Bomb bomb = Instantiate(SpawnedObject);
            bomb.Exploded += Release;

            base.CreateFunc();

            return bomb;
        }

        protected override void ActionOnDestroy(Bomb bomb)
        {
            bomb.Exploded -= Release;

            Destroy(bomb);
        }

        protected override void GetSpawnedObject(Vector3 position)
        {
            Bomb bomb = Pool.Get();
            bomb.transform.position = position;

            base.GetSpawnedObject(position);
        }
    }
}
