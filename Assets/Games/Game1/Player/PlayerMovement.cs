using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform[] m_transforms;
    [SerializeField] private InputPlayerManagerCustom m_inputPlayerManager;
    [SerializeField] private int m_index = 0;
    private int m_mooveSpeed = 1;

    private void OnEnable()
    {
        m_inputPlayerManager.OnMoveLeft += MoveToPrevPosition;
        m_inputPlayerManager.OnMoveRight += MoveToNextPosition;
    }

    private void OnDisable()
    {
        m_inputPlayerManager.OnMoveLeft -= MoveToPrevPosition;
        m_inputPlayerManager.OnMoveRight -= MoveToNextPosition;
    }

    private void Start()
    {
        m_index = 2;
        UpdatePosition();
    }

    public void MoveToNextPosition()
    {
        m_index += m_mooveSpeed;
        m_index = Mathf.Clamp(m_index, 0, m_transforms.Length - 1);
        UpdatePosition();
    }

    public void MoveToPrevPosition()
    {
        m_index -= m_mooveSpeed;
        m_index = Mathf.Clamp(m_index, 0, m_transforms.Length - 1);
        UpdatePosition();
    }

    public void MoveToDirection(int direction)
    {
        m_index = m_index + m_mooveSpeed * direction;
        m_index = Mathf.Clamp(m_index, 0, m_transforms.Length - 1);
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        transform.position = m_transforms[m_index].position;
    }

    public int GetCurrentIndex()
    {
        return m_index;
    }
}