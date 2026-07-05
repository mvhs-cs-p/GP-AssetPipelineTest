using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput m_PlayerInput;
    public float m_MoveSpeed;


    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = m_PlayerInput.MoveInput;
        moveInput = moveInput * Time.deltaTime * m_MoveSpeed;
        transform.Translate(moveInput);
    }
}
