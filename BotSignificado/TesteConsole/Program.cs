using BotDicionario;
using System;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            BotSignificado bot = new BotSignificado();

            while (true)
            {
                string pergunta = Console.ReadLine();
                Console.WriteLine(bot.ObtémResposta(pergunta));

                Console.WriteLine("\n\n");
            }
        }
    }
}
