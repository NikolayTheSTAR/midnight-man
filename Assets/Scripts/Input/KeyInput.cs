using System;
using UnityEngine;
using Zenject;

public class KeyInput : MonoBehaviour
{
    private const float rotationSpeed = 10;

    private bool up;
    private bool down;
    private bool left;
    private bool right;

    private float sharpX = 0;
    private float sharpY = 0;
    private float smoothX = 0;
    private float smoothY = 0;

    private IKeyInputHandler keyInputHandler;

    [Inject]
    private void Construct(IKeyInputHandler keyInputHandler)
    {
        this.keyInputHandler = keyInputHandler;
    }

    private bool inLock = false;

    public void SetLock(bool inLock)
    {
        this.inLock = inLock;
        sharpX = 0;
        sharpY = 0;
        smoothX = 0;
        smoothY = 0;
    }

    void Update()
    {
        if (inLock) return;

        // arrows
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PressButton(DirectionType.Left, true);
        if (Input.GetKeyUp(KeyCode.LeftArrow)) PressButton(DirectionType.Left, false);

        if (Input.GetKeyDown(KeyCode.RightArrow)) PressButton(DirectionType.Right, true);
        if (Input.GetKeyUp(KeyCode.RightArrow)) PressButton(DirectionType.Right, false);

        if (Input.GetKeyDown(KeyCode.UpArrow)) PressButton(DirectionType.Up, true);
        if (Input.GetKeyUp(KeyCode.UpArrow)) PressButton(DirectionType.Up, false);

        if (Input.GetKeyDown(KeyCode.DownArrow)) PressButton(DirectionType.Down, true);
        if (Input.GetKeyUp(KeyCode.DownArrow)) PressButton(DirectionType.Down, false);

        // WASD
        if (Input.GetKeyDown(KeyCode.A)) PressButton(DirectionType.Left, true);
        if (Input.GetKeyUp(KeyCode.A)) PressButton(DirectionType.Left, false);

        if (Input.GetKeyDown(KeyCode.D)) PressButton(DirectionType.Right, true);
        if (Input.GetKeyUp(KeyCode.D)) PressButton(DirectionType.Right, false);

        if (Input.GetKeyDown(KeyCode.W)) PressButton(DirectionType.Up, true);
        if (Input.GetKeyUp(KeyCode.W)) PressButton(DirectionType.Up, false);

        if (Input.GetKeyDown(KeyCode.S)) PressButton(DirectionType.Down, true);
        if (Input.GetKeyUp(KeyCode.S)) PressButton(DirectionType.Down, false);

        if (Input.GetKeyDown(KeyCode.E)) keyInputHandler.OnStartActionInput();
        if (Input.GetKeyUp(KeyCode.E)) keyInputHandler.OnEndActionInput();

        DoInput();
    }

    private int pressedButtonsCount = 0;

    private void PressButton(DirectionType directionType, bool press)
    {
        switch (directionType)
        {
            case DirectionType.Up:
                up = press;
                break;
            case DirectionType.Down:
                down = press;
                break;
            case DirectionType.Left:
                left = press;
                break;
            case DirectionType.Right:
                right = press;
                break;
        }

        if (press)
        {
            pressedButtonsCount++;
            if (pressedButtonsCount == 1) keyInputHandler.OnStartKeyInput();
        }
        else
        {
            pressedButtonsCount--;
            if (pressedButtonsCount == 0) keyInputHandler.OnEndKeyInput();
        }
    }

    public void BreakInput()
    {
        up = false;
        down = false;
        right = false;
        left = false;
        DoInput();
    }

    private void DoInput()
    {
        if (up) sharpY = 1;
        else if (down) sharpY = -1;
        else sharpY = 0;

        if (right) sharpX = 1;
        else if (left) sharpX = -1;
        else sharpX = 0;

        smoothX = Mathf.Lerp(smoothX, sharpX, rotationSpeed * Time.deltaTime);
        smoothY = Mathf.Lerp(smoothY, sharpY, rotationSpeed * Time.deltaTime);

        if (Math.Abs(smoothX) < 0.01f && Math.Abs(smoothY) < 0.01f)
        {
            smoothX = 0;
            smoothY = 0;
        }

        if (smoothX == 0 && smoothY == 0)
        {
            if (currentInputVectorIsZero) return;
            else currentInputVectorIsZero = true;
        }
        else currentInputVectorIsZero = false;

        keyInputHandler.KeyInput(new Vector2(smoothX, smoothY));
    }

    private bool currentInputVectorIsZero = true;
}

public interface IKeyInputHandler
{
    void OnStartKeyInput();
    void KeyInput(Vector2 direction);
    void OnEndKeyInput();
    void OnStartActionInput();
    void OnEndActionInput();
}

public enum DirectionType
{
    Up,
    Down,
    Left,
    Right
}