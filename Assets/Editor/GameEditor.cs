using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class GameEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Enemy enemy = (Enemy)target;

        
    }
    public void OnSceneGUI()
    {

    }

   

}
