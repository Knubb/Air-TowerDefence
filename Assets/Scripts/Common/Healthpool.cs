﻿using System;
using UnityEngine;

namespace AirTowerDefence.Common
{
    public class Healthpool : MonoBehaviour, IDamagable
    {
        /// <summary>
        /// invoked once after initialization and for status changes.
        /// </summary>
        public event Action<(int lives, int health)> _HealthStatusChangedEvent;

        private int _MaxHealth;

        [SerializeField]
        private int _Lives;
        public int Lives
        {
            get { return _Lives; }
            private set
            { _Lives = value; }
        }

        [SerializeField]
        private int _Health;
        public int Health
        {
            get { return _Health; }
            private set
            {
                if (value > _MaxHealth) 
                { _Health = _MaxHealth; }
                else
                { _Health = value; }
            }
        }
        private void Update()
        {
            //Test
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                TakeDamage(1);
            }
        }

        private void Awake()
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _MaxHealth = 3;
            }
            else
            {
                _MaxHealth = _Health;
            }

            NotifyHealthStatusChanged();
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                if (_Lives > 0)
                {
                    ConvertLifeToHealth();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }

            NotifyHealthStatusChanged();
        }

        private void ConvertLifeToHealth()
        {
            Lives--;

            Health = _MaxHealth;
        }

        private void NotifyHealthStatusChanged()
        {
            _HealthStatusChangedEvent?.Invoke((_Lives, _Health));
        }
    }
}
