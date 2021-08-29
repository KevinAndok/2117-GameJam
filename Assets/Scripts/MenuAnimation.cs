using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuAnimation : MonoBehaviour
{
    public Transform pointsParent;
    private List<Transform> points = new List<Transform>();
    private bool clickedPlay = false;
    private Tween currentTween;
    private Coroutine animCorot;
    Rigidbody2D newCube; // the rigidbody cube he's about to throw

    [Header("Ball-E Things")]
    public Transform ballERoot;
    public Transform ballEThrowStart;
    public Vector2 throwForce;
    public Transform armJoint;
    public Transform neckJoint;
    public Transform palm;

    [Header("Walking Points")]
    public Transform cubeMaking;
    public Transform cubeThrowing;
    public Transform ballPickupPoint;
    public Transform endPos;

    [Header("Props and thier prefabs")]
    public Rigidbody2D cubeRbPrefab;
    public SpriteRenderer cubeProp;
    public Transform ballOnFloor;

    public float walkWaitMin;
    public float walkWaitMax;
    public float walkSpeed;

    // rotating stuff
    private bool isRotatingArmDown = false;
    private bool isRotatingArmUp = false;
    Vector3 initialRotation = Vector3.zero;
    Vector3 initialNeckRotation = Vector3.zero;
    float initialTime = 0f;
    float initialNeckTime = 0f;
    bool isStaringAtBall = false;
    bool isLookingAtEnd = false;

    private void Start()
    {
        foreach (Transform tr in pointsParent) {
            points.Add(tr);
        }

        animCorot = StartCoroutine(WorkCycle());
    }

    private void Update()
    {
        if (isStaringAtBall)
        {
            neckJoint.right = ballOnFloor.position - neckJoint.position;
        }


        if (isRotatingArmDown)
        {
            armJoint.right = Vector3.Lerp(initialRotation, armJoint.position - ballOnFloor.position, 2 * (Time.time - initialTime));
        }

        if (isLookingAtEnd)
        {
            neckJoint.right = Vector3.Lerp(initialNeckRotation, neckJoint.position - neckJoint.position + Vector3.right, 1 / 0.2f * (Time.time - initialNeckTime));
        }

        if (isRotatingArmUp)
        {
            armJoint.right = Vector3.Lerp(initialRotation, Vector3.down, 2 * (Time.time - initialTime));
        }
    }

    private IEnumerator WorkCycle()
    {
        while (true)
        {
            // cube making -> throwing
            currentTween = ballERoot.transform.DOMove(cubeThrowing.position, 1.2f);
            
            yield return new WaitForSeconds(1.5f);
            newCube = Object.Instantiate(cubeRbPrefab, ballEThrowStart.position, Quaternion.identity, null);
            newCube.velocity = new Vector2(
                throwForce.x + Random.Range(-1f, 1f),
                throwForce.y + Random.Range(-1f, 1f));
            newCube = null;
            cubeProp.enabled = false;

            // throwing -> cube making
            yield return new WaitForSeconds(0.4f);
            ballERoot.Rotate(ballERoot.up, 180f);
            currentTween = ballERoot.transform.DOMove(cubeMaking.position, 1.2f);
            yield return new WaitForSeconds(2.4f);
            ballERoot.Rotate(ballERoot.up, 180f);
            cubeProp.enabled = true;
        }
    }

    private IEnumerator GoPlay()
    {
        yield return new WaitForSeconds(1.7f);

        if (cubeProp.enabled == true)
        {
            newCube = Object.Instantiate(cubeRbPrefab, ballEThrowStart.position, Quaternion.identity, null);
            cubeProp.enabled = false;
            yield return new WaitForSeconds(0.4f);
            ballERoot.Rotate(ballERoot.up, 180f);
            yield return new WaitForSeconds(0.2f);
        }

        isStaringAtBall = true;
        float dist = Vector2.Distance(ballPickupPoint.position, ballERoot.position);
        ballERoot.DOMove(ballPickupPoint.position, dist / 3.8f);
        yield return new WaitForSeconds(1.2f);

        initialRotation = armJoint.right;
        initialTime = Time.time;

        isRotatingArmDown = true;

        yield return new WaitForSeconds(0.5f);

        isRotatingArmDown = false;
        isStaringAtBall = false;

        yield return new WaitForSeconds(0.2f);

        ballOnFloor.parent = palm;

        isLookingAtEnd = true;
        initialNeckTime = Time.time;
        initialNeckRotation = neckJoint.transform.right;

        yield return new WaitForSeconds(0.1f);

        isRotatingArmUp = true;
        initialRotation = armJoint.right;
        initialTime = Time.time;

        yield return new WaitForSeconds(0.7f);

        ballERoot.DOMove(endPos.position, 1f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(1.3f);

        float f = 0f;
        DOTween.To(() => f, x => f = x, 1f, 0.8f).OnUpdate(() => Fades.fadeOutCounter = f).OnComplete(() =>
            SceneManager.LoadScene(1)
        );

        

    }

    // stop work cycle and go play
    public void OnClickedPlay()
    {
        if (clickedPlay) return;
        clickedPlay = true;

        StopCoroutine(animCorot);
        currentTween.Pause();

        ballERoot.DOJump(ballERoot.position, 0.3f, 3, 1.5f).SetEase(Ease.Linear);

        StartCoroutine(GoPlay());
    }

}

/*bool canContinue = true;

        while (true)
        {
            yield return new WaitUntil(() => { return canContinue == true; });
            canContinue = false;

            yield return new WaitForSeconds(Random.RandomRange(walkWaitMin, walkWaitMax));

            Vector2 target = points[Random.Range(0, points.Count - 1)].position;
            float time = Vector2.Distance(target, transform.position) / walkSpeed;
            transform.DOMove(target, time).OnComplete(() => {
                canContinue = true; 
            });
        }*/
