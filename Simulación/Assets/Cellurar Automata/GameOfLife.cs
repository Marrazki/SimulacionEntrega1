using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class GOL : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool[,] gridActual;
    bool[,] gridExpected;

    GameObject[,] gridObjects;


    public int ancho = 20;
    public int alto = 20;

    public float espacioEntreCubos = 1.1f;
    public float life = 0.6f;
    public float updateInterval = 0.2f;
    private float timer = 0;
    void Start()
    {
        GenerarGrid();
        RefrescarGrid();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            Simulation();
            timer = 0;
        }
    }
    void GenerarGrid()
    {
        gridActual = new bool[alto, ancho];
        gridExpected = new bool[alto, ancho];

        gridObjects = new GameObject[alto, ancho];

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                GameObject nuevoCubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                nuevoCubo.transform.position = new Vector3(i * espacioEntreCubos, 0, j * espacioEntreCubos);
                gridObjects[i, j] = nuevoCubo;

                float random = Random.value;
                if (random < life)
                {
                    gridActual[i, j] = true;
                }
                else
                {
                    gridActual[i, j] = false;
                }

            }
        }
    }
    void Simulation()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                int vecinosVivos = CuentaVecinosVivos(i, j);
                bool vivo = gridActual[i, j];

                //if (vivo)
                //{
                //    gridExpected[i, j] = (vecinosVivos == 2 || vecinosVivos == 3);
                //}
                //else
                //{
                //    gridExpected[i, j] = (vecinosVivos == 3);
                //}

                if (vecinosVivos < 2)
                {
                    gridExpected[i, j] = false;
                }
                else if (vecinosVivos == 3)
                {
                    gridExpected[i, j] = true;
                }
                else if (vecinosVivos >= 4)
                {
                    gridExpected[i, j] = false;
                }
                else
                {
                    gridExpected[i, j] = gridActual[i,j];
                }
            }
        }
        System.Array.Copy(gridExpected, gridActual, gridActual.Length);
        //gridActual = gridExpected;
        RefrescarGrid();
    }
    void RefrescarGrid()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                MeshRenderer actualRender = gridObjects[i, j].GetComponent<MeshRenderer>();
                if (gridActual[i, j])
                {
                    actualRender.material.color = Color.green;
                }
                else
                {
                    actualRender.material.color = Color.red;
                }
            }
        }
    }
    int CuentaVecinosVivos(int cuboX, int cuboY)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                int vecinoX = cuboX + i;
                int vecinoY = cuboY + j;
                if (vecinoX >= 0 && vecinoX < ancho && vecinoY >= 0 && vecinoY < alto)
                {
                    if (gridActual[vecinoY, vecinoX])
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }
}