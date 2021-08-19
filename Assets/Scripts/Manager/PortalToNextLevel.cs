using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokeyBallTest
{
    
    public class PortalToNextLevel : MonoBehaviour
    {
        [SerializeField] private float m_portalRadius = 5.0f;
        [SerializeField] private float m_portalHeight = 0.1f;
        private float progress = 0.0f;

        private void OnEnable()
        {
            transform.localScale = new Vector3(transform.localScale.x, m_portalHeight, transform.localScale.z);
            StartCoroutine("OpenPortal");
        }

        private IEnumerator OpenPortal()
        {
            while (transform.localScale.x < m_portalRadius)
            {
                progress += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(m_portalRadius, m_portalHeight, m_portalRadius), progress);
                yield return new WaitForEndOfFrame();
            }
        }
        public int index;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsName.instance.Player))
            {
                
                SceneManager.LoadScene(index);
            }
        }
    }
}

