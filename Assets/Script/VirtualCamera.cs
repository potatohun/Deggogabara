using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineConfiner2D confiner;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();

        // ĸƾ ī�ǹٶ� ����
        GameObject captain = GameObject.FindGameObjectWithTag("Player");
        virtualCamera.Follow = captain.transform;

        // �ǴϽö��� ����
        GameObject finishLine = GameObject.FindGameObjectWithTag("FinishLine");
        Debug.Log(finishLine.GetComponent<CompositeCollider2D>());
        confiner.m_BoundingShape2D = finishLine.GetComponent<PolygonCollider2D>();
    }
}
