using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using UnityEngine;
using UnityEngine.UI;

namespace NineBitByte.FutureJourney.View
{
  /// <summary> Behavior that uses a HealthBar that follows an object around. </summary>
  public class HealthBarBehavior : BaseBehavior
  {
    private Image _healthImage;
    private IDamageReceiver _damagable;
    private GameObject _owner;
    private RectTransform _rectTransform;

    private Vector2 _canvasOffset;
    private Vector3 _positionOffset;

    /// <summary />
    public void Start()
    {
      _healthImage = transform.Find("Health").GetComponent<Image>();
      _rectTransform = GetComponent<RectTransform>();
      var parentRectTransform = transform.root.GetComponent<RectTransform>();

      _canvasOffset = parentRectTransform.sizeDelta / 2f;
    }

    public void Initialize(GameObject owner, IDamageReceiver damageable, Vector2 positionOffset)
    {
      _owner = owner;
      _damagable = damageable;
      _positionOffset = positionOffset;
    }

    /// <summary />
    protected void LateUpdate()
    {
      if (_damagable == null)
        return;

      if (_owner.IsDestroyed())
      {
        _damagable = null;
        _owner = null;
        UnityExtensions.Destroy(gameObject);
        return;
      }

      _healthImage.fillAmount = _damagable.Health / 100.0f;

      _rectTransform.anchoredPosition = CalculateScreenPosition(_owner.transform.position + _positionOffset);
    }

    /// <summary> Gets the screen position of an item in the world (if it was on screen). </summary>
    private Vector2 CalculateScreenPosition(Vector3 worldPosition) 
      => RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition) - _canvasOffset;
  }
}
