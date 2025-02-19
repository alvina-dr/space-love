using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using FMODUnity;
using UnityEngine.InputSystem.Users;

public class PlayerCursor : MonoBehaviour
{
    #region Properties
    Vector3 direction;
    [Header("COMPONENTS")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer circleSprite;
    [SerializeField] private SpriteRenderer internCircleSprite;
    [SerializeField] private SpriteRenderer crossSprite;
    public IA_PlayerCursor controlPlayer;

    [Header("CURRENT INFO")]
    public List<Enemy> targetList = new List<Enemy>();
    public int playerCurrentPoint = 0;

    [Header("ACTION BUTTON")]
    public EnemyData.Color cursorColor;

    #endregion

    #region Methods
    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        internCircleSprite.transform.DOScale(0.05f, .1f).OnComplete(() =>
        {
            switch(cursorColor)
            {
                case EnemyData.Color.Blue:
                    var audioEventRed = RuntimeManager.CreateInstance("event:/Character/TirBleu");
                    audioEventRed.start();
                    break;
                case EnemyData.Color.Red:
                    var audioEventBlue = RuntimeManager.CreateInstance("event:/Character/TirRouge");
                    audioEventBlue.start();
                    break;
            }
            internCircleSprite.transform.DOScale(0.1f, .1f);
        });
        if (targetList.Count > 0)
        {
            if (targetList[targetList.Count - 1].currentColor == cursorColor && !GPCtrl.Instance.loveFrenzy)
            {
                targetList.Remove(targetList[targetList.Count - 1]);
                Shoot(callbackContext);
            } else
            {
                playerCurrentPoint += targetList[targetList.Count - 1].data.scoreOnKill;
                targetList[targetList.Count - 1].Damage(1, this);
            }
        }
    }

    public void GainPoints(int _value)
    {
        playerCurrentPoint += _value;
        switch (cursorColor)
        {
            case EnemyData.Color.Red:
                GPCtrl.Instance.UICtrl.redScore.SetValue(playerCurrentPoint);
                break;
            case EnemyData.Color.Blue:
                GPCtrl.Instance.UICtrl.blueScore.SetValue(playerCurrentPoint);
                break;
        }
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        if (!DataHolder.Instance.GeneralData.computerMode)
        {
            direction = callbackContext.ReadValue<Vector2>().normalized;
        }
    }
    #endregion

    #region Unity API
    private void OnTriggerEnter(Collider collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null && (_enemy.currentColor != cursorColor || GPCtrl.Instance.loveFrenzy))
        {
            var audioEvent = RuntimeManager.CreateInstance("event:/Character/TargetReveal");
            audioEvent.start();
            _enemy.ChangeColor(EnemyData.Color.White);
            _enemy.SetOutline(true);
            if (targetList.Count == 0)
            {
                crossSprite.transform.DORotate(new Vector3(0, 0, 360), .3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                crossSprite.transform.DOScale(.15f, .3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic);
            }
            targetList.Add(_enemy);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null)
        {
            if (targetList.Contains(_enemy))
            {
                if (!GPCtrl.Instance.loveFrenzy) _enemy.ChangeColor(_enemy.currentColor);
                _enemy.SetOutline(false);
            }
            targetList.Remove(_enemy);
            StopCrossAnimation();
        }
    }

    public void StopCrossAnimation()
    {
        if (targetList.Count == 0)
        {
            crossSprite.transform.DOKill();
            crossSprite.transform.DOScale(.1f, .3f);
            crossSprite.transform.DORotate(Vector3.zero, .3f);
        }
    }

    void Update()
    {
        if (DataHolder.Instance.GeneralData.computerMode)
        {
            switch(cursorColor)
            {
                case EnemyData.Color.Blue:
                    direction = new Vector3(Input.GetAxisRaw("HorizontalRight"), Input.GetAxisRaw("VerticalRight"), 0).normalized;
                    break;
                case EnemyData.Color.Red:
                    direction = new Vector3(Input.GetAxisRaw("HorizontalLeft"), Input.GetAxisRaw("VerticalLeft"), 0).normalized;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * Time.deltaTime * DataHolder.Instance.GeneralData.cursorSpeed;
    }

    private void Start()
    {
        controlPlayer = new IA_PlayerCursor();
        controlPlayer.Enable();
        controlPlayer.Player.Move.performed += OnMove;
        controlPlayer.Player.Fire.performed += Shoot;

        var gamepad = Joystick.all[0];
        switch (cursorColor)
        {
            case EnemyData.Color.Red:
                gamepad = Joystick.all[0];
                GPCtrl.Instance.UICtrl.redScore.SetValue(playerCurrentPoint);
                break;
            case EnemyData.Color.Blue:
                gamepad = Joystick.all[1];
                GPCtrl.Instance.UICtrl.blueScore.SetValue(playerCurrentPoint);
                break;
        }

        var user = InputUser.PerformPairingWithDevice(gamepad);
        user.AssociateActionsWithUser(controlPlayer);
    }
    #endregion
}
