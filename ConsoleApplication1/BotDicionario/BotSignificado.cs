namespace BotDicionario
{
    public class BotSignificado
    {
        private const int MÁXIMO_TAMANHO_MENSAGEM = 500;
        private Dicionario dicionário;

        public BotSignificado()
        {
            dicionário = new Dicionario();
        }

        public string ObtémResposta(string pergunta)
        {
            pergunta = pergunta.Trim();
            string resposta = "";
            string[] palavras = pergunta.Split(' ');

            foreach (string palavra in palavras)
            {
                resposta += ObtemRespostaPalavraÚnica(palavra) + "\n";
                if (resposta.Length > MÁXIMO_TAMANHO_MENSAGEM)
                    return resposta;
            }

            return resposta;
        }

        public string ObtemRespostaPalavraÚnica(string palavra)
        {
            palavra = palavra.Trim().ToLower();
            string resposta = "";

            string significado;
            if (dicionário.hash.TryGetValue(palavra, out significado))
            {
                resposta += significado;
            } else
            {
                resposta += "Tem certeza?\n";

                var sugestões = dicionário.ObterSemelhante(palavra, 2);


                foreach (string sugestão in sugestões)
                {
                    string respostaPalavraÚnica = ObtemRespostaPalavraÚnica(sugestão);
                    resposta += string.Format("{0}: {1}", sugestão, respostaPalavraÚnica);

                }
            }

            resposta += "\n\n";

            return resposta;

        } 
    }
}
