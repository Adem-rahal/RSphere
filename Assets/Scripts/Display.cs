
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Display : MonoBehaviour
{
    // Start is called before the first frame update
    public QubitState qubit1;
    public Complex[] state;
    public TextMeshProUGUI qubitStateDisplay;
    void Start()
    {
        qubitStateDisplay.text = "Qubit State : 1 |0> + 0 |1>";

    }

    // Update is called once per frame
    void Update()
    {
        qubit1 = FindObjectOfType<QubitState>();

        state = qubit1.qubit_vector_state;

        ///////////////////////////////_FORMATING_///////////////////////////////////

        float zero_i = (float)state[0].Imaginary;
        float zero_r = (float)state[0].Real;

        float one_i = (float)state[1].Imaginary;
        float one_r = (float)state[1].Real;

        string[] s = new string[2];

        if (zero_i <= 0.0000001)
        {
            s[0] = zero_r.ToString("0.00");
        }
        else if (zero_r <= 0.0000001)
        {
            s[0] = zero_i.ToString("0.00") + "i";
        }
        else
        {
            s[0] = "(" + zero_r.ToString("0.0") + "+" + zero_i.ToString("0.0") + "i" + ")";
        }

        if (one_i <= 0.0000001)
        {
            s[1] = one_r.ToString("0.00");
        }
        else if (one_r <= 0.0000001)
        {
            s[1] = one_i.ToString("0.00") + "i";
        }
        else
        {
            s[1] = "(" + one_r.ToString("0.0") + "+" + one_i.ToString("0.0") + "i" + ")";
        }

        ///////////////////////////////_END_FORMATING_///////////////////////////////////

        qubitStateDisplay.text = "Qubit State : " + s[0] + " |0> + " + s[1] + " |1>";

    }
}
