using System;

class GenerarCombinaciones {

    Validador validadorTablero;
    private string[] letra = {"R", "G", "B", "W", "Y", "P"};
    private string[] casillas;
    private Random rng;
    private int tamArreglo;
    private int tamTablero;

    //Constructor.
    public GenerarCombinaciones(int dimension) {
        validadorTablero = new Validador();

        this.tamTablero = dimension;
        rng = new Random();
        tamArreglo = dimension*dimension;
        casillas = new string[tamArreglo];

        GenerarCasillas(dimension);
    }

    //Genera las casillas que conformarán el tablero, dependiendo del tamaño asignado.
    private void GenerarCasillas(int tamTablero) {
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

    //Genera un documeto .txt, que contiene el tablero con las casillas generadas en formato de matriz.
    private void CrearDocumento(string nombreArchivo, string[] casillas) {
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

    //Llama a la función de validar el tablero.
    private void Validar(string nombreArchivo, string[] casillas, string tipoValidacion, int contador) {
        bool valido = false;
        
        if (tipoValidacion == "0") valido = validadorTablero.ValidarTableroCompleto(nombreArchivo);
        if (tipoValidacion == "1") valido = validadorTablero.ValidarTableroActual(nombreArchivo);

        if (valido) {      //Si el tablero es válido. 
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss:ffff");
            File.Copy("Tablero.txt", "Tableros4x4/" + date + " " + time + ".txt", true);    //Se copia el archivo del tablero con otro nombre.
        }
        Console.Clear();
        Console.WriteLine("Ejecutando... Generación y validación de tableros... Iteración : " + contador);
        Console.WriteLine("El programa se detendrá cuando finalice de permutar casillas.");  

        return;
    }

    //Permuta dos posiciones en el arreglo.
    private void Permutar(string[] casillas, int i, int j) {
        
        string temp = casillas[i];
        casillas[i] = casillas[j];
        casillas[j] = temp;
    }

    //Función recursiva que explora todas las permutaciones entre elementos posibles.
    //Funciona de forma iterativa.
    private void Combinaciones(string[] casillas, string tipoValidacion) {
        string nombreArchivo = "Tablero.txt";
        int contador = 0;

        int[] indices = new int[tamArreglo];
        for (int i = 0; i < tamArreglo; i++) {
            indices[i] = 0;
        }

        contador++;
        CrearDocumento(nombreArchivo, casillas);
        Validar(nombreArchivo, casillas, tipoValidacion, contador);

        int pos = 0;
        while (pos < tamArreglo) {
            if (indices[pos] < pos) {
                if (pos % 2 == 0) {
                    Permutar(casillas, 0, pos);
                } else {
                    Permutar(casillas, indices[pos], pos);
                }
                
                contador++;
                CrearDocumento(nombreArchivo, casillas);
                Validar(nombreArchivo, casillas, tipoValidacion, contador);
                
                indices[pos]++;
                pos = 0;
            } else {
                indices[pos] = 0;
                pos++;
            }
        }
    }


    //Método utilizado para llamar y ejecutar el resto de funciones.
    public void GenerarTablero(string tipoValidacion) {
        Combinaciones(casillas, tipoValidacion);
    }

}