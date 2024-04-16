using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw_Controller : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private Entity_Creator creator;
    [SerializeField] private SplineComputer splineComputer;
    [SerializeField] private MeshRenderer splineRenderer;
    [SerializeField] private SplineRenderer splRenderer;
    [SerializeField] private Image drawingField;

    [Header("Settings")]

    [SerializeField] private float minDistance = 0.1f;
    [SerializeField] private bool canDraw;

    [SerializeField] private Vector3[] positions;

    private int index;
    private Vector3 previousPosition;
    private List<Vector3> invertedPositions = new List<Vector3>();

    private void Start()
    {
        previousPosition = transform.position;
        index = 0;
    }

    private void OnMouseEnter()
    {
        canDraw = true;
    }

    private void OnMouseDown()
    {
        StartDraw();
    }

    private void OnMouseExit()
    {
        canDraw = false;
    }

    private void ResetDrawing()
    {
        index = 0;
        invertedPositions.Clear();
        SplinePoint[] newPoints = new SplinePoint[0];
        splineComputer.SetPoints(newPoints);
    }

    private void StartDraw()
    {
        ResetDrawing();
        splineComputer.SetPoint(index, new SplinePoint(GetMousePosition()));
        index++;
        ChangeSplineRendererState(true);

        StartCoroutine(Draw());
    }

    private void ChangeSplineRendererState(bool state)
    {
        splRenderer.enabled = state;
        splineRenderer.enabled = state;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 screenPosDepth = Input.mousePosition;
        screenPosDepth.z = 5;
        Vector3 position = Camera.main.ScreenToWorldPoint(screenPosDepth);

        return position;
    }

    private Vector3 GetPointRelativeToScreen()
    {
        Vector3 screenPosDepth = Input.mousePosition;
        screenPosDepth.z = 5;
        Vector3 position = new Vector3();

        float width = Screen.width;
        float height = Screen.height;

        int xOldMin = -2;
        int xOldMax = 2;
        float newMin = (width - drawingField.rectTransform.rect.width) / 2;
        float newMax = newMin + drawingField.rectTransform.rect.width;
        float newMinY = drawingField.rectTransform.anchoredPosition.y - (drawingField.rectTransform.rect.height / 2);
        float newMaxY = newMinY + drawingField.rectTransform.rect.height;

        Debug.Log(newMax);

        position.x = (screenPosDepth.x - newMin) * (xOldMax - xOldMin) / (newMax - newMin) + xOldMin;
        position.y = (screenPosDepth.y - newMinY) * (xOldMax - xOldMin) / (newMaxY - newMinY) + xOldMin;

        return position;
    }

    private void StopDraw()
    {
        positions = new Vector3[invertedPositions.Count - 1];

        for (int i = 0; i < invertedPositions.Count - 1; i++)
        {
            positions[i] = invertedPositions[i];
        }

        creator.MovePoints(positions);

        ChangeSplineRendererState(false);
    }

    private IEnumerator Draw()
    {

        while (canDraw && Input.GetMouseButton(0))
        {
            Vector3 currentPosition = GetMousePosition();

            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {

                if (previousPosition == transform.position)
                {
                    splineComputer.SetPoint(index, new SplinePoint(currentPosition));
                    invertedPositions.Add(GetPointRelativeToScreen());
                    index++;
                }


                else
                {
                    splineComputer.SetPoint(index, new SplinePoint(currentPosition));
                    invertedPositions.Add(GetPointRelativeToScreen());
                    index++;
                }

                previousPosition = currentPosition;

                yield return null;
            }

            yield return null;
        }


        StopDraw();
        yield return null;
    }
}
