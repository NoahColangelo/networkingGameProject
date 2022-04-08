using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiCam))]
public class MultiCamEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MultiCam _multiCam = (MultiCam)target;

        GUIContent cameraPresets = new GUIContent("Camera Presets");
        _multiCam.presetIndex = EditorGUILayout.Popup(cameraPresets, _multiCam.presetIndex, _multiCam.cameraPresetChoice);

        if(_multiCam.presetIndex == 0)//first person camera
        {
            _multiCam.FirstPersonCamera();
            FillValues(0, _multiCam);
        }

        if (_multiCam.presetIndex == 1)//third person camera
        {
            _multiCam.ThirdPersonCamera();
            FillValues(1, _multiCam);
        }

        if (_multiCam.presetIndex == 2)//top down camera
        {
            _multiCam.TopDownCamera();
            FillValues(2, _multiCam);
        }
    }

    private void FillValues(int index, MultiCam multiCam)
    {
        multiCam.cameraPresetValues[index]._distance = EditorGUILayout.FloatField("Distane", multiCam.cameraPresetValues[index]._distance);

        multiCam.cameraPresetValues[index]._height = EditorGUILayout.FloatField("Height", multiCam.cameraPresetValues[index]._height);

        multiCam.cameraPresetValues[index]._pan = EditorGUILayout.FloatField("Pan", multiCam.cameraPresetValues[index]._pan);

        //EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Look At Offset"/*, GUILayout.MaxWidth(200)*/);

        multiCam.cameraPresetValues[index]._lookAtOffset.x = EditorGUILayout.FloatField("X", multiCam.cameraPresetValues[index]._lookAtOffset.x/*, GUILayout.MaxWidth(50)*/);
        multiCam.cameraPresetValues[index]._lookAtOffset.y = EditorGUILayout.FloatField("Y", multiCam.cameraPresetValues[index]._lookAtOffset.y/*, GUILayout.MaxWidth(50)*/);
        multiCam.cameraPresetValues[index]._lookAtOffset.z = EditorGUILayout.FloatField("Z", multiCam.cameraPresetValues[index]._lookAtOffset.z/*, GUILayout.MaxWidth(50)*/);

        //EditorGUILayout.EndHorizontal();

    }

}
