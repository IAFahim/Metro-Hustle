using UnityEngine;

namespace Manger1.Runtime
{
    public class TruckKill : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Destroy(other.gameObject);
        }
    }
}
