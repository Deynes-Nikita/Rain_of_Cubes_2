using UnityEngine;
using TMPro;
using Spawner;

namespace Scoreboard
{
   internal enum NameObjects
    {
        Cube,
        Bomb
    }

    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private NameObjects _name;
        [SerializeField] protected TextMeshProUGUI CountCreatedObjectsText;
        [SerializeField] protected TextMeshProUGUI CountActiveObjectsText;
        [SerializeField] private SpawnerBox _spawner;

        private IShowableCountObjects _countObjects = null;

        private void Awake()
        {
            if (_name == NameObjects.Cube)
                _countObjects = (IShowableCountObjects)_spawner.Cube;
            else
                _countObjects = (IShowableCountObjects)_spawner.Bomb;
        }

        private void OnEnable()
        {
            _countObjects.ChangedCountsOfObjects += Show;
        }

        private void OnDisable()
        {
            _countObjects.ChangedCountsOfObjects -= Show;
        }

        protected void Show(int countCreatedObjects, int countActiveObjects)
        {
            CountCreatedObjectsText.text = countCreatedObjects.ToString();
            CountActiveObjectsText.text = countActiveObjects.ToString();
        }
    }
}