using System;

class GenerarCombinaciones {

    Validador validadorTablero;
    private string[] letra = {"R", "G", "B", "W", "Y", "P"};
    private string[] casillas;
    private Random rng;
    private int n;
    private int tamTablero;

    //Constructor.
    public GenerarCombinaciones(int tamTablero) {
        validadorTablero = new Validador();

        this.tamTablero = tamTablero;
        casillas = new string[tamTablero*tamTablero];
        rng = new Random();
        n = tamTablero*tamTablero;

        GenerarCasillas(tamTablero);
        AleatorizarCasillas();
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

    //Reordena el arreglo de casiilas de forma aleatoria.
    private void AleatorizarCasillas() {
        while (n > 1) {
            int k = rng.Next(n--);
            string aux = casillas[n];
            casillas[n] = casillas[k];
            casillas[k] = aux;
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
    private void Validar(string nombreArchivo, string[] casillas, string tipoValidacion) {
        if (tipoValidacion == "0") validadorTablero.ValidarTableroCompleto(nombreArchivo);
        if (tipoValidacion == "1") validadorTablero.ValidarTableroActual(nombreArchivo);

        if (validadorTablero.Valido) {      //Si el tablero es válido. 
            string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("hh:mm:ss.ffff tt");
            File.Copy("Tablero.txt", date + " " + time + ".txt", true);    //Se copia el archivo del tablero con otro nombre.
        }
        Console.Clear();
        Console.WriteLine("Ejecutando... Generación y validación de tableros...");
        Console.WriteLine("El programa se detendrá cuando finalice de permutar casillas.");  
    }

    //Permuta dos posiciones en el arreglo.
    private void Permutar(string[] casillas, int i, int j) {
        
        string temp = casillas[i];
        casillas[i] = casillas[j];
        casillas[j] = temp;
    }

    //Función recursiva que explora todas las permutaciones entre elementos posibles.
    private void Combinaciones(string[] casillas, int cid, string tipoValidacion) {
        if (cid == casillas.GetLength(0) -1) {
            string nombreArchivo = "Tablero.txt";
            CrearDocumento(nombreArchivo, casillas);
            Validar(nombreArchivo, casillas, tipoValidacion);
            return; 
        }

        for (int i = cid; i < casillas.GetLength(0); i ++) {
            Permutar(casillas, i, cid);
            Combinaciones(casillas, cid+1, tipoValidacion);
            Permutar(casillas, i, cid);
        }
    }

    //Método utilizado para llamar y ejecutar el resto de funciones.
    public void GenerarTablero(string tipoValidacion) {
        Combinaciones(casillas, 0, tipoValidacion);
    }

}