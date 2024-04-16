using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Controller : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private Animator anim;
    [SerializeField] private Transform cashedTransform;

    [Header("Settings")]

    [SerializeField] private float movingTime;

    private Vector3 destinyPoint;
    private Entity_Creator creator;
    private float yPos = -0.52f;

    public void SetDestinyPoint(Vector3 point)
    {
        destinyPoint = point;

        StartCoroutine(MoveToPoint());
    }

    public void SetCreator(Entity_Creator cr)
    {
        creator = cr;
    }

    private IEnumerator MoveToPoint()
    {
        float currentTime = 0f;
        bool isMoving = true;
        Vector3 startPoint = cashedTransform.localPosition;
        Vector3 targetPoint = new Vector3(destinyPoint.x, yPos, destinyPoint.y - 2.5f);

        while (isMoving)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(currentTime / movingTime);
            cashedTransform.localPosition = Vector3.Lerp(startPoint, targetPoint, normalizedTime);

            if (currentTime >= movingTime)
                isMoving = false;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Trap"))
        {
            cashedTransform.SetParent(null);
            anim.SetBool("IsDead", true);
            creator.Entity_Destroyed(this);
            Destroy(gameObject, 5);
        }

        else if (other.gameObject.CompareTag("Plus_Entity"))
        {
            creator.CreateNewEntity(cashedTransform.localPosition);
            Destroy(other.gameObject);
        }
    }
}
