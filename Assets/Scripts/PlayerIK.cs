using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public Animator a;
    public Transform right, left, gaze;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorIK() {
        a.SetLookAtWeight(1);
        a.SetLookAtPosition(gaze.position);

        a.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
        a.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
        a.SetIKPosition(AvatarIKGoal.RightHand,right.position);
        a.SetIKRotation(AvatarIKGoal.RightHand,right.rotation);
        a.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
        a.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);  
        a.SetIKPosition(AvatarIKGoal.LeftHand,left.position);
        a.SetIKRotation(AvatarIKGoal.LeftHand,left.rotation);
    }
}
