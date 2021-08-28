using System.Collections;
using UnityEngine;
using System;

public class FadeOut : MonoBehaviour
{
    public bool startFadeOut = false; //only used in update - if this bool is true, start fadig out and change it to false
    public float fadeOutTime = 1f; //how long a fade out takes - doubled because fade in happens at this speed also

    private bool fadeOut = false; //while true, fading out - else fading in
    private float fadeOutCounter = 0; //used to calculate alpha when divided by fadeOutTime

    private bool hasStartedBrighteningThisF=false;

    public delegate void outDlg(out bool b);
    public outDlg onFadeMiddle;
    private bool canFadeOut=false;

    private void Update()
    {
        if (startFadeOut)
        {
            startFadeOut = false;
            StartFadeOut();
        }
    }

    private void FixedUpdate()
    {
        //opaque -> black
        if (fadeOut) {
            hasStartedBrighteningThisF = false;
            fadeOutCounter = Mathf.MoveTowards(fadeOutCounter, fadeOutTime, Time.fixedDeltaTime);
        } 

        //black -> opaque
        else if (fadeOutCounter > 0) {
            
            //if we started brightening right now
            if (!hasStartedBrighteningThisF) {
                hasStartedBrighteningThisF = true;
                canFadeOut = false;
                
                //if you're supposed to do something during complete blackness, do it
                if (onFadeMiddle != null) {
                    onFadeMiddle.Invoke(out canFadeOut); //canFadeOut is supposed to be set to true at the subscribed method's end
                }
                else {
                    canFadeOut = true;
                }
            }

            if (canFadeOut) {
                fadeOutCounter = Mathf.MoveTowards(fadeOutCounter, 0, Time.fixedDeltaTime);
            }
            
        } 

    }

    private void OnGUI()
    {
        if (fadeOutCounter > 0) Fade(fadeOutCounter / fadeOutTime);
    }

    public void StartFadeOut()
    {
        StartCoroutine(ToggleFadeOut());
    }

    IEnumerator ToggleFadeOut()
    {
        fadeOut = true;
        yield return new WaitForSeconds(fadeOutTime);
        fadeOut = false;
    }

    void Fade(float alpha)
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        Color color = Color.HSVToRGB(0, 0, 0);
        color.a = alpha;
        DrawQuad(screenRect, color);
    }

    void DrawQuad(Rect position, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }

}
