using System;
using UnityEngine;
/// <summary>
/// Canvas fader works to show and hide a canvas group
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public sealed class CanvasFader : MonoBehaviour
{
    #region Variables

    [SerializeField]
    [Tooltip("Start this canvas group faded out")]
    bool m_startFadedOut = false;

    [SerializeField]
    [Tooltip("Fade in time")]
    float m_fadeInTime = 1f;

    [SerializeField]
    [Tooltip("Fade out time")]
    float m_fadeOutTime = 1f;

    [SerializeField]
    [Tooltip("sets the canvas group as non interactable when faded out, unless it was not interactable to begin with")]
    bool m_effectsInteractable = true;

    [SerializeField]
    [Tooltip("sets the canvas group as non b when fadlocking of reaycasts when faded out, unless it was not blocking raycasts to begin with")]
    bool m_effectBlocksRaycasts = true;

    // components
    CanvasGroup m_canvasGroup = null;

    // state 
    bool m_originalInteractableState = false;
    bool m_originalBlocksRaycastsState = false;
    float? m_fadeTime = null;
    float m_totalFadeTime = 0f;
    bool m_fadeIn = true;
    bool m_paused = false;

    #endregion

    #region Properties

    /// <summary>
    /// The faded value
    /// </summary>
    public float FadedValue { get { return m_fadeTime.HasValue ? (m_fadeTime.Value / m_totalFadeTime) : (m_fadeIn ? 1f : 0f); } }

    /// <summary>
    /// Whether or not this canvas is showing
    /// </summary>
    public bool IsShowing { get { return m_fadeIn; } }

    #endregion

    #region Callbacks

    /// <summary>
    /// Callback for when the canvas fader is shown
    /// </summary>
    public Action Shown;

    /// <summary>
    /// Callback for when the canvas fader is hidden
    /// </summary>
    public Action Hidden;

    /// <summary>
    /// Callback for when the canvas fader is fully shown (alpha 1)
    /// </summary>
    public Action FullyShown;

    /// <summary>
    /// Callback for when the canvas fader is fully hidden (alpha 0)
    /// </summary>
    public Action FullyHidden;

    #endregion

    #region Construction
    public void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_originalInteractableState = m_canvasGroup.interactable;
        m_originalBlocksRaycastsState = m_canvasGroup.blocksRaycasts;

        if (m_startFadedOut)
        {
            Hide(0f);
        }
        else
        {
            Show(0f);
        }
    }

    #endregion

    #region Update
    public void Update()
    {
        if (m_paused)
            return;

        if (m_fadeTime.HasValue)
        {
            // instant
            if (Mathf.Approximately(m_totalFadeTime, 0f))
            {
                m_fadeTime = null;
                m_canvasGroup.alpha = m_fadeIn ? 1f : 0f;

                if (m_fadeIn)
                {
                    FullyShown?.Invoke();
                }
                else
                {
                    FullyHidden?.Invoke();
                }
            }
            else
            {
                m_canvasGroup.alpha = Mathf.Lerp(m_fadeIn ? 1f : 0f, m_fadeIn ? 0f : 1f, m_fadeTime.Value / m_totalFadeTime);
                m_fadeTime -= Time.deltaTime;

                if (m_fadeTime < 0f)
                {
                    m_fadeTime = null;
                    m_canvasGroup.alpha = m_fadeIn ? 1f : 0f;

                    if (m_fadeIn)
                    {
                        FullyShown?.Invoke();
                    }
                    else
                    {
                        FullyHidden?.Invoke();
                    }
                }
            }
        }
    }

    #endregion

    #region Display

    /// <summary>
    /// pauses this canvas faders funcitonality
    /// </summary>
    public void Pause()
    {
        m_paused = true;
    }

    /// <summary>
    /// Unpause this canvas faders funcitonality
    /// </summary>
    public void Unpause()
    {
        m_paused = false;
    }

    /// <summary>
    /// fade in the canvas group
    /// </summary>
    public void Show()
    {
        Set(m_fadeInTime, true);
    }

    /// <summary>
    /// fade in the canvas group
    /// </summary>
    /// <param name="overrideTime">Override the fade in time</param>
    public void Show(float overrideTime)
    {
        Set(overrideTime, true);
    }

    /// <summary>
    /// fade out the canvas group
    /// </summary>
    public void Hide()
    {
        Set(m_fadeOutTime, false);
    }

    /// <summary>
    /// fade out the canvas group
    /// </summary>
    /// <param name="overrideTime">Override the fade in time</param>
    public void Hide(float overrideTime)
    {
        Set(overrideTime, false);
    }

    void Set(float fadeTime, bool value)
    {
        m_fadeTime = fadeTime;
        m_totalFadeTime = fadeTime;
        m_fadeIn = value;
        m_canvasGroup.interactable = m_effectsInteractable ? value : m_originalInteractableState;
        m_canvasGroup.blocksRaycasts = m_effectBlocksRaycasts ? value : m_originalBlocksRaycastsState;

        // callbacks
        if (value)
        {
            Shown?.Invoke();
        }
        else
        {
            Hidden?.Invoke();
        }
    }

    #endregion
}
