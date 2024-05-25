
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class ScrollText_Single : UdonSharpBehaviour
{
    private ScrollRect _scroll;

    [Range(0, 0.5f)] public float _speed = 0.05f;
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
        if (_scroll.horizontalNormalizedPosition > 1)
        {
            _scroll.horizontalNormalizedPosition = 0;
        }
        _scroll.horizontalNormalizedPosition += _speed * Time.deltaTime;
    }

    void ScrollVertical()
    {
        if (_scroll.verticalNormalizedPosition < 0)
        {
            _scroll.verticalNormalizedPosition = 1;
        }
        _scroll.verticalNormalizedPosition -= _speed * Time.deltaTime;

    }



}
