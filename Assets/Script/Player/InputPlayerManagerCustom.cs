using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class InputPlayerManagerCustom : MonoBehaviour
{
    public event Action OnMoveLeft;
    public event Action OnMoveRight;

    [SerializeField] private float _tapDuration = 1.0f;
    private float _tapTimer = 0.0f;
    private bool _IsTouching = false;
    private float width = 0.0f;
    private float height = 0.0f;
    
    private void Start() 
    {
        width = Screen.width;
        height = Screen.height;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);

            if (firstTouch.phase == TouchPhase.Began)
            {
                _IsTouching = true;
                _tapTimer = 0.0f;
            }
            else if (firstTouch.phase == TouchPhase.Ended)
            {
                _IsTouching = false;

                if (_tapTimer <= _tapDuration)
                {
                    Debug.LogWarning($"Tap OK!! Touch at {firstTouch.rawPosition}, end at {_tapTimer}");

                    if (firstTouch.position.x > width / 2) 
                    {
                        Debug.Log("Tap Right");
                        MoveRight();
                    }
                    else
                    {
                        Debug.Log("Tap Left");
                        MoveLeft();
                    }
                }

                _tapTimer = 0.0f;
            }
        }

        if (_IsTouching)
        {
            _tapTimer += Time.deltaTime; 
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
    }

    public void MoveLeft()
    {
        OnMoveLeft?.Invoke();
    }

    public void MoveRight()
    {
        OnMoveRight?.Invoke();
    }
}