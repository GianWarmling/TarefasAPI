
namespace TarefasAPI.Responses
{
    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T? Dados { get; set; }

        public static ApiResponse<T> Ok(T? dados, string mensagem = "")
        {
            return new ApiResponse<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        public static ApiResponse<T> Falha(Models.TaskModel? tarefa, string mensagem)
        {
            return new ApiResponse<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Dados = default
            };
        }

        internal static object? Falha(string v)
        {
            throw new NotImplementedException();
        }
    }
}
