using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using FMODUnity;

public class PlayerCursor : MonoBehaviour
{
    #region Properties
    Vector3 direction;
    [Header("COMPONENTS")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer circleSprite;
    [SerializeField] private SpriteRenderer internCircleSprite;
    public PlayerInput playerInput;

    [Header("CURRENT INFO")]
    public List<Enemy> targetList = new List<Enemy>();
    public int playerCurrentPoint = 0;

    [Header("ACTION BUTTON")]
    public KeyCode actionButton;
    public EnemyData.Color cursorColor;
    private char inputValue;
    #endregion

    #region Methods
    public void Shoot()
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
                Shoot();
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

    public void OnMove(InputValue value)
    {
        if (!DataHolder.Instance.GeneralData.computerMode)
        {
            direction = value.Get<Vector2>().normalized;
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
            targetList.Add(_enemy);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null)
        {
            if(!GPCtrl.Instance.loveFrenzy) _enemy.ChangeColor(_enemy.currentColor);
            targetList.Remove(_enemy);
        }
    }

    void Update()
    {
        //byte[] input = serialController.ReadSerialMessage();
        //if(input.Contains<byte>((byte)inputValue)) {
        //    Shoot();
        //}
        if (Input.GetKeyDown(actionButton))
        {
            Shoot();
        }
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

    //public void OnFire(InputValue value)
    //{
    //    Shoot();
    //}

    private void FixedUpdate()
    {
        rb.velocity = direction * Time.deltaTime * DataHolder.Instance.GeneralData.cursorSpeed;
    }

    private void Start()
    {
        switch (cursorColor)
        {
            case EnemyData.Color.Red:
                GPCtrl.Instance.UICtrl.redScore.SetValue(playerCurrentPoint);
                inputValue = 'R';
                break;
            case EnemyData.Color.Blue:
                GPCtrl.Instance.UICtrl.blueScore.SetValue(playerCurrentPoint);
                inputValue = 'B';
                break;
        }
    }
    #endregion


}
