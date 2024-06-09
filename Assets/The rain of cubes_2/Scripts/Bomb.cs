using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(Renderer))]
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _minLifeTimeSeconds = 2.0f;
        [SerializeField] private float _maxLifeTimeSeconds = 5.0f;
        [SerializeField] private float _explosionForce = 500f;
        [SerializeField] private float explosionRadius = 4f;

        private Renderer _renderer;
        private float _lifeTimeSeconds;

        public event Action<Bomb> Exploded;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material = InstallerRenderMode.ToFadeMode(_renderer.material);
        }

        private void OnEnable()
        {
            ResetParametrs();
            StartCoroutine(Disappear());
        }

        private void ResetParametrs()
        {
            _lifeTimeSeconds = UnityEngine.Random.Range(_minLifeTimeSeconds, _maxLifeTimeSeconds);

            Color color = _renderer.material.color;
            color.a = 1.0f;
            _renderer.material.color = color;
        }

        private IEnumerator Disappear()
        {
            Color color = _renderer.material.color;
            float elapsed = 0.0f;

            while (elapsed <= _lifeTimeSeconds)
            {
                elapsed += Time.deltaTime;

                color.a = Mathf.MoveTowards(color.a, 0, Time.deltaTime / _lifeTimeSeconds);
                _renderer.material.color = color;

                yield return null;
            }

            Explode();
        }

        private void Explode()
        {
            foreach (Rigidbody expodableCube in GetExpodable())
            {
                expodableCube.AddExplosionForce(_explosionForce, transform.position, explosionRadius);
            }

            Exploded?.Invoke(this);
        }

        private List<Rigidbody> GetExpodable()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

            List<Rigidbody> cubes = new List<Rigidbody>();

            foreach (Collider hit in hits)
            {
                if (hit.attachedRigidbody != null)
                    cubes.Add(hit.attachedRigidbody);
            }

            return cubes;
        }
    }
}
