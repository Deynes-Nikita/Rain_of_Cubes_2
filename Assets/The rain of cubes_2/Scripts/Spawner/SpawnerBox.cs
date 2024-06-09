using UnityEngine;

namespace Spawner
{
    [RequireComponent(typeof(CubeSpawner))]
    [RequireComponent(typeof(BombSpawner))]
    public class SpawnerBox : MonoBehaviour
    {
        private CubeSpawner _cubeSpawner = null;
        private BombSpawner _bombSpawner = null;

        public CubeSpawner Cube => _cubeSpawner;
        public BombSpawner Bomb => _bombSpawner;

        private void Awake()
        {
            _cubeSpawner = GetComponent<CubeSpawner>();
            _bombSpawner = GetComponent<BombSpawner>();
        }
    }
}
