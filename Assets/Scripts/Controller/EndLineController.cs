using UnityEngine;

namespace PokeyBallTest.Controller
{
    public class EndLineController : MonoBehaviour
    {
        [SerializeField] private GameObject m_nextLevelPortal = default;

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag(TagsName.instance.Player))
            {
                Invoke("OpenPortal", 0.2f); 
            }
            
        }

        private void OpenPortal()
        {
            m_nextLevelPortal?.SetActive(true);
        }
    }
}

