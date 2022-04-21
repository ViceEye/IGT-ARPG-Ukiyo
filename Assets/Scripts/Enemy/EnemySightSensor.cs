using UnityEngine;

namespace Enemy
{
    public class EnemySightSensor : MonoBehaviour
    {
        public GameObject _player;
        public float _distance;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }

        public bool Ping()
        {
            if (_player != null && _distance > 0.0f)
            {
               var distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
               if (distance <= _distance)
               {
                   return true;
               }
            }
            return false;
        }
    }
}