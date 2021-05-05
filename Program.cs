using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Datatable2List
{
    public class Produto
    {
        public int Codigo { get; set; }
        public string Descricao{ get; set; }
        public double Preco { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool semargs = args.Length == 0;
            if (semargs) 
            {
                args = new string[1];
                args[0] = "1000";
            }

            TimeSpan difference;
            DateTime inicio = DateTime.UtcNow;
            List<Produto> lista;

            DataTable produtos = new("produtos");
            produtos.Columns.Add("codigo", typeof(int));
            produtos.Columns.Add("descricao", typeof(string));
            produtos.Columns.Add("Preco", typeof(double));

            int numregistros =  int.Parse(args[0]);
            Console.WriteLine(string.Format("Populando tabela com {0:n0} registros...",numregistros));
            for (int i = 0; i < numregistros; i++)
            {
                produtos.Rows.Add();
                produtos.Rows[i].SetField("codigo", i);
                produtos.Rows[i].SetField("descricao",  "produto " + i.ToString());
                produtos.Rows[i].SetField("preco", i*0.5);
            }
            difference = DateTime.UtcNow.Subtract(inicio);
            Console.WriteLine("Tabela populada: " + difference);

            lista = new List<Produto>();
            inicio = DateTime.UtcNow;
            foreach (DataRow produto in produtos.Rows)
            {
                Produto prod = new()
                {
                    Codigo = (int)produto["codigo"],
                    Descricao = (string)produto["descricao"],
                    Preco = (double)produto["preco"]
                };

                lista.Add(prod);
            }
            difference = DateTime.UtcNow.Subtract(inicio);
            Console.WriteLine(string.Format("\nLista: {0:n0} registros...", lista.Count));
            Console.WriteLine("Foreach: " + difference);

            lista = new List<Produto>();
            inicio = DateTime.UtcNow;
            lista = (from DataRow produto in produtos.Rows
                     select new Produto()
                     {
                         Codigo = (int)produto["codigo"],
                         Descricao = (string)produto["descricao"],
                         Preco = (double)produto["preco"]
                     }).ToList();
            difference = DateTime.UtcNow.Subtract(inicio);
            Console.WriteLine(string.Format("\nLista: {0:n0} registros...", lista.Count));
            Console.WriteLine("Linq...: " + difference);


            lista = new List<Produto>();
            inicio = DateTime.UtcNow;
            lista = (from DataRow produto in produtos.Rows.AsParallel()
                     select new Produto()
                     {
                         Codigo = (int)produto["codigo"],
                         Descricao = (string)produto["descricao"],
                         Preco = (double)produto["preco"]
                     }).ToList();
            difference = DateTime.UtcNow.Subtract(inicio);
            Console.WriteLine(string.Format("\nLista: {0:n0} registros...", lista.Count));
            Console.WriteLine("PLinq..: " + difference);

            if (semargs) 
            {
                Console.ReadLine();
            }   
        }
    }
}
