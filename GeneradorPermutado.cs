class GeneradorPermutado {
    private List<String> casillas;

    public GeneradorPermutado() {
        casillas = new List<String>();
    }

    public int NumCasillas() { 
        return casillas.Count(); 
    }
    public void LeerTablero(String ruta) {
        char[] delimit = new char[] { ' ', '\n' };
        StreamReader objReader = new StreamReader(ruta, System.Text.Encoding.Default);
        String ? linea = "";
        
        casillas.Clear();
        while (linea != null) {
            linea = objReader.ReadLine();
            if (linea != null) 
                foreach (string substr in linea.Split(delimit))
                    if(substr != "")
                        casillas.Add(substr);
        }
        objReader.Close();    //Cerrar archivo.    
    }

    private void PermutarCasillas(int a, int b) {
        String aux = "";

        aux = casillas[a];
        casillas[a] = casillas[b];
        casillas[b] = aux;
    }

    private void CrearDocumento(String nombreArchivo) {
        StreamWriter sw = new StreamWriter(nombreArchivo);
        int k = 0;
        int cantCasillas = (int) Math.Sqrt(casillas.Count());


        for (int i = 0; i < cantCasillas; i++){
            for (int j = 0; j < cantCasillas; j++) {
                sw.Write(casillas[k] + " ");
                k++;
            }
            sw.WriteLine();
        }
        sw.Close();
    }

    public void GenerarTablero(String archivoValido, String nombreArchivo, int a, int b) {
        LeerTablero(archivoValido);
        PermutarCasillas(a, b);
        CrearDocumento(nombreArchivo);
    }

}