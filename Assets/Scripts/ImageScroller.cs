using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField]
    private Vector2 _scrollSpeed;

    private RawImage _image;

    private Rect _rawImageRect;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<RawImage>();
        _rawImageRect = _image.uvRect;   
    }

    // Update is called once per frame
    void Update()
    {
        _rawImageRect.x += _scrollSpeed.x * Time.deltaTime;
        _rawImageRect.y += _scrollSpeed.y * Time.deltaTime;
        _rawImageRect.width = _image.uvRect.width;
        _rawImageRect.height = _image.uvRect.height;

        _image.uvRect = _rawImageRect;
    }
}
