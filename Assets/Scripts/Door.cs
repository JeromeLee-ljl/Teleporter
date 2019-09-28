using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Need config")] public RenderTexture renderTex;
    public Door connectedDoor;

    [Header("Default")] public Camera doorCamera;
    public MeshRenderer meshRender;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;

        doorCamera.targetTexture = renderTex;
        meshRender.material.mainTexture = renderTex;
    }

    private void LateUpdate()
    {
        //更新camera到对应门的的位置
        doorCamera.transform.position = GetPositionInConnectedDoor(_mainCamera.transform.position);
        doorCamera.transform.rotation = GetRotationInConnectedDoor(_mainCamera.transform.rotation);

        // 设置近裁剪平面
        Transform center = connectedDoor.transform;
        Vector4 clipPlane = -center.forward;
        clipPlane.w = -Vector3.Dot(clipPlane, center.position);
        clipPlane = Matrix4x4.Transpose(Matrix4x4.Inverse(doorCamera.worldToCameraMatrix)) * clipPlane;

        // 使用mainCamera 可以将整个画面渲染到方形rendererTextrue中
        doorCamera.projectionMatrix = _mainCamera.CalculateObliqueMatrix(clipPlane);
    }

    ///pos进入门后在连接的门的位置
    private Vector3 GetPositionInConnectedDoor(Vector3 pos)
    {
        Vector3 localPos = transform.InverseTransformPoint(pos);
        localPos.x = -localPos.x;
        localPos.z = -localPos.z;
        return connectedDoor.transform.TransformPoint(localPos);
    }
    ///dir进入门后在连接的门的方向
    private Vector3 GetVectorInConnectedDoor(Vector3 dir)
    {
        Vector3 localPos = transform.InverseTransformVector(dir);
        localPos.x = -localPos.x;
        localPos.z = -localPos.z;
        return connectedDoor.transform.TransformVector(localPos);
    }

    ///rotation进入门后在连接的门的旋转
    private Quaternion GetRotationInConnectedDoor(Quaternion rotation)
    {
        Quaternion localRotation = Quaternion.Inverse(transform.rotation) * rotation;
        localRotation = Quaternion.Euler(0, 180, 0) * localRotation;
        return connectedDoor.transform.rotation * localRotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller == null) return;
            if (controller.Rbody.velocity.magnitude < 0.0001f) return; // 停止
            if (Vector3.Angle(transform.forward, controller.Rbody.velocity) > 90) return;
            Vector3 clipPos = _mainCamera.transform.position + _mainCamera.transform.forward * 0.05f;
            if (transform.InverseTransformPoint(clipPos).z < 0) return;
            controller.transform.position = GetPositionInConnectedDoor(controller.transform.position);
            controller.transform.rotation = GetRotationInConnectedDoor(controller.transform.rotation);
            controller.Rbody.velocity = GetVectorInConnectedDoor(controller.Rbody.velocity);
            Debug.Log(controller.Rbody.velocity);
        }
    }
}