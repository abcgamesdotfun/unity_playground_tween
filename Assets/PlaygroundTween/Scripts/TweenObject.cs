using System;
using System.Collections.Generic;
using Abc.Tween;
using UnityEngine;

namespace Abc {
  public class TweenObject : MonoBehaviour {
    public bool IsAnimating { get; private set; } = false;

    public SpriteRenderer sr;

    Dictionary<string, Action<float, float>> _handlers = new Dictionary<string, Action<float, float>>();
    InterpolationFloat _tw;
    Vector2 _initPos;
    string _curType = PlaygroundTweenController.TweenTypes.First();

    public void SetType(string t) {
      _curType = t;
    }

    public void Animate(float duration, float delay) {
      if (IsAnimating) return;

      _handlers[_curType](duration, delay);
    }

    void Fade(float duration, float delay) {
      IsAnimating = true;

      float from = 0f;
      float to = 1f - from;

      sr.SetAlpha(from);

      _tw = new InterpolationFloat(0f, 1f, duration, delay);
      _tw.OnLerp += t => {
        float a = t;

        sr.SetAlpha(a);
      };
      _tw.OnDone += () => {
        _tw = null;

        IsAnimating = false;
      };
    }

    void Scale(float duration, float delay) {
      IsAnimating = true;

      float from = 0f;
      float to = 1f - from;

      sr.SetScale(from);

      _tw = new InterpolationFloat(0f, 1f, duration, delay);
      _tw.OnLerp += t => {
        float a = t;

        sr.SetScale(a);
      };
      _tw.OnDone += () => {
        _tw = null;

        IsAnimating = false;
      };
    }

    void Move(float duration, float delay) {
      IsAnimating = true;

      sr.SetWorldPos(_initPos);

      _tw = new InterpolationFloat(0f, 1f, duration, delay);
      _tw.OnLerp += t => {
        Vector2 curPos = AbcBezierCurve.Quadratic(
          _initPos,
          _initPos.AddY(2f),
          _initPos,
          t
        );

        sr.SetWorldPos(curPos);
      };
      _tw.OnDone += () => {
        _tw = null;

        IsAnimating = false;
      };
    }

    void Combo(float duration, float delay) {
      IsAnimating = true;

      sr.SetWorldPos(_initPos);

      _tw = new InterpolationFloat(0f, 1f, duration, delay);
      _tw.OnLerp += t => {
        Vector2 curPos = AbcBezierCurve.Quadratic(
          _initPos,
          _initPos.AddY(2f),
          _initPos,
          t
        );

        sr.SetAlpha(t).SetScale(t).SetWorldPos(curPos);
      };
      _tw.OnDone += () => {
        _tw = null;

        IsAnimating = false;
      };
    }

    void Start() {
      _initPos = sr.WorldPos();

      _handlers[PlaygroundTweenController.TweenTypeFade] = Fade;
      _handlers[PlaygroundTweenController.TweenTypeScale] = Scale;
      _handlers[PlaygroundTweenController.TweenTypeMove] = Move;
      _handlers[PlaygroundTweenController.TweenTypeCombo] = Combo;

      Animate(.5f, 1f);
    }

    void Update() {
      float dt = Time.deltaTime;

      _tw?.Run(dt);
    }
  }
}