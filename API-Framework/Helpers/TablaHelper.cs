using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace API_Framework.Helpers
{
    public class TablaHelper
    {
        public TablaHelper()
        {
            Columnas = new List<Columna>();
            Filas = new List<Fila>();
            FilasPagina = 10;
        }

        public IEnumerable<Columna> Columnas { get; set; }
        public IEnumerable<Fila> Filas { get; set; }
        public int FilasPagina { get; set; }

        public void Ordenar()
        {
            Columnas = Columnas.OrderBy(c => c.Posicion);
            foreach(Fila fila in Filas)
            {
                fila.Celdas = fila.Celdas.OrderBy(c => c.Posicion);
            }
        }

        public static TablaHelper TablaPrueba(int columnas, int filas)
        {
            TablaHelper tabla = new TablaHelper();

            // Generar columnas
            List<Columna> cols = new List<Columna>();
            List<int> colsOcuapdas = new List<int>();
            for (int i = 0; i < columnas; i++)
            {
                int pos = GenPosRandom(colsOcuapdas, columnas);
                cols.Add(new Columna()
                {
                    Filtrar = GenRandom(),
                    Ordenar = GenRandom(),
                    Posicion = pos,
                    Valor = $"Columna {pos}"
                });
                colsOcuapdas.Add(pos);
            }
            tabla.Columnas = cols;

            List<Fila> fils = new List<Fila>();
            for (int i = 0; i< filas; i++)
            {
                Fila fila = new Fila();
                fila.Mostrar = GenRandom();
                List<Celda> celdas = new List<Celda>();
                List<int> posOcupadas = new List<int>();
                for (int j = 0; j < columnas; j++)
                {
                    int pos = GenPosRandom(posOcupadas, columnas);
                    celdas.Add(new Celda()
                    {
                        Valor = $"Fila {i + 1} Columna {pos}",
                        Posicion = pos
                    });
                    posOcupadas.Add(pos);
                }
                fila.Celdas = celdas;
                posOcupadas.Clear();
                fils.Add(fila);
            }
            tabla.Filas = fils;

            return tabla;
        }

        private static bool GenRandom()
        {
            Thread.Sleep(15);
            Random random = new Random(DateTime.Now.Millisecond);
            int r = random.Next(1, 101);
            return r > 50;
        }

        private static int GenPosRandom(IEnumerable<int> posOcuapdas, int max)
        {
            Thread.Sleep(8);
            Random random = new Random(DateTime.Now.Millisecond);
            int r = 0;
            do
            {
                r = random.Next(1, max + 1);
            } while (posOcuapdas.Contains(r));
            return r;
        }
    }

    public class Columna
    {
        public Columna()
        {
            Filtrar = true;
            Ordenar = true;
            TipoFiltro = Filtros.TEXTO;
        }

        public int Posicion { get; set; }
        public string Valor { get; set; }
        public bool Filtrar { get; set; }
        public bool Ordenar { get; set; }
        public string TipoFiltro { get; set; }

        public struct Filtros
        {
            public const string TEXTO = "texto";
            public const string FECHA = "fecha";
            public const string NUMERO = "numero";
            public const string HORA = "hora";
        }
    }

    public class Fila
    {
        public Fila()
        {
            Celdas = new List<Celda>();
            Mostrar = true;
        }

        public IEnumerable<Celda> Celdas { get; set; }
        public bool Mostrar { get; set; }
    }

    public class Celda
    {
        public Celda()
        {
        }

        public int Posicion { get; set; }
        public string Valor { get; set; }
    }
}