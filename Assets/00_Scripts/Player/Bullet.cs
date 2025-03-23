using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_speed;
    Transform _target;
    Vector3 _targetPos;
    double _dmg;
    string _characterName;
    bool _getHit = false;

    public ParticleSystem AttackParticle;

    Dictionary<string, GameObject> _projecTiles = new Dictionary<string, GameObject>();
    Dictionary<string, ParticleSystem> _muzzles = new Dictionary<string, ParticleSystem>();

    private void Awake()
    {
        Transform projectiles = transform.GetChild(0);
        Transform muzzles = transform.GetChild(1);

        for(int i = 0; i < projectiles.childCount; i++)
        {
            _projecTiles.Add(projectiles.GetChild(i).name, projectiles.GetChild(i).gameObject);
        }
        for(int i = 0; i <muzzles.childCount; i++)
        {
            _muzzles.Add(muzzles.GetChild(i).name,muzzles.GetChild(i).GetComponent<ParticleSystem>());
        }
    }
    public void AttackInit(Transform target, double dmg)
    {
        _target = target;
        if(_target != null)
        {
            _target.GetComponent<Character>().GetDamage(dmg);
        }
        _getHit = true;
        AttackParticle.Play();
        StartCoroutine(ReturnObject(AttackParticle.duration));
    }
    public void Init(Transform target, double dmg, string characterName)
    {
        
        if(target == null ) {
            Debug.Log(target);
        }
        _target = target;
        transform.LookAt(_target);
        _getHit = false;
        _targetPos = _target.position;

        this._dmg = dmg;
        _characterName = characterName;
        _projecTiles[_characterName].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_getHit) return;
        _targetPos.y = 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * m_speed);

        if(Vector3.Distance(transform.position, _targetPos) <= .1f)
        {
            if(_target != null)
            {
                _getHit = true;
                _target.GetComponent<Character>().GetDamage(_dmg);
                _projecTiles[_characterName].gameObject.SetActive(false);
                _muzzles[_characterName].Play();

                StartCoroutine(ReturnObject(_muzzles[_characterName].duration));
            }
        }
    }

    IEnumerator ReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        Base_Mng.Pool.m_pool_Dictionary["AttackHelper"].Return(this.gameObject);
    }
}
