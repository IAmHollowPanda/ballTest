using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject[] balls;
    public Vector3 offset;
    
    [SerializeField]
    private int currentTarget;

    private float yawSpeed = 100f;
    private float yawInput = 0f;


    void Start() {
        currentTarget = 0;
    }
    
    void Update() {
        if(Input.GetMouseButtonDown(0)){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                var selection = hit.transform;
                var selectedBall = selection.GetComponent<BallBehaviour>();

                if(selectedBall != null){

                    if(Time.time - selectedBall.firstClickTime < 0.2f){
                        selectedBall.transform.position = new Vector3(selectedBall.getX(0), selectedBall.getY(0), selectedBall.getZ(0));
                        selectedBall.SetMove(false);
                        selectedBall.ResetLine();
                        selectedBall.SetCurrentPosition(0);
                    }else{
                        if(selectedBall.getCurrentPosition() != selectedBall.getPoints())
                            selectedBall.SetMove(true);
                    }
                    selectedBall.firstClickTime = Time.time;

                    currentTarget = selectedBall.ballNumber;
                    for(int i=0;i<balls.Length;i++){
                        balls[i].GetComponent<BallBehaviour>().SetMove(false);
                    }
                    balls[currentTarget].GetComponent<BallBehaviour>().SetMove(true);
                }
            }
        }

        yawInput -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        transform.position = balls[currentTarget].transform.position - offset;
        transform.LookAt(balls[currentTarget].transform.position + Vector3.up);  

        transform.RotateAround(balls[currentTarget].transform.position, Vector3.up, yawInput);
    }

    public void ChangeTargettoNext(){

        for(int i=0;i<balls.Length;i++){
            balls[i].GetComponent<BallBehaviour>().SetMove(false);
        }

        currentTarget = (currentTarget + 1); 
        currentTarget = Mathf.Clamp(currentTarget, 0, balls.Length-1);
        balls[currentTarget].GetComponent<BallBehaviour>().SetMove(true);

    }

    public void ChangeTargettoPrev(){
        for(int i=0;i<balls.Length;i++){
            balls[i].GetComponent<BallBehaviour>().SetMove(false);
        }
        currentTarget = (currentTarget - 1); 
        currentTarget = Mathf.Clamp(currentTarget, 0, balls.Length-1);
        balls[currentTarget].GetComponent<BallBehaviour>().SetMove(true);

    }
}
