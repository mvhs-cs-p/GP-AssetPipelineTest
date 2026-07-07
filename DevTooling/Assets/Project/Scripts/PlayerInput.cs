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
        int boostInput = 0;

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            boostInput = 1;
        }

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            yInput += (1 + boostInput);
        }
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            yInput -= (1 + boostInput);
        }
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            xInput -= (1 + boostInput);
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            xInput += (1 + boostInput);
        }

        MoveInput = new Vector2(xInput, yInput);
    }
}
