using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using TheSTAR.Data;
using Zenject;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TutorCursor cursor;

    [Space]
    [SerializeField] private RectTransform upPos;
    [SerializeField] private RectTransform bottomPos;
    [SerializeField] private RectTransform leftPos;
    [SerializeField] private RectTransform rightPos;

    [Space]
    [SerializeField] private bool showDebugs = true;

    private DataController data;

    private bool _inTutorial = false;
    private Transform _currentFocusTran;
    private TutorialBasingType? _currentTutorialBasingType;
    private bool _autoUpdatePos = false;
    private string _currentTutorialId = null;

    private readonly Dictionary<string, string> tutorStepsForAnalytics = new ()
    {
        // добавить ID тутора и аналитическое сообщение
    };

    public string CurrentTutorialID => _currentTutorialId;
    public bool InTutorial => _inTutorial;

    public bool IsComplete(string id) => data.gameData.tutorialData.completedTutorials != null && data.gameData.tutorialData.completedTutorials.Contains(id);

    private readonly Dictionary<CursorViewType, CursorTransformData> cursorViewDatas = new()
    {
        { CursorViewType.Default, new(new(0, 0, 0), new(1, 1, 1)) },
        { CursorViewType.UpEdge, new(new(0, 0, 0), new(1, 1, 1)) },
        { CursorViewType.BottomEnge, new(new(0, 0, 0), new(1, -1, 1)) },
        { CursorViewType.LeftEdge, new(new(0, 0, 90), new(-1, 1, 1)) },
        { CursorViewType.RightEdge, new(new(0, 0, -90), new(1, 1, 1)) },
    };

    private CursorViewType _currentCursorViewType;

    [Inject]
    private void Construct(DataController data)
    {
        this.data = data;
    }

    public void TryShowInUI(string id, Transform focusTran, bool autoUpdatePos = false) => TryShowInUI(id, focusTran, out _, autoUpdatePos);
    public void TryShowInUI(string id, Transform focusTran, out bool successful, bool autoUpdatePos = false) => Show(id, focusTran, autoUpdatePos, TutorialBasingType.UI, out successful);
    public void TryShowInWorld(string id, Transform focusTran, bool autoUpdatePos = true) => TryShowInWorld(id, focusTran, out _, autoUpdatePos);
    public void TryShowInWorld(string id, Transform focusTran, out bool successful, bool autoUpdatePos = true) => Show(id, focusTran, autoUpdatePos, TutorialBasingType.World, out successful);

    private void Show(string id, Transform focusTran, bool autoUpdatePos, TutorialBasingType basingType, out bool successful)
    {
        successful = false;

        if (IsComplete(id)) return;
        if (_inTutorial) BreakTutorial();
        if (showDebugs) Debug.Log("[tutor] Show Tutor " + id);

        _inTutorial = true;
        _currentTutorialId = id;
        gameObject.SetActive(true);
        cursor.gameObject.SetActive(true);

        _currentTutorialBasingType = basingType;
        _autoUpdatePos = autoUpdatePos;

        _currentFocusTran = focusTran;
        UpdateCursorPosition();

        successful = true;
    }

    private void SetCursorVisual(bool toRight) => cursor.SetFlip(toRight);

    /// <summary>
    /// Туториал выполнен, он не будет больше показываться
    /// </summary>
    public void CompleteTutorial()
    {
        if (!InTutorial) return;
        CompleteTutorial(_currentTutorialId);
    }

    public void CompleteTutorial(string id)
    {
        data.gameData.tutorialData.CompleteTutorial(id);
        HideTutor();

        if (tutorStepsForAnalytics.ContainsKey(id))
        {
            // todo решить как обращаться к аналитике
            //AnalyticsManager.Instance.Log(AnalyticSectionType.Tutorial, tutorStepsForAnalytics[id]);
        }
    }

    /// <summary>
    /// Туториал скрывается, но не считается завершённым. Он может быть показан позже
    /// </summary>
    public void BreakTutorial()
    {
        if (!InTutorial) return;

        HideTutor();
    }

    private void HideTutor()
    {
        _inTutorial = false;
        _currentTutorialId = null;
        _autoUpdatePos = false;
        gameObject.SetActive(false);
        cursor.gameObject.SetActive(false);

        _currentTutorialBasingType = null;
        _currentFocusTran = null;

        //SaveData();
    }

    public void UpdateCursorPosition()
    {
        // todo разворачивать курсор вправо/влево автоматически

        if (!_inTutorial) return;

        switch (_currentTutorialBasingType)
        {
            case TutorialBasingType.UI:
                cursor.transform.position = _currentFocusTran.position;
                SetCursorVisual(cursor.transform.localPosition.x > 0);
                break;

            case TutorialBasingType.World:
                var focusPos = _currentFocusTran.position;
                var focusScreenPos = Camera.main.WorldToScreenPoint(focusPos);

                SetCursorVisual(false);

                focusScreenPos = new Vector3(
                    MathUtility.Limit(focusScreenPos.x, leftPos.position.x, rightPos.position.x),
                    MathUtility.Limit(focusScreenPos.y, bottomPos.position.y, upPos.position.y),
                    cursor.transform.position.z);
                cursor.transform.position = focusScreenPos;
                break;
        }
    }

    private void Update()
    {
        if (!_inTutorial) return;
        if (!_autoUpdatePos) return;

        UpdateCursorPosition();
    }

    public enum TutorialBasingType
    {
        UI,
        World
    }

    public enum CursorViewType
    {
        Default,
        UpEdge,
        BottomEnge,
        LeftEdge,
        RightEdge
    }
}

public struct CursorTransformData
{
    private Vector3 rotation;
    private Vector3 scale;

    public CursorTransformData(Vector3 rotation, Vector3 scale)
    {
        this.rotation = rotation;
        this.scale = scale;
    }

    public Vector3 Rotation => rotation;
    public Vector3 Scale => scale;
}

public interface ITutorialStarter
{
    void TryShowTutorial();
}