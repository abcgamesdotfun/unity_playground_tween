using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abc {
  public class PlaygroundTweenController : MonoBehaviour {
    public static readonly string TweenTypeFade = "Fade";
    public static readonly string TweenTypeScale = "Scale";
    public static readonly string TweenTypeMove = "Move";
    public static readonly string TweenTypeCombo = "Combo";
    public static readonly List<string> TweenTypes = new List<string> {
      TweenTypeFade,
      TweenTypeScale,
      TweenTypeMove,
      TweenTypeCombo
    };

    public bool IsAnimating => twObjects.TrueIf(x => x.IsAnimating);
    public Action<string> OnChangeTweenType { get; set; }

    public List<TweenObject> twObjects = new List<TweenObject>();
    [Tooltip("How long the tween will play in second(s)?")]
    public float durationTween = .5f;
    [Tooltip("The delay of start tween in second(s)?")]
    public float delayTween = .1f;

    int _curTypeIdx = 0;

    public void Animate() {
      twObjects.Loop((obj, i) => {
        // we multiply with index to make a 'wavy' effect
        obj.Animate(durationTween, i * delayTween);
      });
    }

    public void NextTweenType() {
      if (IsAnimating) return;

      ++_curTypeIdx;

      if (_curTypeIdx >= TweenTypes.Count) {
        _curTypeIdx = 0;
      }

      string curTwType = TweenTypes[_curTypeIdx];

      twObjects.ForEach((obj) => {
        obj.SetType(curTwType);
      });

      Animate();

      OnChangeTweenType?.Invoke(curTwType);
    }
  }
}