using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static EnumsData;
using System.IO;

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
    public GameObject aimObjectIcon, aimContainerObject, lightContainer, lightShadowCaster;

    public SpriteRenderer[] _bodyPartEffectedToOutline, allBodyPartSprites;
    public SpriteRenderer _headSprite;
    public Material enemyOutlineMat, allieOutlineMat;
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


    //Inventory 
    public Inventory playerInventory;
    public bool isDevMe = false;
    bool isPlayerReady;

    //Head Healthbar 
    public HeadHealthBar healthBar;
    protected void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            InGameManager._inst.isDev = true;
        }
        if (!InGameManager._inst.isDev)
        {
            NetworkPlayers._inst.playerList.Add(_photonView.Owner.NickName, this);
        } else
        {
            NetworkPlayers._inst.playerList.Add(Random.Range(0, 100000).ToString(), this);
            NetworkPlayers._inst._localCPlayer = this;
        }
        aimObjectIcon.SetActive(false);
        currentHealth = maxHealth;
        baseSpeed = speed;
        AfterStartDone();
    }

    void AfterStartDone()
    {
        if (isDevMe || _photonView.IsMine)
        {
            aimObjectIcon.SetActive(true);
            UIInGameCanvas._inst.healthUpdate(currentHealth, maxHealth);
            InGameManager._inst.setCameraFollow(transform);
            playerInventory = Inventory._inst;
            InGameManager._inst.setInventoryReady();
        }
    }
     

    public void changeBasedOnOtherPlayersInfo()
    {
        if (isDevMe || _photonView.IsMine)
        {
            // This Is My Player
            _playerMiniMap.setColor(Color.yellow);
            _playerMiniMap.Activate();
            lightContainer.SetActive(true);
            lightShadowCaster.SetActive(true);
            
            wearDefaultWeapon();
            foreach (var bSprite in _bodyPartEffectedToOutline)
            {
                bSprite.material = allieOutlineMat;
            }
        }
        else
        {

            if (NetworkPlayers._inst._localCPlayer._thisPlayerTeam == _thisPlayerTeam)
            {
                //This is Allie Player For Me
                _playerMiniMap.setColor(Color.green);
                _playerMiniMap.Activate();
                lightContainer.SetActive(true);
                foreach (var bSprite in _bodyPartEffectedToOutline)
                {
                    bSprite.material = allieOutlineMat;
                }
            }
            else
            {
                //This is enemy Player For Me
                _playerMiniMap.setColor(Color.red);
                lightContainer.SetActive(false);
                foreach (var bSprite in _bodyPartEffectedToOutline)
                {
                    bSprite.material = enemyOutlineMat;
                }
            }
        }

        whenPlayerSetupFinished();
        isPlayerReady = true;
    }

 

    

    public virtual void whenPlayerSetupFinished()
    {

    }

  
    private void Update()
    {
        if (!isPlayerReady) return;
     
        if (!_photonView.IsMine && !isDevMe) return;


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


      
        
        overableUpdate();
    }

    
    
    public virtual void overableUpdate()
    {

    }

    float timeBetweenSteps = .1f;
    float stepTimer;
    Vector3 lastPosition;
    int stepCircile;
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            timeBetweenSteps += .05f;
        }
        if (!isPlayerReady) return;
        stepTimer -= Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, lastPosition) >= .3f && stepTimer <= 0f)
        {
            lastPosition = transform.position;
            stepTimer = timeBetweenSteps;
            if (stepCircile == 0)
            {
                SoundManager._inst.playSoundOnceAt(SoundEnum.Step1A, transform.position);
            }
            else if (stepCircile == 1)
            {
                SoundManager._inst.playSoundOnceAt(SoundEnum.Step2A, transform.position);

            }
            else  if (stepCircile == 2)
            {
                SoundManager._inst.playSoundOnceAt(SoundEnum.Step1B, transform.position);
            }
            else if (stepCircile == 3)
            {
                SoundManager._inst.playSoundOnceAt(SoundEnum.Step2B, transform.position);
            }

            if (stepCircile == 3)
            {
                stepCircile = 0;
            }
            else
            {
                stepCircile++;
            }
          

        }
        if (!_photonView.IsMine && !isDevMe) return;
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
        overwriteableAttack();
    }
    protected virtual void overwriteableAttack()
    {

    }

    private void Move()
    {
        if (_photonView.IsMine || isDevMe)
        {
            _rb.MovePosition(_rb.position + moveAmount * Time.fixedDeltaTime);

        }
    }

    private void Surprised()
    {

    }

    void Aiming()
    {
        var mousePos = ScreenManager._inst.worldMousePosition;
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

        if (stream.IsWriting)
        {
            stream.SendNext(this.currentHealth);

        }
        else
        {
            this.currentHealth = (int)stream.ReceiveNext();
            healthBar.setHealth(this.currentHealth, maxHealth);
        }

  
    }

    /* Health */
    public void takeDamage(int amount)
    {

        if (cStatus != playerStatus.alive) return;
        if (_thisPlayerTeam == Team.Doctors)
        {
            SoundManager._inst.playSoundOnceAt(SoundEnum.DoctorTakeDamage, transform.position);
        }
        else
        {
            SoundManager._inst.playSoundOnceAt(SoundEnum.PatientTakeDamage, transform.position);
        }
        healthBar.Shake();
        if (!_photonView.IsMine) return;
       

        var newHealth = currentHealth - amount;
        
        if (newHealth <= 0)
        {
            currentHealth = 0;
            onDeath();
        }
        else
        {
            currentHealth = newHealth;
        }

        UIInGameCanvas._inst.healthDecrease(currentHealth, maxHealth);
        healthBar.setHealth(currentHealth, maxHealth);
    }

 

    public void takeHealth(int amount)
    {
        if (cStatus != playerStatus.alive) return;
        if (!_photonView.IsMine) return;
        var newHealth = currentHealth + amount;
        if (newHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = newHealth;
        }
        UIInGameCanvas._inst.healthIncrease(currentHealth, maxHealth);
        healthBar.setHealth(currentHealth, maxHealth);
    }

    public void onDeath()
    {
        if (_thisPlayerTeam == Team.Doctors)
        {
            SoundManager._inst.playSoundOnceAt(SoundEnum.DoctorDie, transform.position);
        }
        else
        {
            SoundManager._inst.playSoundOnceAt(SoundEnum.PatientDie, transform.position);
        }

        if (!_photonView.IsMine) return;
        SoundManager._inst.playSoundOnce(SoundEnum.WhileWaitForRespawn);
        cStatus = playerStatus.dead;
        respawnTimer = respawnTime;
        UIOverlay.show(EnumsData.UIOverlay.dead);
        InGameManager._inst.setDeadFollowNexT();
    }

    public void onKillSomeone()
    {
        if (_thisPlayerTeam == Team.Patients)
        {
            SoundManager._inst.playSoundOnceAt(SoundEnum.PatientKillDoctor, transform.position);
        }
      
    }
    void respawn()
    {
        if (!_photonView.IsMine) return;
        SoundManager._inst.stopSound(SoundEnum.WhileWaitForRespawn);
        cStatus = playerStatus.respawining;
        Transform point = NetworkPlayers._inst.getSpawnPoint(_thisPlayerTeam);
        currentHealth = maxHealth;
        _rb.simulated = false;
        Vector2 pos = new Vector3(point.position.x, point.position.y, transform.position.z);
        transform.DOMove(pos, 1f).OnComplete(() => { onRespawnFinished(); }).Play();
    }
    void onRespawnFinished()
    {
        if (!_photonView.IsMine) return;
        _rb.simulated = true;
        cStatus = playerStatus.alive;
        InGameManager._inst.setCameraFollow(transform);
        UIOverlay.hide(EnumsData.UIOverlay.dead);
        UIInGameCanvas._inst.healthUpdate(currentHealth, maxHealth);
    }

    /* Weapons */

    public void wearWeapon(Weapon weaponPrefab)
    {
        if (!_photonView.IsMine) return;
        if (_currentWeaponObject != null)
            PhotonNetwork.Destroy(_currentWeaponObject.gameObject);
        GameObject wp = PhotonNetwork.Instantiate(Path.Combine("Weapons", weaponPrefab.name), Vector3.zero, Quaternion.identity, 0);
        _currentWeaponObject = wp.GetComponent<Weapon>();
    }

    public void wearDefaultWeapon()
    {
        if (!_photonView.IsMine) return;
        if (_currentWeaponObject != null)
            PhotonNetwork.Destroy(_currentWeaponObject.gameObject);

        if (!InGameManager._inst.isDev)
        {
            GameObject wp = PhotonNetwork.Instantiate(Path.Combine("Weapons", _defaultWeaponPrefab.name), Vector3.zero, Quaternion.identity, 0);
        }
        else
        {
            Instantiate(_defaultWeaponPrefab, Vector3.zero, Quaternion.identity);
        }
     
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            SoundManager._inst.playSoundOnceAt(SoundEnum.PlayerCollidWithWall, transform.position);
        }
    }

}
