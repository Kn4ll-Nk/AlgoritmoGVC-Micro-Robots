using System;
using System.IO;

class GeneradorAleatorio {
    // Arreglo que contiene todas las casillas que componen un tablero.
    private string[] casillas;
    private int tamTablero;

    public GeneradorAleatorio(int tamTablero) {
        this.tamTablero = tamTablero;
        casillas = new string[tamTablero*tamTablero];
    }

    //Genera las casillas que conformarán el tablero, dependiendo del tamaño asignado.
    private void GenerarCasillas() {
        string[] letra = {"R", "G", "B", "W", "Y", "P"};
        if (tamTablero == 2) {
            for (int i = 0, j = 0; i < tamTablero*tamTablero; i ++, j ++)
                casillas[i] = "1" + letra[j];
        }
        else {
            for (int i = 0, j = 1, k = 0; i < tamTablero*tamTablero; i ++, j ++) {
                if (j > tamTablero) {
                    j = 1; 
                    k++;
                }
                casillas[i] = (j).ToString() + letra[k];
            }
        }
    }

    //Se encarga de reordenar las casillas contenidas en el arreglo, de forma aleatoria. 
    private void AleatorizarCasillas() {
        Random rng = new Random();
        int n = casillas.GetLength(0);

        while (n > 1) {
            int k = rng.Next(n--);
            string aux = casillas[n];
            casillas[n] = casillas[k];
            casillas[k] = aux;
        }
    }

    //Los datos del arreglo de casillas los escribe en un documento .txt en forma de matriz.
    private void CrearDocumento(string nombreArchivo) {
        StreamWriter sw = new StreamWriter(nombreArchivo);
        int k = 0;

        for (int i = 0; i < tamTablero; i++){
            for (int j = 0; j < tamTablero; j++) {
                sw.Write(casillas[k] + " ");
                k++;
            }
            sw.WriteLine();
        }
        sw.Close();
    }

    //Función principal que ejecuta otros métodos.
    public void GenerarTablero(string nombreArchivo) {
        GenerarCasillas();
        AleatorizarCasillas();
        CrearDocumento(nombreArchivo);
    }
}