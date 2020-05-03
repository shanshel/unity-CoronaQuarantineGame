using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static EnumsData;

public abstract class CPlayer : MonoBehaviour, IPunObservable
{
    //cache components
    public Rigidbody2D _rb;
    public Animator _animator;
    public PhotonView _photonView;
    //Editor Options
    public float speed;
    [HideInInspector]
    public float baseSpeed;
    public string moveAnimationKey = "moving";
    public GameObject aimObjectIcon, aimContainerObject;

    public PlayerMiniMap _playerMiniMap;
    //StepOptions
    [SerializeField]
    protected GameObject dirtPrefab;
    protected float stepDirtTimer, stepDirtCooldownTime = 1f;

    //
    [HideInInspector]
    public Quaternion lookQuaternion;
    //Network 
    [HideInInspector]
    public Player _thisPlayer;
    public Team _thisPlayerTeam;

    protected Vector2 moveAmount;

    //Health
    public int maxHealth = 10;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public EnumsData.playerStatus cStatus = EnumsData.playerStatus.alive;
    //public bool isDead = false;

    public float respawnTime = 2f;
    [HideInInspector]
    public float respawnTimer;


    public ParticleSystem speedTrail;
    //Wepaon 
    public Weapon _currentWeaponObject, _defaultWeaponPrefab;

    public bool isInDevMode = false;
    protected virtual void Start()
    {
        if (isInDevMode)
        {
            NetworkPlayers._inst._localCPlayer = this;
            NetworkPlayers._inst.playerList.Add("DevPlayer", this);

        }
        currentHealth = maxHealth;
        wearDefaultWeapon();
        UIInGameCanvas._inst.healthIncrease(currentHealth, maxHealth);
        StartCoroutine(setUpPlayerMiniMap());
        baseSpeed = speed;
    }

    IEnumerator setUpPlayerMiniMap()
    {
        while(_thisPlayer == null)
        {
            yield return null;
        }

        if (_thisPlayer.IsLocal)
        {
            _playerMiniMap.setColor(Color.yellow);
            _playerMiniMap.Activate();
        }
        else
        {
            if (NetworkPlayers._inst._localCPlayer._thisPlayerTeam == _thisPlayerTeam)
            {
                _playerMiniMap.setColor(Color.green);
                _playerMiniMap.Activate();
            }
            else
            {
                _playerMiniMap.setColor(Color.red);
            }
        }

        onNetworkPlayerDefine();

    }

    public virtual void onNetworkPlayerDefine()
    {

    }

  
    private void Update()
    {
        if (!_photonView.IsMine && !isInDevMode) return;


        if (cStatus == playerStatus.dead)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                respawn();
            }
            return;
        }
        if (cStatus != playerStatus.alive) return;
        moveAmount = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized *  speed;
        stepDirtTimer -= Time.deltaTime;
        Aiming();
        idleMoveAnimationUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            takeDamage(1);;
            Debug.Log("current Health is: " + currentHealth);
        }
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine && !isInDevMode) return;
        if (cStatus != playerStatus.alive) return;
        Move();
    }

    private void idleMoveAnimationUpdate()
    {
        if (moveAmount != Vector2.zero)
        {
            _animator.SetBool(moveAnimationKey, true);
            if (stepDirtTimer <= 0f)
            {
                stepDirtTimer = stepDirtCooldownTime;
                spawnStepDirt();
            }
        }
        else
        {
            _animator.SetBool(moveAnimationKey, false);
        }
    }

    protected virtual void spawnStepDirt()
    {
        Instantiate(dirtPrefab, transform.position, Quaternion.identity);
    }

    private void Attack()
    {
        _currentWeaponObject.Shot();
    }

    private void Move()
    {
        if (_photonView.IsMine || isInDevMode)
            _rb.MovePosition(_rb.position + moveAmount * Time.fixedDeltaTime);
    }

    private void Surprised()
    {

    }

    void Aiming()
    {
        var mousePos = InGameManager._inst.worldMousePosition;
        aimObjectIcon.transform.position = new Vector2(mousePos.x, mousePos.y);

        Vector2 dir = mousePos - aimContainerObject.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        lookQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        aimContainerObject.transform.rotation = rotation;
    }

    /* Abstract */
    public void SetPlayerNetworkInfo(Player _player)
    {
        _thisPlayer = _player;
    }

    /* Photon */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    /* Health */
    public void takeDamage(int amount)
    {
        if (cStatus != playerStatus.alive) return;

        var newHealth = currentHealth - amount;
        UIInGameCanvas._inst.healthDecrease(currentHealth, maxHealth);
        if (newHealth <= 0)
        {
            currentHealth = 0;
            onDeath();
        }
        else
        {
            currentHealth = newHealth;
        }

    }

    public void takeHealth(int amount)
    {
        if (cStatus != playerStatus.alive) return;
        UIInGameCanvas._inst.healthIncrease(currentHealth, maxHealth);
        var newHealth = currentHealth + amount;
        if (newHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = newHealth;
        }
    }

    public void onDeath()
    {
        cStatus = playerStatus.dead;
        respawnTimer = respawnTime;
        UIOverlay.show(EnumsData.UIOverlay.dead);
        InGameManager._inst.setDeadFollowNexT();

    }

    void respawn()
    {
        cStatus = playerStatus.respawining;
        Transform point = NetworkPlayers._inst.getSpawnPoint(_thisPlayerTeam);
        currentHealth = maxHealth;
        Debug.Log("Will go to " + point.position);
        _rb.simulated = false;
        Vector2 pos = new Vector3(point.position.x, point.position.y, transform.position.z);
        transform.DOMove(pos, 1f).OnComplete(() => { onRespawnFinished(); }).Play();
    }
    void onRespawnFinished()
    {
        _rb.simulated = true;
        cStatus = playerStatus.alive;
        InGameManager._inst.setCameraFollow(transform);
        UIOverlay.hide(EnumsData.UIOverlay.dead);

    }

    /* Weapons */

    public void wearWeapon(Weapon weaponPrefab)
    {
        if (_currentWeaponObject != null) 
           Destroy(_currentWeaponObject);
        _currentWeaponObject = Instantiate(weaponPrefab, aimContainerObject.transform);
    }

    public void wearDefaultWeapon()
    {
        if (_currentWeaponObject != null)
            Destroy(_currentWeaponObject);
        _currentWeaponObject = Instantiate(_defaultWeaponPrefab, aimContainerObject.transform);
    }

   
    /* Speed Trail */

    public void speedTrailOn()
    {
        speedTrail.Play();
    }

    public void speedTrailOff()
    {
        speedTrail.Stop();

    }

}
