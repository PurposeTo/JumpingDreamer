using UnityEngine;

public class TrainingTipContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject trainingTextObject;

    [SerializeField]
    private GameObject trainingTip;


    public GameObject GetTrainingTextObject() => trainingTextObject;
    public GameObject GetTrainingTipObject() => trainingTip;
}
