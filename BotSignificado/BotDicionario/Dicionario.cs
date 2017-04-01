using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BotDicionario
{
    public class Dicionario
    {
        private static readonly string DIRETORIO_DICIONÁRIO = @"c:\\Dict\\";

        public Dictionary<string, string> hash = new Dictionary<string, string>();

        public Dicionario()
        {
            char[] letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            foreach (char letra in letras)
            {
                Carregar(string.Format("{0}\\{1}.txt", DIRETORIO_DICIONÁRIO, letra));
            }
        }

        public void Carregar(string arquivo)
        {
            string[] linhas = File.ReadAllLines(arquivo);

            bool lendoPalavra = true;
            string significado = "";
            string palavra = "";

            foreach (string linha in linhas)
            {
                if (lendoPalavra)
                {
                    palavra = Limpar(linha);
                    lendoPalavra = false;
                }
                else {
                    significado += linha;
                    if (String.IsNullOrWhiteSpace(linha))
                    {
                        hash[palavra] = significado;
                        significado = "";
                        lendoPalavra = true;
                    }
                }
            }
        }

        private string Limpar(string linha)
        {
            return new string(linha.Where(Char.IsLetter).ToArray()).Trim().ToLower();
        }

        public List<string> ObterSemelhante(string palavraUsuário, int qtd)
        {
            SortedList<string, int> distâncias = new SortedList<string, int>();

            foreach (var palavraDicionário in hash.Keys)
            {
                int distância = CalcLevenshteinDistance(palavraDicionário, palavraUsuário);
                distâncias.Add(palavraDicionário, distância);
            }

            var ordemPorDistância = distâncias.OrderBy(kvp => kvp.Value);
            List<string> resultado = new List<string>();
            foreach (KeyValuePair<string, int> par in ordemPorDistância)
            {
                resultado.Add(par.Key);
                if (resultado.Count == qtd)
                {
                    return resultado;
                }
            }

            throw new NotImplementedException();
        }

        private static int CalcLevenshteinDistance(string a, string b)
        {
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
    }
}
