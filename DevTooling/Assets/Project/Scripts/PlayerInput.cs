using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public float TurnInput { get; private set; }

    // Update is called once per frame
    void Update()
    {
        int xInput = 0;
        int yInput = 0;

        if (Keyboard.current.wKey.isPressed)
        {
            yInput += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            yInput -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            xInput -= 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            xInput += 1;
        }

        MoveInput = new Vector2(xInput, yInput);
    }
}
