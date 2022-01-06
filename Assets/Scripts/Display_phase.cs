using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Display_phase : MonoBehaviour
{
    // Start is called before the first frame update
    public QubitState qubit1;
    public float phase = 0;
    public TextMeshProUGUI qubitStateDisplay;
    void Start()
    {
        qubitStateDisplay.text = "Phase : 0";

    }

    // Update is called once per frame
    void Update()
    {
        qubit1 = FindObjectOfType<QubitState>();

        phase = qubit1.qubitPhase;


        phase = phase / Mathf.PI;

        qubitStateDisplay.text = "Phase : " + phase.ToString("0.00") + "π";

    }
}

