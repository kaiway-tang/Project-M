using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Transform swordHUDTrfm;

    float screenXSize, screenYSize, lastScreenXSize, lastScreenYSize;

    [SerializeField] SpriteRenderer vignetteRenderer, vignetteBoostRenderer, blackCoverSpriteRenderer, manaFlashRenderer;

    float blackCoverTargetAlpha;
    Color fadeRate = new Color(0, 0, 0, 0.01f);
    Color color;

    int alignHUDTimer;

    static HUDManager self;
    private void Awake()
    {
        self = GetComponent<HUDManager>();
    }

    private void Start()
    {
        CalculateScreenSize();
        AlignHUDElements();
    }
    private void FixedUpdate()
    {
        ProcessFading();
        ManageHUDAlignment();
    }

    bool CalculateScreenSize()
    {
        screenYSize = 2 * Camera.main.orthographicSize;
        screenXSize = screenYSize * Camera.main.aspect;

        if (Mathf.Abs(screenXSize - lastScreenXSize) > .001f || Mathf.Abs(screenYSize - lastScreenYSize) > .001f)
        {
            lastScreenXSize = screenXSize;
            lastScreenYSize = screenYSize;
            return true;
        }
        return false;
    }

    void AlignHUDElements()
    {
        vignetteRenderer.transform.localScale = new Vector3(.1788f * screenXSize, .37f * screenYSize, 1);

        Vector3 hudPosition = swordHUDTrfm.localPosition;
        hudPosition.x = screenXSize * -.5f + 7.5f;
        swordHUDTrfm.localPosition = hudPosition;
    }

    void ManageHUDAlignment()
    {
        if (alignHUDTimer > 0) { alignHUDTimer--; }
        else
        {
            alignHUDTimer = 25;
            if (CalculateScreenSize())
            {
                AlignHUDElements();
            }
        }
    }

    bool fadingManaFlash;
    public static void FlashManaBar()
    {
        self.fadingManaFlash = true;
        self.manaFlashRenderer.color = Color.white;
    }


    public static void SetBlackCoverOpacity(float alpha)
    {

    }

    bool fadingBlackCover;
    public static void FadeBlackCoverOpacity(float targetAlpha)
    {
        if (Mathf.Abs(self.blackCoverSpriteRenderer.color.a - targetAlpha) < .01f) { return; }

        self.fadingBlackCover = true;
        self.blackCoverTargetAlpha = targetAlpha;
    }

    void ProcessFading()
    {
        if (fadingBlackCover)
        {
            if (blackCoverSpriteRenderer.color.a - blackCoverTargetAlpha > .01f) { blackCoverSpriteRenderer.color -= fadeRate; }
            else if (blackCoverSpriteRenderer.color.a - blackCoverTargetAlpha < .01f) { blackCoverSpriteRenderer.color -= fadeRate; }
            else
            {
                blackCoverSpriteRenderer.color = new Color(0, 0, 0, blackCoverTargetAlpha);
                fadingBlackCover = false;
            }
        }

        if (fadingVignette)
        {
            if (vignetteBoostRenderer.color.a > 0)
            {
                vignetteBoostRenderer.color -= fadeRate;
            }
            if (vignetteRenderer.color.a > 0)
            {
                vignetteRenderer.color -= fadeRate;
            }
            else
            {
                color = vignetteRenderer.color;
                color.a = 0;
                vignetteRenderer.color = color;
                vignetteBoostRenderer.color = color;
                fadingVignette = false;
            }
        }

        if (fadingManaFlash)
        {
            if (manaFlashRenderer.color.a > 0)
            {
                manaFlashRenderer.color -= fadeRate * 4;
            }
            else
            {
                color = manaFlashRenderer.color;
                color.a = 0;
                manaFlashRenderer.color = color;
                fadingManaFlash = false;
            }
        }
    }

    bool fadingVignette;
    public static void SetVignetteOpacity(float alpha)
    {
        self.fadingVignette = true;

        if (self.vignetteRenderer.color.a < alpha)
        {
            self.color = self.vignetteRenderer.color;
            self.color.a = alpha;
            self.vignetteRenderer.color = self.color;

            if (alpha > 1)
            {
                self.color.a = alpha - 1;
                self.vignetteBoostRenderer.color = self.color;
            }
        }
    }
}