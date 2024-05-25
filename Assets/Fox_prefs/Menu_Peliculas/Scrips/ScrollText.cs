
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;


public class ScrollText : UdonSharpBehaviour
{

    private ScrollRect _scroll;
    private bool moveway = true;
    [Range(0, 0.5f)] public float _speed = 0.1f;
    public bool horizontal = true;
    void Start()
    {
        _scroll = this.GetComponent<ScrollRect>();
    }

    private void Update()
    {
        if (horizontal) ScrollHorizontal();
        else ScrollVertical();
    }

    void ScrollHorizontal()
    {
        if (_scroll.horizontalNormalizedPosition > 0 && _scroll.horizontalNormalizedPosition < 1)
        {
            if (moveway) _scroll.horizontalNormalizedPosition += _speed * Time.deltaTime;
            else _scroll.horizontalNormalizedPosition -= _speed * Time.deltaTime;
        }
        else
        {
            moveway = !moveway;
            if (_scroll.horizontalNormalizedPosition >= 1) _scroll.horizontalNormalizedPosition = 0.999f;
            else _scroll.horizontalNormalizedPosition = 0.001f;
        }
    }

    void ScrollVertical()
    {
        if (_scroll.verticalNormalizedPosition > 0 && _scroll.verticalNormalizedPosition < 1)
        {
            if (moveway) _scroll.verticalNormalizedPosition -= _speed * Time.deltaTime;
            else _scroll.verticalNormalizedPosition += _speed * Time.deltaTime;
        }
        else
        {
            moveway = !moveway;
            if (_scroll.verticalNormalizedPosition >= 1) _scroll.verticalNormalizedPosition = 0.999f;
            else _scroll.verticalNormalizedPosition = 0.001f;
        }
    }


}
