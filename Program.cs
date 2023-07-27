using System;

class Program
{
    private static void Main(string[] args)
    {   
        int tamTablero = Int32.Parse(args[0]);  //Número ingresado por consola al ejecutar el programa.
        if (tamTablero%2 != 0 || tamTablero < 2 || tamTablero > 6) {    //Si el tamaño del tablero ingresado no es válido.
            Console.WriteLine("El tamaño del tablero no puede ser un número impar, menor a 2, o mayor a 6");
            return;
        }

        Generador generadorTablero = new Generador(tamTablero);
        Validador validadorTablero = new Validador();

        while(true) {
            generadorTablero.GenerarTablero("Tablero.txt");
            validadorTablero.ValidarTablero("Tablero.txt", 0);

            if (validadorTablero.Valido) {      //Si el tablero es válido. 
                string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
                string time = DateTime.Now.ToString("hh:mm:ss tt");
                File.Copy("Tablero.txt", date + " " + time + ".txt", true);    //Se copia el archivo del tablero con otro nombre.
                
                Console.WriteLine("¡Se ha encontrado un tablero válido!");
                break;
            }
            Console.Clear();
            Console.WriteLine("Ejecutando... Generación y validación de tableros...");            
        }
    }
}