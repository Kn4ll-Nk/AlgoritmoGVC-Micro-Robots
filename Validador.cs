using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Validador {
    
    #nullable disable
    private List<List<String>> tablero;   //Matriz donde se almacena el tablero del juego. 
    private List<int>[] adyacente;      //Lista que almacena los nodos adjacentes 
    #nullable restore

    private List<String> idTableros = new List<String>();       //Lista que almacena un "ID" de tableros que son válidos. 
    private List<int[]> orden = new List<int[]>();      //Lista donde se almacenan distintas secuencias de ordenamiento para reordenar los cuadrantes del tablero.
    private bool valido = true;     //Variable booleanaque representa si el tablero es válido o no.                 
    private int nNodos;      //Número de nodos que componen el grafo.
    private bool parcial = false;

    //Lee los datos de un documento .txt.
    //Los datos corresponden a un tablero de Micro Robots.
    //Recibe un String correspondiente a la ruta del archivo.
    private void LeerTablero(String ruta) {
        tablero = new List<List<String>>();

        char[] delimit = new char[] { ' ', '\n' };
        StreamReader objReader = new StreamReader(ruta, System.Text.Encoding.Default);
        String ? linea = "";
        
        while (linea != null) {
            linea = objReader.ReadLine();
            if (linea != null) {
                tablero.Add(new List<String>());
                foreach (string substr in linea.Split(delimit))
                {
                    if(substr != "") {
                        tablero.Last().Add(substr);
                    }
                }
            }
        }
        objReader.Close();    //Cerrar archivo.    
    }

    //Miestra el tablero cargado en la consola.
    private void ImprimirTablero() {
        int tam = tablero.Count();

        for (int i = 0; i < tam; i++) {
            for (int j = 0; j < tam; j++) {
                Console.Write(tablero[i][j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    //Devuelve un ID creado a partir de los datos del tablero.
    private String GenerarID() {
        String id = "";
        for (int i = 0; i < tablero.Count(); i++) {
            for (int j = 0; j < tablero.Count(); j++) {
                id += tablero[i][j];    //El ID se crea usando los datos del tablero.
            }
        }
        return id;
    }

    //Se inicializa algunas variables relacionadas al grafo.
    private void InicializarGrafo() {
        nNodos = tablero.Count() * tablero.Count();  //Cantidad de nodos que compondrá el grafo. Es equivalente al total de casillas del tablero.
        adyacente = new List<int>[nNodos];          //Se inicializa la lista de adyacentes.
        for (int i = 0; i < nNodos; i ++) {
            adyacente[i] = new List<int>();
        }
    }

    //Enlaza dos nodos a modo de arista.
    //Recibe dos enteros, que representan las posiciones que tiene los nodos en el grafo.
    private void Arista(int nodoA, int nodoB) {
        adyacente[nodoA].Add(nodoB) ;
    }

    //Verifica si todos los nodos fueron visitados.
    //Retorna verdadero o falso según corresponda.
    //Recibe un arreglo booleano donde cada índice es un nodo y el contenido si fue visitado.
    private bool ComprobarValidez(bool[] visitados) {
        foreach (var nodo in visitados) {
            if (!nodo) return false;    //Si un nodo no fue visitado, se retorna falso.
        }
        return true;
    }

    //Se encarga de recorrer el grafo.
    //Se ejecuta de forma recursiva.
    //Núcleo de la búsqueda en profundidad.
    //Recibe un entero correspondiente a la posición de un nodo, y la lista de nodos a visitar.
    private void RecorrerGrafo(int nodoA, bool[] visitados) {
        visitados[nodoA] = true;

        List<int> nodoAdj = adyacente[nodoA];
        foreach (var val in nodoAdj) {
            if (!visitados[val]) {
                RecorrerGrafo(val, visitados);
            }
        }
    }

    //Método que evalúa si todos los nodos fueron visitados.
    //Si un nodo no fue visitado, se cambia el estado de válido a falso.
    //Si todos los nodos fueron visitados, se genera un ID al tablero actual y se añade a la lista de IDs.
    //Recibe un valor entero que corresponde al nodo de inicio.
    private void DFS(int nodoA) {
        bool[] visitados = new bool[nNodos];
        
        RecorrerGrafo(nodoA, visitados);

        if (!ComprobarValidez(visitados)) {

            valido = false;
        }
        else {
            if (!parcial)
                idTableros.Add(GenerarID());
            else 
                valido = true;
        }
    }

    //Combureba si el movimiento a realizar en el tablero el válido.
    //Recibe dos Strings que corresponden a casillas dentro del tablero.
    //Devuelve verdadero si el movimiento es permitido, falso si no lo es.
    private bool ComprobarConexion(String A, String B) {
        if (A[0] == B[0] || A[1] == B[1]) {
            return true;
        }
        return false;
    }

    //Método encargado de crear el grafo en baso a los movimientos posibles dentro del tablero.
    private void CrearGrafo() {
        int TAM = tablero.Count();

        for (int i = 0; i < TAM; i ++) {
            for (int j = 0; j < TAM; j ++) {
                for (int k = 0; k < TAM; k ++) {
                    if (j != k && ComprobarConexion(tablero[i][j], tablero[i][k])) {    //Comprueba movimientos en horizontal.
                        Arista(i*TAM+j, i*TAM+k);
                    }
                    if (i != k && ComprobarConexion(tablero[i][j], tablero[k][j])) {    //Comprueba movimientos en vertical.
                        Arista(i*TAM+j, k*TAM+j);
                    }
                }
            }
        }
    }
    //Método encargado de imprimir el grafo.
    //Recibe un valor entero correspondiente al nodo actual.
    //Se ejecuta de forma recursiva.
    private void ImprimirGrafo(int nodoA) {
        if (nodoA >= nNodos) return;

        int size = adyacente[nodoA].Count();

        Console.Write(nodoA);
        if (size != 0) {
            Console.Write(" -- {");
            for (int i = 0; i < size; i ++) {
                Console.Write(adyacente[nodoA][i]);
                if (i != size - 1) {
                    Console.Write(", ");
                }
            }
            Console.WriteLine("}");
        }

        ImprimirGrafo(nodoA+1);
    }

    //Calcula todas las formas en que los cuadrantes se pueden ordenar sin repetirse.
    //Los cuadrantes son representados con números del 0 al 3.
    private void ListaOrdenamiento() {
        for (int i = 0; i < 4; i++) 
            for (int j = 0; j < 4; j++) 
                for (int k = 0; k < 4; k++) 
                    for (int l = 0; l < 4; l++) 
                        if(i != j && i != k && i != l
                        && j != k && j != l && k != l){      //Condición que evita la repetición de números.
                            orden.Add(new int[] {i,j,k,l});
                        }
        
        orden.Add(orden.First());       //Añade el primer elemento en la última posición.
        orden.Remove(orden.First());    //Remueve de la lista el primer elemento.
    }

    //Modifica el tablero acorde a los cambios realizados en los cuadrantes.
    //Recibe un arreglo 3D correspondiente a los cuadrantes,  el tamaño de los cuadrantes, y coeficientes que indican el número del cuadrante, ya sea 0, 1, 2, o 3
    private void ModificarTablero(String[,,] cuadrantes, int tamCuadrante, int c0, int c1, int c2, int c3) { 
        for (int i = 0; i < tamCuadrante; i ++) {
            for (int j = 0; j < tamCuadrante; j ++) {
                tablero[i][j] = cuadrantes[c0,i,j];     //Cuadrante superior izquierdo.
                tablero[i][j+tamCuadrante] = cuadrantes[c1,i,j];        //Cuadrante superior derecho,
                tablero[i+tamCuadrante][j] = cuadrantes[c2,i,j];        //Cuadrante inferior izquierdo.
                tablero[i+tamCuadrante][j+tamCuadrante] = cuadrantes[c3,i,j];       //Cuadrante inferior derecho.
            }
        }
    }

    //Revisa cada posición de la lista de combinaciones posibles, y las aplica a los cuadrantes.
    //Modifica el tablero de acuerdo al orden de cuadrantes que corresponda.
    //Por cada tablero formado crea un grafo, y realiza una búsqueda en profundidad.
    private void ReordenarCuadrantes(String[,,] cuadrantes, int tamCuadrante) {
        foreach (var item in orden) {     
            ModificarTablero(cuadrantes, tamCuadrante, item[0], item[1], item[2], item[3]);
            if (idTableros.Count() > 0 && idTableros.Contains(GenerarID())) return;
            InicializarGrafo();
            CrearGrafo();
            DFS(0);
            if (!valido) return;
        }
        RotacionCuadrantes(cuadrantes);
    }

    //Método que se encarga de rotar un cuadrante. 
    //Recibe un arreglo 3D que representa los cuadrantes, y un valor entero que indica el número del cuadrante que se rotará.
    private void Rotar(String[,,] cuadrantes, int idCuadrante) {
        if (!valido) return;    //Si se ha detectado un tablero no válido, no prosigue con el método.

        int tamCuadrante = cuadrantes.GetLength(1);
        String[,] matrizAux = new String[tamCuadrante,tamCuadrante];

        for (int i = 0, k = tamCuadrante-1; i < tamCuadrante; i ++, k--) 
            for (int j = 0; j < tamCuadrante; j++) 
                matrizAux[j,k] = cuadrantes[idCuadrante,i,j];   //Traspasa los valores del cuadrante seleccionado a una matriz auxiliar, pero rotando la matriz 90° hacia la derecha. 

        for (int i = 0; i < tamCuadrante; i ++) 
            for (int j = 0; j < tamCuadrante; j++) 
                cuadrantes[idCuadrante,i,j] = matrizAux[i,j];   //Se reasigna la matriz rotada al cuadrante correspondiente.

        ReordenarCuadrantes(cuadrantes, tamCuadrante);
    }
    //Se encarga de llamar al método rotar.
    //Se utiliza a modo de realizar una ejecución recursiva del método. 
    //Recibe un arreglo 3D que representa a los cuadrantes.
    private void RotacionCuadrantes(String[,,] cuadrantes) {
        
        Rotar(cuadrantes, 0);   
        Rotar(cuadrantes, 1);
        Rotar(cuadrantes, 2);
        Rotar(cuadrantes, 3);
    }

    //Divide el tablero en 4 cuadrantes de un mismo tamaño.
    //Asigna los datos de cada cuadrante un arreglo de 3 dimensiones.
    //La primera dimensión es el número del cuadrante, la segunda y tercera dimensión son el número de filas y columnas. 
    private void DefinirCuadrantes() {
        int tamCuadrante = tablero.Count()/2;
        String[,,] cuadrantes = new String[4,tamCuadrante,tamCuadrante];    

        for (int i = 0; i < tamCuadrante; i ++) {
            for (int j = 0; j < tamCuadrante; j ++) {
                    cuadrantes[0,i,j] = tablero[i][j];
                    cuadrantes[1,i,j] = tablero[i][j+tamCuadrante];
                    cuadrantes[2,i,j] = tablero[i+tamCuadrante][j];
                    cuadrantes[3,i,j] = tablero[i+tamCuadrante][j+tamCuadrante];
            }
        }

        RotacionCuadrantes(cuadrantes);
    }    

    public void ValidarTableroCompleto(String nombreArchivo) {
        orden.Clear();
        LeerTablero(nombreArchivo);
        ListaOrdenamiento();
        DefinirCuadrantes();
    }

    public void ValidarTableroActual(String nombreArchivo) {
        parcial = true;
        LeerTablero(nombreArchivo);
        InicializarGrafo();
        CrearGrafo();
        DFS(0);
    }

    public bool Valido { get => valido; set => valido = value;}
}