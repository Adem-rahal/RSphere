using System.Collections;
using UnityEngine.VFX;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using Accord.Math;
using System;

//////////////////////////////_COMPLEX_MATRIX_OPERATIONS_DEF_//////////////////////////////////////////////////////
public static class ComplexMatrix
{
    public static Complex[] Multiply(this Complex[] a, Complex[] b)
    {
        if (a == null)
            throw new ArgumentNullException("a");
        if (b == null)
            throw new ArgumentNullException("b");

        Complex[] r = new Complex[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            r[i] = Complex.Multiply(a[i], b[i]);
        }
        return r;
    }

    public static Complex[] Multiply_Matrix_Vector(this Complex[,] a, Complex[] b)
    {
        if (a == null)
            throw new ArgumentNullException("a");
        if (b == null)
            throw new ArgumentNullException("b");

        Complex[] r = new Complex[b.Length];

        for (int i = 0; i < b.Length; i++)
        {
            r[i] = Complex.Add(Complex.Multiply(a[i, 0], b[0]), Complex.Multiply(a[i, 1], b[1]));
        }
        return r;
    }

    public static double[] Phase(this Complex[] c)
    {
        return c.Apply((x, i) => x.Phase);
    }

    public static double[,] Phase(this Complex[,] c)
    {
        return c.Apply((x, i, j) => x.Phase);
    }

    public static double[][] Phase(this Complex[][] c)
    {
        return c.Apply((x, i, j) => x.Phase);
    }

}

/////////////////COMPLEX_MATRIX_OPERATIONS_DEF_END_////////////////////////////////

public class QubitState : MonoBehaviour
{
    private Complex one_c = new Complex(1, 0);
    private Complex zero_c = new Complex(0, 0);
    public Complex[] qubit_vector_state = new Complex[2];
    public Complex[,] gate_matrix = new Complex[2, 2];

    [SerializeField]
    private VisualEffect visualEffect;

    [SerializeField]
    private float arcAngle = 6.28f;

    [SerializeField]
    private float collapseSphere = 1;

    [SerializeField]
    private float sphereSize = 3;

    [SerializeField]
    public Color qubitColor;
    public float qubitState = 0;
    public float qubitPhase = 0;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;


    void Start()
    {
        qubit_vector_state[0] = one_c;
        qubit_vector_state[1] = zero_c;  //Put the qubit in the state |psi> = |0> 
        UpdateQubit();
    }

    void Update()
    {
    }

    void UpdateQubit()
    {
        float real = (float)qubit_vector_state[1].Real;
        float imaginary = (float)qubit_vector_state[1].Imaginary;

        // Note: If z is a complex number then : z = x +iy, z^2 = x^2+y^2 => |z| = (x^2 + y^2)^(1/2)
        // With, ^: power

        qubitState = (float)Math.Sqrt((float)Math.Pow(real, 2) + (float)Math.Pow(imaginary, 2));

        //The Measurement rules implies p(1) = z^2

        qubitState = (float)Math.Pow(qubitState, 2);
        arcAngle = 6.28f * qubitState;

        double[] x = ComplexMatrix.Phase(qubit_vector_state);

        Debug.Log(qubit_vector_state[0]);
        Debug.Log(qubit_vector_state[1]);

        qubitPhase = Math.Abs((float)x[1]) - Math.Abs((float)x[0]); // compute the relative phase  |phi| = ||arg(B)| - |arg(A)|| for |psi> = A*|0> + B*|1>
        qubitPhase = Math.Abs(qubitPhase);

        if (arcAngle <= 0.0000001)             //Correct rounding issues
        {
            arcAngle = 6.28f;
            sphereSize = 0;
            collapseSphere = 1;
            qubitState = 0;
        }

        else
        {
            sphereSize = 3;
            collapseSphere = 3;
        }

        visualEffect.SetFloat("CollapseSphere", collapseSphere);
        visualEffect.SetFloat("SphereSize", sphereSize);
        visualEffect.SetFloat("ArcAngle", arcAngle);
    }
    void ApplyGate(Complex[,] _gate_matrix, Complex[] _qubit_vector_state) //Apply the quantum gate
    {
        qubit_vector_state = ComplexMatrix.Multiply_Matrix_Vector(_gate_matrix, _qubit_vector_state);
        UpdateQubit();
    }

    public void Measurement()
    {
        float rnd = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(rnd);
        qubitPhase = 0;
        arcAngle = 6.28f;

        if (rnd >= qubitState)
        {
            qubitState = 0;
            StartCoroutine(collapseAnimationZero());
            collapseSphere = 1;
            qubit_vector_state[0] = one_c;
            qubit_vector_state[1] = zero_c;  //Put the qubit in the state |psi> = |0> 
        }
        else
        {
            qubitState = 1;
            StartCoroutine(collapseAnimationOne());
            collapseSphere = 3;
            qubit_vector_state[0] = zero_c;
            qubit_vector_state[1] = one_c;  //Put the qubit in the state |psi> = |1> 
        }

        float intensity = 10;
        qubitColor = color1;
        visualEffect.SetFloat("CollapseSphere", collapseSphere);
        visualEffect.SetVector4("QubitColor", qubitColor * intensity);

    }

