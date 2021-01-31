using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallBehaviour : MonoBehaviour
{
    private LineRenderer line;

    Camera cam;

    public int ballNumber;

    public TextAsset movementTrajectoryForBall1;
    private float[] pointX; private float[] pointY; private float[] pointZ;
    public float speed;
    private int currentPosition = 1;

    public float firstClickTime = 0f;

    [SerializeField]
    private bool isMoving = false;

    public float getX(int index){
        return pointX[index];
    }
    public float getY(int index){
        return pointY[index];
    }
    public float getZ(int index){
        return pointZ[index];
    }
    public void SetMove(bool move){
        isMoving = move;
    }
    public void SetCurrentPosition(int pos){
        currentPosition = pos;
    }
    public int getCurrentPosition(){
        return currentPosition;
    }
    public void ResetLine(){
        line.positionCount = 0;
    }
    public int getPoints(){
        return pointX.Length;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        BallTrajectoryPositions ballWay = JsonUtility.FromJson<BallTrajectoryPositions>(movementTrajectoryForBall1.text);

        pointX = ballWay.x;
        pointY = ballWay.y;
        pointZ = ballWay.z;

        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isMoving == true){
            Vector3 nextPoint = new Vector3(pointX[currentPosition],
                                            pointY[currentPosition],
                                            pointZ[currentPosition]);

            if(Vector3.Distance(transform.position, nextPoint) > 0.001f){         
                transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
            }else{
                line.positionCount++;
                if(currentPosition!=0)
                    line.SetPosition(line.positionCount - 1, transform.position);

                currentPosition += 1;
                
                if(currentPosition==pointX.Length){
                    isMoving = false;
                    currentPosition = 0;
                }
                
            }
        }        
    }

    public void ChangeSpeed(float newSpeed){
        speed = newSpeed*100;
    }

    private class BallTrajectoryPositions{
        public float[] x;
        public float[] y;
        public float[] z;
    }
}
