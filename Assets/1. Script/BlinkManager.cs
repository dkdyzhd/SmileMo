using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlinkManager : MonoBehaviour
{
    public LoopType loopType;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.DOFade(0.0f, 0.65f).SetLoops(-1, loopType);
    }
}
