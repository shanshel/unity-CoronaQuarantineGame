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
        weaponRenderer();
     
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
        if (Input.GetKeyDown(KeyCode.Tab) && _photonView.IsMine)
        {
            takeDamage(1);
        }

      
        
        overableUpdate();
    }

    void weaponRenderer()
    {
        /*
        if (allBodyPartSprites[0].enabled)
        {
            if (_currentWeaponObject && _currentWeaponObject.VisiablePartContainer != null)
            {
                _currentWeaponObject.VisiablePartContainer.SetActive(true);
            } else
            {
                _currentWeaponObject.VisiablePartContainer.SetActive(false);
            }
        } else
        {
            _currentWeaponObject.VisiablePartContainer.SetActive(false);
        }
        */
    }
    /*
    public virtual void renderToLocalPlayer()
    {
     
        if (NetworkPlayers._inst._localCPlayer == null || _currentWeaponObject == null) return;
      
  
  

        //if it's on my team render me 
        if (_thisPlayerTeam == NetworkPlayers._inst._localCPlayer._thisPlayerTeam)
        {
            return;
        }

        if (NetworkPlayers._inst._localCPlayer._thisPlayerTeam == Team.Doctors)
        {
            if (_headSprite.isVisible)
            {
                RaycastHit2D hit = Physics2D.Raycast(_headSprite.transform.position, NetworkPlayers._inst._localCPlayer.transform.position - _headSprite.transform.position, 50f, LayerMask.GetMask("DoctorPlayerLayer")  | LayerMask.GetMask("Wall"));

                if (hit.transform.tag == "Doctor")
                {
                    //When Doctor Hitted (Mean That's I'm a Doctor)
                    var doctorScript = hit.transform.GetComponent<CPlayer>();
                    var dir = (_headSprite.transform.position - NetworkPlayers._inst._localCPlayer.transform.position);
                    var lookAngle = doctorScript.lookQuaternion.eulerAngles.z;
                   
                    Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
                    float doctorToEnemyAngle = rotation.eulerAngles.z;


                    if (Mathf.DeltaAngle(lookAngle, doctorToEnemyAngle) < 45f && Mathf.DeltaAngle(lookAngle, doctorToEnemyAngle) > -45f)
                    {

                        _playerMiniMap.gameObject.SetActive(true);
                        for (var x = 0; x < allBodyPartSprites.Length; x++)
                        {
                            allBodyPartSprites[x].enabled = true;
                            //hit.transform.GetComponent<DoctorPlayerController>().onSeeEnemy();
                        }
                    }
                    else
                    {
                        _playerMiniMap.gameObject.SetActive(false);
                        for (var x = 0; x < allBodyPartSprites.Length; x++)
                        {
                            allBodyPartSprites[x].enabled = false;
                        }
                    }


                }
                else
                {
                    _playerMiniMap.gameObject.SetActive(false);
                    for (var x = 0; x < allBodyPartSprites.Length; x++)
                    {
                        allBodyPartSprites[x].enabled = false;
                    }
                }
            }
            else
            {
                for (var x = 0; x < allBodyPartSprites.Length; x++)
                {
                    allBodyPartSprites[x].enabled = false;
                }
            }
        }
  
        else if (NetworkPlayers._inst._localCPlayer._thisPlayerTeam == Team.Patients)
        {
            if (_headSprite.isVisible)
            {
                RaycastHit2D hit = Physics2D.Raycast(_headSprite.transform.position, NetworkPlayers._inst._localCPlayer.transform.position - _headSprite.transform.position, 50f, (LayerMask.GetMask("PatientPlayerLayer") | LayerMask.GetMask("Wall")));
                if (hit == null) return;
                if (hit.transform.tag == "Patient")
                {
                    _playerMiniMap.gameObject.SetActive(true);

                    _currentWeaponObject.VisiablePartContainer.SetActive(true);
                    for (var x = 0; x < allBodyPartSprites.Length; x++)
                    {
                        allBodyPartSprites[x].enabled = true;
                        //hit.transform.GetComponent<DoctorPlayerController>().onSeeEnemy();
                    }
                }
                else
                {
                    _playerMiniMap.gameObject.SetActive(false);
                    _currentWeaponObject.VisiablePartContainer.SetActive(false);
                    for (var x = 0; x < allBodyPartSprites.Length; x++)
                    {
                        allBodyPartSprites[x].enabled = false;
                    }
                }
            }
            else
            {
                _playerMiniMap.gameObject.SetActive(false);
                _currentWeaponObject.VisiablePartContainer.SetActive(false);
                for (var x = 0; x < allBodyPartSprites.Length; x++)
                {
                    allBodyPartSprites[x].enabled = false;
                }
            }
        }

       
  
    }
    */
    public virtual void overableUpdate()
    {

    }

    private void FixedUpdate()
    {
        if (!isPlayerReady) return;
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
        throw new System.NotImplementedException();
    }

    /* Health */
    public void takeDamage(int amount)
    {
        if (!_photonView.IsMine) return;
        if (cStatus != playerStatus.alive) return;

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
    }

    public void takeHealth(int amount)
    {
        if (!_photonView.IsMine) return;
        if (cStatus != playerStatus.alive) return;
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

    }

    public void onDeath()
    {
        if (!_photonView.IsMine) return;
        cStatus = playerStatus.dead;
        respawnTimer = respawnTime;
        UIOverlay.show(EnumsData.UIOverlay.dead);
        InGameManager._inst.setDeadFollowNexT();
    }

    void respawn()
    {
        if (!_photonView.IsMine) return;
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

}
