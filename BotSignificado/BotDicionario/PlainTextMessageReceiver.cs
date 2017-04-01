using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace BotDicionario
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;
        private BotSignificado bot = new BotSignificado();

        public PlainTextMessageReceiver(IMessagingHubSender sender)
        {
            _sender = sender;
        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            Trace.TraceInformation($"From: {message.From} \tContent: {message.Content}");
            string respostaCompleta = bot.ObtémResposta(message.Content.ToString());

            if (respostaCompleta.Length < 500)
                await _sender.SendMessageAsync(respostaCompleta, message.From, cancellationToken);
            else
            {
                await _sender.SendMessageAsync(respostaCompleta.Substring(0, 500), message.From, cancellationToken);
                await _sender.SendMessageAsync(respostaCompleta.Substring(500), message.From, cancellationToken);
            }
        }
    }
}
