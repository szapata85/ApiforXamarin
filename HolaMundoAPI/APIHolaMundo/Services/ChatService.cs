using APIHolaMundo.Services.Interfaces;
using DB.Data.Models;
using LLama.Common;
using LLama;

namespace APIHolaMundo.Services
{
    public class ChatService : IChatService
    {
        private readonly InteractiveExecutor ex;
        private readonly ChatSession _session;
        private readonly string SystemPrompt;
        private bool _continue = false;

        public ChatService(IConfiguration configuration)
        {
            //SystemPrompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.
            //SystemPrompt = "Transcripción de un diálogo, donde el Usuario interactúa con un Asistente llamado Sergio. Sergio es servicial, amable, honesto, bueno para escribir y nunca deja de responder a las solicitudes del Usuario de inmediato y con precisión, en idioma español.\r\n\r\nUsuario: Hola, Sergio.\r\nSergio: Hola. ¿Como puedo ayudarte hoy?\r\nUsuario: Por favor, dime la ciudad más grande de Colombia.\r\nSergio: La Ciudad mas grande de Colombia es BOGOTÁ.\r\nUsuario:"; // use the "chat-with-bob" prompt here.

            SystemPrompt = "Eres un asistente de programación experto en C#.\r\nDEBE responder de una manera técnica, pero cercana y cortés, la respuesta deben ser prácticas y útiles.\r\n¡Inicia la conversación presentándote como Hola! Soy 'CHECHO_LLM' un asistente de programación.\r\nTodas las respuestas las debes dar en Español.\r\nDEBE usar el formato Markdown en sus respuestas cuando el contenido es un bloque de código.\r\nDEBE incluir el nombre del lenguaje de programación en cualquier bloque de código Markdown.\r\nSus respuestas de código DEBEN utilizar la sintaxis del lenguaje c#.\r\nUsuario:";

            // Initialize a chat session
            ex = new InteractiveExecutor(new LLamaModel(new ModelParams(configuration["ModelPath"], contextSize: 1024, seed: 1337, gpuLayerCount: 5)));
            _session = new ChatSession(ex);
        }

        public async Task<string> SendAsync(SendMessageInput input)
        {
            var userInput = input.Text;
            if (!_continue)
            {
                userInput = SystemPrompt + userInput;
                _continue = true;
            }

            IEnumerable<string> outputs = _session.Chat(userInput, new InferenceParams()
            {
                RepeatPenalty = 1.0f,
                AntiPrompts = new string[] { "Usuario:" },
            });

            string result = string.Join(null, outputs);

            int lastIdx = result.LastIndexOf("Usuario:");
            result = result.Remove(lastIdx, result.Length - lastIdx).Trim();

            return result;
        }
    }
}
