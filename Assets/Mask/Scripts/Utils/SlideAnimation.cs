using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;
using Kk;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class SlideAnimation : MonoBehaviour
{
    [Serializable]
    public enum Slidetype
    {
        leftSide,
        rightSide,
        topSide,
        bottomSide,
    }

    [SerializeField] Slidetype type;
    [SerializeField] TimeForTwerkingLerp.TwerkingAnimation typeLerp = TimeForTwerkingLerp.TwerkingAnimation.OutQuad;
    [SerializeField] bool isInit;
    [SerializeField] bool fadeOnly;
    [Range(0.1f, 5)]
    [SerializeField] float delayFade;
    [SerializeField] float slideValue;
    [SerializeField] UnityEvent OnSlideInEvent;
    [SerializeField] UnityEvent OnSlideOutEvent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 initPos;
    private Vector2 targetPos;
    private bool isInitPos;
    public void SetSlideType(Slidetype slidetype) => type = slidetype;
    private void Awake()
    {
        InitSlide();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    void InitStartPosition()
    {
        InitSlide();
        if (isInitPos || fadeOnly) return;
        initPos = rectTransform.anchoredPosition;
        isInitPos = true;
    }

    void InitSlide()
    {
        if (!rectTransform && TryGetComponent(out RectTransform rect))
            rectTransform = rect;
        if (!canvasGroup && TryGetComponent(out CanvasGroup canvas))
            canvasGroup = canvas;

    }
    private void OnEnable()
    {
        InitSide();
        canvasGroup.alpha = 0;
        if (isInit) SlideIn();
    }

    void InitSide()
    {
        InitStartPosition();
        targetPos = initPos;
        slideValue = slideValue != 0 ? slideValue : SliderValue();
        if (!fadeOnly)
            switch (type)
            {
                case Slidetype.leftSide:
                    targetPos = new Vector2(targetPos.x - slideValue, targetPos.y);
                    break;
                case Slidetype.rightSide:
                    targetPos = new Vector2(targetPos.x + slideValue, targetPos.y);
                    break;
                case Slidetype.topSide:
                    targetPos = new Vector2(targetPos.x, targetPos.y + slideValue);
                    break;
                case Slidetype.bottomSide:
                    targetPos = new Vector2(targetPos.x, targetPos.y - slideValue);
                    break;
            }
        if (!fadeOnly) rectTransform.anchoredPosition = targetPos;
        this.gameObject.SetActive(true);
    }
    public void SlideIn(Action action = null)
    {
        InitSlide();
        Slide(true, action);
    }
    public void SlideIn()
    {
        InitSlide();
        Slide(true, null);
    }
    public void SlideOut(Action action = null)
    {
        InitSlide();
        Slide(false, action);
    }
    public void SlideOut()
    {
        InitSlide();
        Slide(false, null);
    }
    public void Slide(bool slide)
    {
        StopAllCoroutines();
        if (slide)
        {
            InitSide();
            if (gameObject.activeInHierarchy)
                StartCoroutine(OnSlideIn());
        }
        else
            if (gameObject.activeInHierarchy)
            StartCoroutine(OnSlideOut());
    }
    void Slide(bool slideIn, Action action = null)
    {
        StopAllCoroutines();
        if (slideIn)
        {
            InitSide();
            if (gameObject.activeInHierarchy)
                StartCoroutine(OnSlideIn(action));
        }
        else
            if (gameObject.activeInHierarchy)
            StartCoroutine(OnSlideOut(action));

    }
    IEnumerator OnSlideIn(Action action = null)
    {
        var timer = 0f;
        var alpha = canvasGroup.alpha;
        var position = rectTransform.anchoredPosition;
        while (timer < 1)
        {
            timer += Time.unscaledDeltaTime / delayFade;
            canvasGroup.alpha = timer;
            if (!fadeOnly) rectTransform.anchoredPosition = Vector3.Lerp(position, initPos, TimeForTwerkingLerp.TimeTwerkingLerp(typeLerp, timer));
            yield return null;
        }
        SetCanvasGroup(true);
        if (!fadeOnly)
            rectTransform.anchoredPosition = initPos;
        action?.Invoke();
        OnSlideInEvent?.Invoke();
    }
    IEnumerator OnSlideOut(Action action = null)
    {
        var timer = 0f;
        var alpha = canvasGroup.alpha;
        var position = rectTransform.anchoredPosition;
        while (timer < 1)
        {
            timer += Time.unscaledDeltaTime / delayFade;
            canvasGroup.alpha = alpha - timer;
            if (!fadeOnly) rectTransform.anchoredPosition = Vector3.Lerp(position, targetPos, TimeForTwerkingLerp.TimeTwerkingLerp(typeLerp, timer));
            yield return null;
        }
        if (!fadeOnly)
            rectTransform.anchoredPosition = targetPos;
        SetCanvasGroup(false);
        this.gameObject.SetActive(false);
        action?.Invoke();
        OnSlideOutEvent?.Invoke();
    }
    void SetCanvasGroup(bool isActive)
    {
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
    float SliderValue() => type == Slidetype.leftSide || type == Slidetype.rightSide
            ? Mathf.Abs(rectTransform.rect.width)
            : Mathf.Abs(rectTransform.rect.height);
    void SetAnchorAndPivot(Slidetype slidetype)
    {
        switch (slidetype)
        {
            case Slidetype.leftSide:
                this.rectTransform.pivot = new Vector2(0, this.rectTransform.pivot.y);
                this.rectTransform.anchorMin = new Vector2(0, this.rectTransform.anchorMin.y);
                this.rectTransform.anchorMax = new Vector2(0, this.rectTransform.anchorMax.y);
                break;
            case Slidetype.rightSide:
                this.rectTransform.pivot = new Vector2(1, this.rectTransform.pivot.y);
                this.rectTransform.anchorMin = new Vector2(1, this.rectTransform.anchorMin.y);
                this.rectTransform.anchorMax = new Vector2(1, this.rectTransform.anchorMax.y);
                break;
            case Slidetype.topSide:
                this.rectTransform.pivot = new Vector2(this.rectTransform.pivot.x, 1);
                this.rectTransform.anchorMin = new Vector2(this.rectTransform.anchorMax.x, 1);
                this.rectTransform.anchorMax = new Vector2(this.rectTransform.anchorMax.x, 1);
                break;
            case Slidetype.bottomSide:
                this.rectTransform.pivot = new Vector2(this.rectTransform.pivot.x, 0);
                this.rectTransform.anchorMin = new Vector2(this.rectTransform.anchorMax.x, 0);
                this.rectTransform.anchorMax = new Vector2(this.rectTransform.anchorMax.x, 0);
                break;
        }
    }
    IEnumerator CheckAnimationFinishPlay(Animator anim)
    {
        yield return new WaitForFlow();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
    }
}
