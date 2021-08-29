using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Button : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void OnMouseEnter()
    {
        text.DOColor(Color.yellow, 1f).From(Color.white);
    }

}
