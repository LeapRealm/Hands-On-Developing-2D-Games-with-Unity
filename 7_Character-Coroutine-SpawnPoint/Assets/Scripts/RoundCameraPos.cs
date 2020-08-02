using UnityEngine;
using Cinemachine;

public class RoundCameraPos : CinemachineExtension
{
    public float pixelsPerUnit = 32;
    
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var finalPos = state.FinalPosition;
            var newPos = new Vector3(Round(finalPos.x), Round(finalPos.y), finalPos.z);

            state.PositionCorrection += newPos - finalPos;
        }
    }

    private float Round(float x)
    {
        return Mathf.Round(x * pixelsPerUnit) / pixelsPerUnit;
    }
}