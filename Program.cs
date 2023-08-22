class Program
{
    private string MensajeValidacion() {
        string? validacionCompleta = "";

        do {
            Console.WriteLine("Qué tipo de validación desea: ");
            Console.WriteLine("(0) - Validación Completa (Rotar y reordenar Cuadrantes)");
            Console.WriteLine("(1) - Validación Parcial (Solo el tablero actual, sin rotar ni reoridenar)");
            Console.Write("Ingrese la opción correspondiente : ");
            validacionCompleta = Console.ReadLine();
            Console.WriteLine();

            if (validacionCompleta != "0" && validacionCompleta != "1") 
                Console.WriteLine("Opción ingresada no es válida. Intente de nuevo.");
        } while (validacionCompleta != "0" && validacionCompleta != "1");

        return validacionCompleta;
    }
    private void GVCAleatorio(int tamTablero) {
        string? validacionCompleta = MensajeValidacion();

        GeneradorAleatorio generadorTablero = new GeneradorAleatorio(tamTablero);
        Validador validadorTablero = new Validador();
        string nombreArchivo = "Tablero.txt";

        while(true) {
            generadorTablero.GenerarTablero(nombreArchivo);
            if (validacionCompleta == "0") validadorTablero.ValidarTableroCompleto(nombreArchivo);
            else if (validacionCompleta == "1") validadorTablero.ValidarTableroActual(nombreArchivo);
            

            if (validadorTablero.Valido) {      //Si el tablero es válido. 
                string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
                string time = DateTime.Now.ToString("hh:mm:ss.ffff tt");
                File.Copy("Tablero.txt", date + " " + time + ".txt", true);    //Se copia el archivo del tablero con otro nombre.
                
                Console.WriteLine("¡Se ha encontrado un tablero válido!");
                break;
            }
            Console.Clear();
            Console.WriteLine(validadorTablero.Valido);
            Console.WriteLine("Ejecutando... Generación y validación de tableros..."); 
            Console.WriteLine("El programa se detendrá cuando se encuentre un tablero válido.");            
        } 
    }

    private void GVCPermutado() {
        string validacionCompleta = MensajeValidacion();

        GeneradorPermutado generadorTablero = new GeneradorPermutado();
        Validador validadorTablero = new Validador();
        String archivoValido = "0_TableroValidoPrueba.txt";
        String nombreArchivo = "Tablero.txt";

        generadorTablero.LeerTablero(archivoValido);
        for (int i = 0; i < generadorTablero.NumCasillas(); i ++) {
            for (int j = i+1; j < generadorTablero.NumCasillas(); j++) {
                generadorTablero.GenerarTablero(archivoValido, nombreArchivo, i, j);
                if (validacionCompleta == "0") validadorTablero.ValidarTableroCompleto(nombreArchivo);
                else if (validacionCompleta == "1") validadorTablero.ValidarTableroActual(nombreArchivo);

                if (validadorTablero.Valido) {      //Si el tablero es válido. 
                    string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
                    string time = DateTime.Now.ToString("hh:mm:ss.ffff tt");
                    File.Copy("Tablero.txt", date + " " + time + ".txt", true);    //Se copia el archivo del tablero con otro nombre.
                    
                    Console.WriteLine("¡Se ha encontrado un tablero válido!");
                    break;
                }
                Console.Clear();
                Console.WriteLine("Ejecutando... Generación y validación de tableros...");
                Console.WriteLine("El programa se detendrá cuando se encuentre un tablero válido, o finalice de permuter casillas.");  
            }
        }
    }
    private static void Main(string[] args)
    {   
        Program p = new Program();
        string? tipoGeneracion = "";

        Console.Clear();
        do {
            Console.WriteLine("-----------------AlgoritmoGVC");
            Console.WriteLine("Qué método de generación desea utilizar: ");
            Console.WriteLine("(0) - Aleatoria (genera un tablero al azar)");
            Console.WriteLine("(1) - Permitado (utiliza un tablero válido permuta sus casillas)");
            Console.Write("Ingrese la opción correspondiente : ");
            tipoGeneracion = Console.ReadLine();
            Console.WriteLine();

            if (tipoGeneracion != "0" && tipoGeneracion != "1") 
                Console.WriteLine("Opción ingresada no es válida. Intente de nuevo.");
        } while (tipoGeneracion != "0" && tipoGeneracion != "1");

        switch (tipoGeneracion) {
            case "0": 
                int tamTablero = 0;
                do {
                    Console.Write("Ingrese el tamaño del tablero que desea generar (2, 4 o 6) : ");
                    tamTablero = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    
                    if (tamTablero%2 != 0 || tamTablero < 2 || tamTablero > 6)    //Si el tamaño del tablero ingresado no es válido.
                        Console.WriteLine("El tamaño del tablero no puede ser un número impar, menor a 2, o mayor a 6. Intente de nuevo.");
                } while (tamTablero%2 != 0 || tamTablero < 2 || tamTablero > 6);

                p.GVCAleatorio(tamTablero);
                break;
            case "1":
                p.GVCPermutado();
                break;
        }  
    }
}