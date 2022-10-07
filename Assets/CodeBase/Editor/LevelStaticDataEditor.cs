﻿using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor {
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor {
    private const string INITIAL_POINT_TAG = "InitialPoint";

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      var levelData = (LevelStaticData)target;
      if (GUILayout.Button("Collect")) {
        levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>().Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterId, x.transform.position)).ToList();
        levelData.LevelKey = SceneManager.GetActiveScene().name;
        levelData.InitialHeroPosition = GameObject.FindWithTag(INITIAL_POINT_TAG).transform.position;
      }
      
      EditorUtility.SetDirty(target);
    }
  }
}