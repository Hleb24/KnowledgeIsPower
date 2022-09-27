using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor {
  public class Tools : MonoBehaviour {
    [MenuItem("Editor/Clear Prefs")]
    public static void ClearPrefs() {
      PlayerPrefs.DeleteAll();
      PlayerPrefs.Save();
    }
  }
}