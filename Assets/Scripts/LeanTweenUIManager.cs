using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LeanTweenUIManager))]
[CanEditMultipleObjects]
public class LeanTweenUIManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (Application.isPlaying)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 14;
            buttonStyle.fixedHeight = 20;

            if (GUILayout.Button("Preview Animation", buttonStyle))
            {
                foreach (var obj in targets)
                {
                    LeanTweenUIManager script = (LeanTweenUIManager)obj;
                    script.PreviewAnimation();
                }
            }

            if (GUILayout.Button("Preview End Animation", buttonStyle))
            {
                foreach (var obj in targets)
                {
                    LeanTweenUIManager script = (LeanTweenUIManager)obj;
                    script.PreviewEndAnimation();
                }
            }
        }

        EditorGUILayout.Space(5);
        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif


public class LeanTweenUIManager : MonoBehaviour
{
    public enum AnimationType { Position, LayoutPosition, Scale, Rotation, Color, Size }
    public enum AnimationDirection { In, Out, Both }

    public AnimationType startAnimationType;
    public AnimationType endAnimationType;
    public AnimationDirection animationDirection;
    public LeanTweenType easeType = LeanTweenType.easeOutQuad;

    [Header("Components")]
    public RectTransform targetRectTransform;
    public CanvasGroup targetCanvasGroup;
    public Graphic targetGraphic;
    public LayoutElement targetLayout;

    [Header("Start Animation")]
    public Vector3 startFromValue;
    public Vector3 startToValue;
    public float startDuration = 0.3f;
    public float startDelay = 0f;

    [Header("End Animation")]
    public Vector3 endFromValue;
    public Vector3 endToValue;
    public float endDuration = 0.3f;
    public float endDelay = 0f;

    [Header("Animations")]
    public bool fadeIn = false;
    public bool fadeOut = false;
    public float fadeDuration = 0.5f;
    public bool loop = false;
    public bool pingPong = false;

    [Header("Configurations")]
    public bool playEndAnimation;
    public float animDuration;

    private LTDescr tween;

    void OnEnable()
    {
        targetRectTransform = GetComponent<RectTransform>();
        targetLayout = GetComponent<LayoutElement>();
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (animationDirection == AnimationDirection.In || animationDirection == AnimationDirection.Both)
        {
            PlayTween(startAnimationType, startFromValue, startToValue, startDuration, startDelay);

            if (fadeIn)
            {
                if (targetCanvasGroup == null)
                {
                    gameObject.AddComponent<CanvasGroup>();
                    targetCanvasGroup = GetComponent<CanvasGroup>();
                }

                targetCanvasGroup.alpha = 0f;
                tween = LeanTween.alphaCanvas(targetCanvasGroup, 1f, fadeDuration).setDelay(startDelay);
            }

            Show();
        }
    }

    private void Update()
    {
        if(playEndAnimation == true)
        {
            if(animDuration == 0)
            {
                return;
            } else
            {
                Invoke("PlayEndAnimation", animDuration);
            }
        }
    }

    public void PlayEndAnimation(System.Action onComplete = null)
    {
        if (animationDirection == AnimationDirection.Out || animationDirection == AnimationDirection.Both)
        {
            PlayTween(endAnimationType, endFromValue, endToValue, endDuration, endDelay);

            if (fadeOut)
            {
                if (targetCanvasGroup == null)
                {
                    gameObject.AddComponent<CanvasGroup>();
                    targetCanvasGroup = GetComponent<CanvasGroup>();
                }

                tween = LeanTween.alphaCanvas(targetCanvasGroup, 0f, fadeDuration)
                    .setDelay(endDelay)
                    .setOnComplete(() =>
                    {
                        Hide();
                        onComplete?.Invoke();
                    });
            }
            else
            {
                tween.setOnComplete(() =>
                {
                    Hide();
                    onComplete?.Invoke();
                });
            }
        }
    }

    private void PlayTween(AnimationType type, Vector3 from, Vector3 to, float duration, float delay)
    {
        switch (type)
        {
            case AnimationType.Position:
                targetRectTransform.anchoredPosition = from;
                tween = LeanTween.move(targetRectTransform, to, duration).setEase(easeType).setDelay(delay);
                break;

            case AnimationType.Scale:
                targetRectTransform.localScale = from;
                tween = LeanTween.scale(targetRectTransform, to, duration).setEase(easeType).setDelay(delay);
                break;

            case AnimationType.Rotation:
                targetRectTransform.eulerAngles = from;
                tween = LeanTween.rotate(targetRectTransform.gameObject, to, duration).setEase(easeType).setDelay(delay);
                break;

            case AnimationType.Color:
                if (targetGraphic == null) targetGraphic = gameObject.AddComponent<Graphic>();
                targetGraphic.color = new Color(from.x, from.y, from.z, 1);
                tween = LeanTween.color(targetGraphic.gameObject, new Color(to.x, to.y, to.z, 1), duration).setEase(easeType).setDelay(delay);
                break;

            case AnimationType.LayoutPosition:
                if (targetRectTransform == null || targetLayout == null) return;
                targetRectTransform.anchoredPosition = from;
                tween = LeanTween.value(gameObject, from, to, duration).setEase(easeType).setDelay(delay).setOnUpdate((Vector3 val) =>
                {
                    targetRectTransform.anchoredPosition = val;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(targetRectTransform);
                });
                break;

            case AnimationType.Size:
                targetRectTransform.sizeDelta = from;
                tween = LeanTween.value(gameObject, from, to, duration).setEase(easeType).setDelay(delay).setOnUpdate((Vector2 val) =>
                {
                    targetRectTransform.sizeDelta = val;
                });
                break;
        }

        if (loop)
        {
            tween.setLoopClamp();
        }
        if (pingPong)
        {
            tween.setLoopPingPong();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    [ContextMenu("Preview Animation")]
    public void PreviewAnimation()
    {
        PlayAnimation();
    }

    [ContextMenu("Preview End Animation")]
    public void PreviewEndAnimation()
    {
        PlayEndAnimation();
    }
}
