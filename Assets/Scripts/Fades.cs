using UnityEngine;

public class Fades : MonoBehaviour
{
    public static float fadeOutTime = 1f; //how long a fade out takes - doubled because fade in happens at this speed also

    public static float fadeOutCounter = 0; //used to calculate alpha when divided by fadeOutTime

    private void OnGUI()
    {
        if (fadeOutCounter > 0) Fade(fadeOutCounter / fadeOutTime);
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
