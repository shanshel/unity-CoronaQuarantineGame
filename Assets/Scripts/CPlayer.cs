using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsData;

public abstract class CPlayer : MonoBehaviour, IPunObservable
{
    //cache components
    public Rigidbody2D _rb;
    public Animator _animator;
    public PhotonView _photonView;
    //Editor Options
    public float speed;
    public string moveAnimationKey = "moving";
    public GameObject aimObject, lightObject;

    //StepOptions
    [SerializeField]
    protected GameObject dirtPrefab;
    protected float stepDirtTimer, stepDirtCooldownTime = 1f;

    //
    public Quaternion lightAngle;
    //Network 
    public Player _thisPlayer;
    public Team _thisPlayerTeam;

    protected Vector2 moveAmount;


    
  
    private void Update()
    {
        if (!_photonView.IsMine) return;
        moveAmount = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized *  speed;
        stepDirtTimer -= Time.deltaTime;
        Aiming();
        idleMoveAnimationUpdate();
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine) return;
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

    }

    private void Move()
    {
        if (_photonView.IsMine)
            _rb.MovePosition(_rb.position + moveAmount * Time.fixedDeltaTime);
    }

    private void Surprised()
    {

    }

    void Aiming()
    {
        var mousePos = InGameManager._inst.worldMousePosition;
        aimObject.transform.position = new Vector2(mousePos.x, mousePos.y);

        Vector2 dir = mousePos - lightObject.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        lightAngle = Quaternion.AngleAxis(angle, Vector3.forward);
        lightObject.transform.rotation = rotation;
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
}
