using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public int enemyHealth = 5;

    public SkinnedMeshRenderer _rend;

    public Material _baseMAT;

    public Material _hitFlashMAT;



    private void Start()
    {
        _baseMAT = _rend.material;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerHitSphere")
        {
            StartCoroutine(TakeDamage(1));
        }
        if (other.gameObject.tag == "RegularBullet")
        {
            StartCoroutine(TakeDamage(2));
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "ChargeBullet")
        {
            StartCoroutine(TakeDamage(5));
        }
    }

    IEnumerator TakeDamage(int damage)
    {
        enemyHealth -= damage;
        RumbleManager.Instance.RumblePulse(0.3f, 0.6f, 0.25f);
        if (enemyHealth > 0)
        {
            _rend.material = _hitFlashMAT;
            HitStopManager.Instance.DoHitStop(0.1f);
            yield return new WaitForSeconds(0.2f);
            _rend.material = _baseMAT;
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
