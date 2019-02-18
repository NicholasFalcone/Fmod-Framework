using UnityEngine;
using UnityEngine.Networking;
using FmodEditor;

namespace NetworkingExample
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField]
        private GenericEvent m_fireEvent;
        [SerializeField]
        private GenericEvent m_footStep;

        public GameObject bulletPrefab;
        public Transform bulletSpawn;


        void Update()
        {
            if (!isLocalPlayer)
                return;

            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            if(z != 0)
            {
                Move(z);
            }

            transform.Rotate(0, x, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdFire();
            }
        }

        void Move(float _directiion)
        {
            transform.Translate(0, 0, _directiion);
            RpcPlayFootStep();
        }

        // This [Command] code is called on the Client …
        // … but it is run on the Server!
        [Command]
        void CmdFire()
        {
            ///Playe Shoot Sound
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);

            RpcFireSound(transform.position);
            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

            // Spawn the bullet on the Clients
            NetworkServer.Spawn(bullet);

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }

        [ClientRpc]
        private void RpcFireSound(Vector3 _position)
        {
            FmodManager.instance.PlaySoundOneShot(m_fireEvent.eventPath, _position);
        }

        [ClientRpc]
        private void RpcPlayFootStep()
        {
        
            if(FmodManager.instance.IsPlaying(m_footStep.fmodEvent))
            {
                FmodManager.instance.AttachSfx(m_footStep.fmodEvent, transform);
                FmodManager.instance.StartEvent(m_footStep);
            }
        }

        public override void OnStartLocalPlayer()
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            FmodManager.instance.CreateGenericEnventInstance(ref m_footStep);
        }
    }
}