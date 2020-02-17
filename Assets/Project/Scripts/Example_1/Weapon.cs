using System.Collections;
using UnityEngine;
using CustomFMOD;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform m_muzzle;
    [SerializeField]
    private Transform m_ejection;
    [SerializeField]
    private float m_rate = 0.3f;
    [SerializeField]
    private bool m_canShot = true;
    [SerializeField]
    private Animator m_aniamtor;

    #region Sfx
    [SerializeField]
    private FMODEventInstance m_weaponFireSound;
    #endregion

    private void Awake()
    {
        m_aniamtor = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(Reload());
        FMODDatabase.Instance.GetFmodEvent(m_weaponFireSound);
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

        GameObject currentBullet = PoolObject.GetObject(Globals.PoolKey.BulletGameObject)/*Instantiate(m_bulletPrefab, m_muzzle.position, m_muzzle.rotation)*/;
        if (currentBullet == null)
        {
            Debug.LogError("Bullet Doesent Found");
            return;
        }
        currentBullet.transform.position = m_muzzle.transform.position;
        currentBullet.transform.rotation = m_muzzle.transform.rotation;

        m_aniamtor.SetTrigger("Shot");
        
        //VFX
        GameObject VFXMuzzle = PoolObject.GetObject(Globals.PoolKey.VFXMuzzle);
        if(VFXMuzzle != null)
            VFXMuzzle.transform.position = m_muzzle.transform.position;
        GameObject VFXEjection = PoolObject.GetObject(Globals.PoolKey.VFXEjection, m_ejection.position, m_ejection.eulerAngles, false);
        if (VFXEjection != null)
            VFXEjection.SetActive(true);

        StartCoroutine(Reload());
    }
    public void PlayFireSound(int i)
    {
        //Play Fire Sound
        if (i == 1)
            m_weaponFireSound.Play();
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_rate);
        m_canShot = true;
    }
}
