using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform m_muzzle;
    [SerializeField]
    private float m_rate = 0.3f;
    [SerializeField]
    private bool m_canShot = true;

    private void OnEnable()
    {
        StartCoroutine(Reload());
    }

    public void OnEquip()
    {
        //Load Pool and Stuff
        gameObject.SetActive(true);
    }

    public void OnUnEquip()
    {
        //Release Event and stuff
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called on FPSController
    /// </summary>
    public void Fire()
    {
        if (!m_canShot)
            return;

        m_canShot = false;
        GameObject VFXMuzzle = PoolObject.GetObject(Globals.PoolKey.VFXMuzzle);
        if(VFXMuzzle != null)
            VFXMuzzle.transform.position = m_muzzle.transform.position;
        GameObject VFXEjection = PoolObject.GetObject(Globals.PoolKey.VFXEjection, false);

        GameObject currentBullet = PoolObject.GetObject(Globals.PoolKey.BulletGameObject)/*Instantiate(m_bulletPrefab, m_muzzle.position, m_muzzle.rotation)*/;
        if(currentBullet == null)
        {
            Debug.LogError("Bullet Doesent Found");
            return;
        }
        currentBullet.transform.position = m_muzzle.transform.position;
        currentBullet.transform.rotation = m_muzzle.transform.rotation;

        VFXEjection.transform.position = transform.position;
        VFXEjection.SetActive(true);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_rate);
        m_canShot = true;
    }
}
