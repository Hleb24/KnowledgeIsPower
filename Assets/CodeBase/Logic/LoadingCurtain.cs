using System.Collections;
using UnityEngine;

namespace CodeBase.Logic {
  public class LoadingCurtain : MonoBehaviour {
    private const float SECONDS = 0.03f;
    public CanvasGroup Curtain;

    private void Awake() {
      DontDestroyOnLoad(this);
    }

    public void Show() {
      gameObject.SetActive(true);
      Curtain.alpha = 1f;
    }

    public void Hide() {
      StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
      var wait = new WaitForSeconds(SECONDS);
      while (Curtain.alpha > 0) {
        Curtain.alpha -= SECONDS;
        yield return wait;
      }

      gameObject.SetActive(false);
    }
  }
}