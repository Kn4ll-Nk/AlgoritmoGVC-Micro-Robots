class Program
{
    private int SolicitudTamano() {
        int tamTablero = 0;
        do {
            Console.Write("Ingrese el tamaño del tablero que desea generar (2, 4 o 6) : ");
            tamTablero = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            
            if (tamTablero%2 != 0 || tamTablero < 2 || tamTablero > 6)    //Si el tamaño del tablero ingresado no es válido.
                Console.WriteLine("El tamaño del tablero no puede ser un número impar, menor a 2, o mayor a 6. Intente de nuevo.");
        } while (tamTablero%2 != 0 || tamTablero < 2 || tamTablero > 6);

        return tamTablero;
    }
    private string MensajeValidacion() {
        string? tipoValidacion = "";
        do {
            Console.WriteLine("Qué tipo de validación desea: ");
            Console.WriteLine("(0) - Validación Completa (Rotar y reordenar Cuadrantes)");
            Console.WriteLine("(1) - Validación Parcial (Solo el tablero actual, sin rotar ni reoridenar)");
            Console.Write("Ingrese la opción correspondiente : ");
            tipoValidacion = Console.ReadLine();
            Console.WriteLine();

            if (tipoValidacion != "0" && tipoValidacion != "1") 
                Console.WriteLine("Opción ingresada no es válida. Intente de nuevo.");
        } while (tipoValidacion != "0" && tipoValidacion != "1");
        return tipoValidacion;
    }
    private void GVCAleatorio(int tamTablero) {
        string? tipoValidacion = MensajeValidacion();

        GeneradorAleatorio generadorTablero = new GeneradorAleatorio(tamTablero);
        Validador validadorTablero = new Validador();
        string nombreArchivo = "Tablero.txt";

        while(true) {
            generadorTablero.GenerarTablero(nombreArchivo);
            if (tipoValidacion == "0") validadorTablero.ValidarTableroCompleto(nombreArchivo);
            if (tipoValidacion == "1") validadorTablero.ValidarTableroActual(nombreArchivo);
            

            if (validadorTablero.Valido) {      //Si el tablero es válido. 
                string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
                string time = DateTime.Now.ToString("hh:mm:ss.ffff tt");
                File.Copy("Tablero.txt", date + " " + time + ".txt", true);    //Se copia el archivo del tablero con otro nombre.
                
                Console.WriteLine("¡Se ha encontrado un tablero válido!");
                break;
            }
            Console.Clear();
            Console.WriteLine("Ejecutando... Generación y validación de tableros..."); 
            Console.WriteLine("El programa se detendrá cuando se encuentre un tablero válido.");            
        } 
    }

    private void GVCPermutado(int tamTablero) {
        string tipoValidacion = MensajeValidacion();

        GenerarCombinaciones generadorTablero = new GenerarCombinaciones(tamTablero);
        generadorTablero.GenerarTablero(tipoValidacion);
    }
    private static void Main(string[] args) {   
        Program p = new Program();
        int tamTablero = 0;
        string? tipoGeneracion = "";

        Console.Clear();
        do {
            Console.WriteLine("-----------------AlgoritmoGVC");
            Console.WriteLine("Qué método de generación desea utilizar: ");
            Console.WriteLine("(0) - Aleatoria (genera un tablero al azar)");
            Console.WriteLine("(1) - Permutación (genera un tablero y explora todas las combinacioenes de casillas psibles)");
            Console.Write("Ingrese la opción correspondiente : ");
            tipoGeneracion = Console.ReadLine();
            Console.WriteLine();

            if (tipoGeneracion != "0" && tipoGeneracion != "1") 
                Console.WriteLine("Opción ingresada no es válida. Intente de nuevo.");
        } while (tipoGeneracion != "0" && tipoGeneracion != "1");


        switch (tipoGeneracion) {
            case "0": 
                tamTablero = p.SolicitudTamano();
                p.GVCAleatorio(tamTablero);
                break;
            case "1":
                tamTablero = p.SolicitudTamano();
                p.GVCPermutado(tamTablero);
                break;
        }  
    }
}