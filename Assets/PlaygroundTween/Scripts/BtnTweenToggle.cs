using TMPro;
using UnityEngine;

namespace Abc {
  public class BtnTweenToggle : MonoBehaviour {
    public PlaygroundTweenController controller;
    public TMP_Text txt;

    public void Next() {
      controller.NextTweenType();
    }

    void Start() {
      controller.OnChangeTweenType += twType => {
        txt.text = twType;
      };
    }
  }
}
