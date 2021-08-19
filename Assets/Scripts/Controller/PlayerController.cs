using System.Collections;
using UnityEngine;

namespace PokeyBallTest.Controller
{
    [RequireComponent(typeof(SpringJoint), typeof(Rigidbody))]
    [DisallowMultipleComponent]//Prevent adding multiple PlayerController wich can lead to weird behaviour
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_tempRigidbody = default;
        [SerializeField] private LineRenderer m_jointRenderer = default;
        [SerializeField] private float m_maxSpeed = 20.0f;
        [SerializeField] private float m_minHeightOffsetToLaunch = 0.2f;

        public SpringJoint m_joint; //The joint used to propel the ball
        public Rigidbody m_rigidbody;
        public bool m_isAttachedToWall = true;
        public bool m_isDragging = false;
        private Vector3 m_offset = Vector3.zero;
        private float m_zOffset = 2.5f;
        private LayerMask m_mask; //used to raycast only against specific layers
        private Vector3 m_wallHitPoint = Vector3.zero;
        public bool m_isAlive = true;
        private float m_startHeight = 0.0f;
        public GameObject holePrefab, HitFx, TrailFx;

        void Start()
        {
            m_joint = GetComponent<SpringJoint>();
            m_rigidbody = GetComponent<Rigidbody>();
            SetMask();
            
            TryAttachToWall();

            m_zOffset = Camera.main.WorldToScreenPoint(transform.position).z;
        }

        
        private void SetMask()
        {
            LayerMask wallMask = LayerMask.NameToLayer(LayersName.instance.Target);
            LayerMask obstacleMask = LayerMask.NameToLayer(LayersName.instance.Obstacle);
            LayerMask instantDeathObstacle = LayerMask.NameToLayer(LayersName.instance.InstantDeathObstacle);
            m_mask = wallMask | obstacleMask | instantDeathObstacle;
            m_mask = ~m_mask;
        }

       
        private void Update()
        {
            if (m_isAlive)
            {
                ProcessLeftMouseClick();
                RenderJoint();
            }
        }

        private void FixedUpdate()
        {
            
            if (m_rigidbody.velocity.magnitude > m_maxSpeed)
            {
                m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxSpeed;
            }
        }

        
        private void ProcessLeftMouseClick()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!m_isAttachedToWall)
                {
                    TryAttachToWall();
                }
                else
                {
                    m_startHeight = transform.position.y;
                    m_offset = transform.position - GetMousePosition();
                    m_isDragging = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && m_isDragging)
            {
                DetachPlayerFromWall();
                m_isDragging = false;
            }

            if (m_isDragging)
                MouseDrag();
        }


        private void TryAttachToWall()
        {
            RaycastHit hit = RaycastForward();

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayersName.instance.Obstacle)) //If layer is "Obstacle", player can't attach to it
            {

                GameObject hitFx = Instantiate(HitFx, hit.point, Quaternion.identity);

                AttachBallToSubstituteBody();
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayersName.instance.InstantDeathObstacle))
            {
                PlayerIsDiying();
                AttachBallToSubstituteBody();
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayersName.instance.Target))
            {

                Vector3 holepos = hit.point;
                holepos.z -= 0.1f;
                GameObject go = (GameObject)(Instantiate(holePrefab, holepos, Quaternion.identity));
                m_isAttachedToWall = true;
                m_wallHitPoint = hit.point;
                Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                if (rb) //If target is valid, check if it has a rigidbody to attach to
                {
                    m_joint.connectedAnchor = m_wallHitPoint;
                    m_joint.connectedBody = rb;
                    SetJointRendererState(true);
                }
                else
                {
                    AttachBallToSubstituteBody();
                }
            }
            else
            {
                AttachBallToSubstituteBody();
            }
        }

        private void AttachBallToSubstituteBody()
        {
            m_joint.connectedBody = m_tempRigidbody;
        }

        
        private RaycastHit RaycastForward()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity, m_mask))
            {
                return hit;
            }

            return new RaycastHit();
        }

       
        private void MouseDrag()
        {
            SetPlayerPositionWhileDragging();
        }

        
        private void SetPlayerPositionWhileDragging()
        {
            Vector3 mousePosition = GetMousePosition();
            if ((mousePosition + m_offset).y < transform.position.y) //Security check to lock drag to the top
            {
                transform.position = mousePosition + m_offset;
            }
        }

        
        private Vector3 GetMousePosition()
        {
            Vector3 mousePos = Input.mousePosition;

            mousePos.z = m_zOffset;
            mousePos.x = 0;

            return Camera.main.ScreenToViewportPoint(mousePos);
        }

        
        private void DetachPlayerFromWall()
        {
            if (m_isAttachedToWall && IsHeightOffsetEnoughToLaunch())
            {
                m_joint.connectedBody = m_tempRigidbody;
                m_isAttachedToWall = false;
            }
        }

        
        private bool IsHeightOffsetEnoughToLaunch()
        {
            return ((m_startHeight - transform.position.y) > m_minHeightOffsetToLaunch);
        }

       
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(TagsName.instance.DeathZone))
            {
                PlayerIsDiying();
            }
        }

       
        private void RenderJoint()
        {
            if (m_isAttachedToWall)
            {
                TrailFx.SetActive(false);
                m_jointRenderer.SetPosition(0, transform.position); 
                m_jointRenderer.SetPosition(1, m_wallHitPoint); 
            }
            else
            {
               
                TrailFx.SetActive(true);
                SetJointRendererState(false);
            }
        }

       
        private void PlayerIsDiying()
        {
            
            m_isAlive = false;
            SetJointRendererState(false);
        }

        
        private void SetJointRendererState(bool enabled)
        {
            if (m_jointRenderer)
                m_jointRenderer.enabled = enabled;
        }

       
    }

    
}
