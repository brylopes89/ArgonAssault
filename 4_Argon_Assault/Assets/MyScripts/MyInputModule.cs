using UnityEngine;
using UnityEngine.EventSystems;

public class MyInputModule : BaseInputModule
{
    public string m_HorizontalAxis = "LeftStickX";
    public string m_VerticalAxis = "LeftStickY";
    public string m_SubmitButton = "Submit";
    public string m_CancelButton = "Cancel";

    public Vector2 currentMove;
    public Vector2 prevMove;
    public Vector2 nextNav;
    public bool xRelease;
    public int xHold;
    public bool yRelease;
    public int yHold;
    public float deadZone = 0f;

    public int xRepeat;
    public int yRepeat;

    public int xRepeatStart;
    public int xRepeatRate;
    public int xRepeatDelay;

    public int yRepeatStart = 45;
    public int yRepeatRate;
    public int yRepeatDelay;

    private float m_NextAction;


    public override void Process()
    {
        UpdateMoveVector();

        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent) { usedEvent |= SendMoveEventToSelectedObject(); }

            if (!usedEvent) { SendSubmitEventToSelectedObject(); }
        }
    }

    private void UpdateMoveVector()
    {
        prevMove = currentMove;
        nextNav = Vector2.zero;
        currentMove.x = Input.GetAxisRaw(m_HorizontalAxis);
        currentMove.y = Input.GetAxisRaw(m_VerticalAxis);

        if (Mathf.Abs(currentMove.x) > deadZone || Mathf.Abs(currentMove.x) > Mathf.Abs(prevMove.x))
        { xHold++; xRepeat++; }
        if (Mathf.Abs(currentMove.y) > deadZone || Mathf.Abs(currentMove.y) > Mathf.Abs(prevMove.y))
        { yHold++; yRepeat++; }

        if (Mathf.Abs(currentMove.x) < Mathf.Abs(prevMove.x) || Mathf.Abs(currentMove.x) <= deadZone) { xHold = -1; xRepeat = 0; }
        if (Mathf.Abs(currentMove.y) < Mathf.Abs(prevMove.y) || Mathf.Abs(currentMove.y) <= deadZone) { yHold = -1; yRepeat = 0; }

        if (yRepeat > yRepeatStart)
        {
            if (yRepeat % yRepeatRate == 0)
            {
                yHold = 0;
            }
        }

        if (xHold == 0)
        {
            if (currentMove.x < 0) { nextNav.x = -1f; }
            if (currentMove.x > 0) { nextNav.x = 1f; }
        }
        if (yHold == 0)
        {
            if (currentMove.y < 0) { nextNav.y = -1f; }
            if (currentMove.y > 0) { nextNav.y = 1f; }
        }

    }

    private bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }

    private bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)// || m_CurrentInputMode != InputMode.Buttons)
            return false;

        var data = GetBaseEventData();
        if (Input.GetButtonDown(m_SubmitButton))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        if (Input.GetButtonDown(m_CancelButton))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }

    private bool SendMoveEventToSelectedObject()
    {
        float time = Time.unscaledTime;
        var axisEventData = GetAxisEventData(nextNav.x, nextNav.y, 0.6f);
        if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
            || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        }
        return axisEventData.used;
    }
}
