using info;
using Modelos;
using Newtonsoft.Json;
using Services;

namespace Services
{
    public class EspelharCliente
    {

        private readonly Cliente cliente;

        public EspelharCliente(Cliente cliente)
        {
            this.cliente = cliente;
        }

        public bool Salvar()
        {
            try
            {
                var json = JsonConvert.SerializeObject(cliente);
                var result = ConsumirWS.WSInsertUpdate(json);
                return true;
            }
            catch (Exception e)
            {
                Logger.Erro(e.Message);
                return false;
            }
        }


    }
}