    void UpdateColor()
    {
        if (qubitPhase < Mathf.PI / 2)
        {
            float x = qubitPhase / (Mathf.PI / 2);
            qubitColor = Color.Lerp(color1, color2, x);
        }
        else if (qubitPhase < Mathf.PI)
        {
            float x = (float)(qubitPhase - (Math.PI / 2)) / (Mathf.PI / 2);
            qubitColor = Color.Lerp(color2, color3, x);
        }
        else if (qubitPhase < 3 * Mathf.PI / 2)
        {
            float x = (float)(qubitPhase - (Math.PI)) / (float)((3 * Mathf.PI / 2) - Math.PI);
            qubitColor = Color.Lerp(color3, color4, x);
        }
        else
        {
            float x = (float)(qubitPhase - (3 * Mathf.PI / 2)) / (float)((2 * Mathf.PI) - (3 * Mathf.PI / 2));
            qubitColor = Color.Lerp(color4, color1, x);
        }

        float intensity = 10;
        visualEffect.SetVector4("QubitColor", qubitColor * intensity);
    }

    ///////////////////////////////////_GATE DEFINITIONS_////////////////////////////////////////////////
    public void HGate()
    {
        Complex h = new Complex(1 / Math.Sqrt(2), 0);

        gate_matrix[0, 0] = h;
        gate_matrix[0, 1] = h;
        gate_matrix[1, 0] = h;
        gate_matrix[1, 1] = -h; //                                       (1  1)
                                //Create the Hadamard gate H = 1/sqrt(2)*(1 -1)

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }
    public void RZGate()
    {
        Complex phi = new Complex(0, Math.PI / 2);

        gate_matrix[0, 0] = one_c;
        gate_matrix[0, 1] = zero_c;
        gate_matrix[1, 0] = zero_c;
        gate_matrix[1, 1] = Complex.Exp(phi); //                  (1        0    )
                                              //             RZ = (0  exp(i*pi/2))

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    public void XGate()
    {
        gate_matrix[0, 0] = zero_c;
        gate_matrix[0, 1] = one_c;
        gate_matrix[1, 0] = one_c;
        gate_matrix[1, 1] = zero_c;            //           (0  1)
                                               //       X = (1  0)

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    public void YGate()
    {
        Complex i = new Complex(0, 1);

        gate_matrix[0, 0] = zero_c;
        gate_matrix[0, 1] = -i;
        gate_matrix[1, 0] = i;
        gate_matrix[1, 1] = zero_c;            //          (0  -i)
                                               //      Y = (i   0)

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }
    public void ZGate()
    {

        gate_matrix[0, 0] = one_c;
        gate_matrix[0, 1] = zero_c;
        gate_matrix[1, 0] = zero_c;
        gate_matrix[1, 1] = -one_c;            //               (1   0)
                                               //            Z =(0  -1)

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    public void SGate()
    {
        Complex i = new Complex(0, 1);

        gate_matrix[0, 0] = one_c;
        gate_matrix[0, 1] = zero_c;
        gate_matrix[1, 0] = zero_c;
        gate_matrix[1, 1] = i;            //              (1  0)
                                          //           S =(0  i)

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    public void CustomGate1()
    {
        Complex phi = new Complex(0, Math.PI / 2);  // phi = i*pi/2
        Complex i = new Complex(0, 1);

        gate_matrix[0, 0] = one_c;
        gate_matrix[0, 1] = zero_c;
        gate_matrix[1, 0] = zero_c;
        gate_matrix[1, 1] = one_c;

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    public void CustomGate2()
    {
        Complex phi = new Complex(0, Math.PI / 2);
        Complex i = new Complex(0, 1);

        gate_matrix[0, 0] = one_c;
        gate_matrix[0, 1] = zero_c;
        gate_matrix[1, 0] = zero_c;
        gate_matrix[1, 1] = one_c;

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    public void CustomGate3()
    {
        Complex phi = new Complex(0, Math.PI / 2);
        Complex i = new Complex(0, 1);

        gate_matrix[0, 0] = one_c;
        gate_matrix[0, 1] = zero_c;
        gate_matrix[1, 0] = zero_c;
        gate_matrix[1, 1] = one_c;

        ApplyGate(gate_matrix, qubit_vector_state);
        UpdateColor();
    }

    ///////////////////////////////////_END_GATE DEFINITIONS_////////////////////////////////////////////
    ///////////////////////////////////_ANIMATIONS_////////////////////////////////////////////
    IEnumerator collapseAnimationOne()
    {
        sphereSize = 1;
        visualEffect.SetFloat("SphereSize", sphereSize);
        visualEffect.SetFloat("ArcAngle", arcAngle);
        yield return new WaitForSeconds(0.2f);
        sphereSize = 3;
        visualEffect.SetFloat("SphereSize", sphereSize);
    }

    IEnumerator collapseAnimationZero()
    {
        sphereSize = 1;
        visualEffect.SetFloat("SphereSize", sphereSize);
        visualEffect.SetFloat("ArcAngle", arcAngle);
        yield return new WaitForSeconds(0.2f);
        sphereSize = 0;
        visualEffect.SetFloat("SphereSize", sphereSize);
    }

}
