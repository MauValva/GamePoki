using UnityEngine;

namespace KrolStudio
{
    public class EnemyEffects : MonoBehaviour, IEnemyEffects
    {
        [SerializeField] Transform splatRed;

        public void PlaySplatRed()
        {
            var obj = Object.Instantiate(splatRed); // TODO: make a pool
            obj.transform.position = transform.position + new Vector3(0f, 0.8f, 0f);
        }
    }
}