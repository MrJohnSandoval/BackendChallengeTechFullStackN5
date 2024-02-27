using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BackendChallengeTechFullStackN5.Data;
using BackendChallengeTechFullStackN5.Models;
using System.Linq;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Elasticsearch.Net;
using Nest;
using System;
using Newtonsoft.Json;

namespace BackendChallengeTechFullStackN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]        
    public class PermisoController: ControllerBase
    {                      
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ElasticLowLevelClient _elasticClient;
        private readonly ProducerConfig _producerConfig;
        private readonly string _kafkaTopic;

        public PermisoController(IConfiguration configuration, ApplicationDbContext context)
        {
            _context = context;
            _configuration = configuration;

            // Obtengo el valor de ElasticsearchURL de la configuración
            string elasticsearchUrl = _configuration.GetValue<string>("Environment:ElasticsearchURL");

            // Configuro la conexión a Elasticsearch            
            var connectionSettings = new ConnectionSettings(new Uri(elasticsearchUrl));

            // Creo una instancia del cliente de Elasticsearch
            _elasticClient = new ElasticLowLevelClient(connectionSettings);

            string kafkaBootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers");
            _kafkaTopic = _configuration.GetValue<string>("Kafka:Topic");

            _producerConfig = new ProducerConfig { BootstrapServers = kafkaBootstrapServers };
        }

        // GET: api/Permiso/GetPermissions
        [HttpGet("GetPermissions")]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermissions()
        {
            // Obtenengo los permisos de la base de datos
            var permisos = await _context.Permisos.ToListAsync();

            // Itero sobre cada permiso y Indexolo en Elasticsearch
            foreach (var permiso in permisos)
            {
                // Creo el documento utilizando los datos del permiso
                var document = new
                {
                    id = permiso.Id,
                    nombreEmpleado = permiso.NombreEmpleado,
                    apellidoEmpleado = permiso.ApellidoEmpleado,
                    tipoPermiso = permiso.TipoPermiso,
                    fechaPermiso = permiso.FechaPermiso.ToString("yyyy-MM-dd"),
                    accion = "GetPermissions"
                };

                var kafkaMessage = new
                {
                    id = Guid.NewGuid().ToString(),
                    operation = "GetPermissions",
                    permiso = permiso
                };

                SendMessageToKafka(kafkaMessage);

                // Indexo el documento en Elasticsearch
                var indexResponse = _elasticClient.Index<StringResponse>("permisos-n5", JsonConvert.SerializeObject(document));

                if (!indexResponse.Success)
                {
                    // Manejo el error si la indexación falla
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al Indexo el documento en Elasticsearch");
                }
            }

            // Devuelvo los permisos obtenidos de la base de datos
            return permisos;
        }

        // GET: api/Permiso/GetPermission
        [HttpGet("GetPermission/{id}")]
        public async Task<ActionResult<Permiso>> GetPermission(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);

            if (permiso == null)
            {
                return NotFound();
            }
            else
            {
                // Creo el documento utilizando los datos del permiso
                var document = new
                {
                    id = permiso.Id,
                    nombreEmpleado = permiso.NombreEmpleado,
                    apellidoEmpleado = permiso.ApellidoEmpleado,
                    tipoPermiso = permiso.TipoPermiso,
                    fechaPermiso = permiso.FechaPermiso.ToString("yyyy-MM-dd"),
                    accion = "GetPermission"
                };  

                var kafkaMessage = new
                {
                    id = Guid.NewGuid().ToString(),
                    operation = "GetPermission",
                    permiso = permiso
                };

                SendMessageToKafka(kafkaMessage);         

                // Indexo el documento en Elasticsearch
                var indexResponse = _elasticClient.Index<StringResponse>("permisos-n5", JsonConvert.SerializeObject(document));

                if (!indexResponse.Success)
                {
                    // Manejo el error si la indexación falla
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al Indexo el documento en Elasticsearch");
                }
            }

            return permiso;
        }

        // POST: api/Permiso/RequestPermission
        [HttpPost("RequestPermission")]
        public async Task<ActionResult<Permiso>> RequestPermission(Permiso permiso)
        {
            //Post(); 
            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

           // Creo el documento utilizando los datos del permiso
            var document = new
            {
                id = permiso.Id,
                nombreEmpleado = permiso.NombreEmpleado,
                apellidoEmpleado = permiso.ApellidoEmpleado,
                tipoPermiso = permiso.TipoPermiso,
                fechaPermiso = permiso.FechaPermiso.ToString("yyyy-MM-dd"),
                accion = "RequestPermission"
            };      
          
            var kafkaMessage = new
            {
                id = Guid.NewGuid().ToString(),
                operation = "RequestPermission",
                permiso = permiso
            };

            SendMessageToKafka(kafkaMessage);     

            // Indexo el documento en Elasticsearch
            var indexResponse = _elasticClient.Index<StringResponse>("permisos-n5", JsonConvert.SerializeObject(document));

            if (!indexResponse.Success)
            {
                // Manejo el error si la indexación falla
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al Indexo el documento en Elasticsearch");
            }

            return CreatedAtAction("GetPermissions", new { id = permiso.Id }, permiso);
        }

        // PUT: api/Permiso/ModifyPermission/1
        [HttpPut("ModifyPermission/{id}")]
        public async Task<ActionResult<Permiso>> ModifyPermission(int id, Permiso permiso)
        {
            //Put(); 
            if (id != permiso.Id)
            {
                return BadRequest();
            }

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var permisoActualizado = await _context.Permisos.FindAsync(id);

            // Creo el documento utilizando los datos del permiso
            var document = new
            {
                id = permiso.Id,
                nombreEmpleado = permiso.NombreEmpleado,
                apellidoEmpleado = permiso.ApellidoEmpleado,
                tipoPermiso = permiso.TipoPermiso,
                fechaPermiso = permiso.FechaPermiso.ToString("yyyy-MM-dd"),
                accion = "ModifyPermission"
            };           

            var kafkaMessage = new
            {
                id = Guid.NewGuid().ToString(),
                operation = "ModifyPermission",
                permiso = permiso
            };

            SendMessageToKafka(kafkaMessage);

            // Indexo el documento en Elasticsearch
            var indexResponse = _elasticClient.Index<StringResponse>("permisos-n5", JsonConvert.SerializeObject(document));

            if (!indexResponse.Success)
            {
                // Manejo el error si la indexación falla
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al Indexo el documento en Elasticsearch");
            }

            return permisoActualizado;
        }

        // DELETE: api/Permiso/RemovePermission/5
        [HttpDelete("RemovePermission/{id}")]
        public async Task<ActionResult<Permiso>> RemovePermission(int id)
        {
            //Delete(); 
            var permiso = await _context.Permisos.FindAsync(id);                        

            if (permiso == null)
            {
                return NotFound();
            }

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();

            // Creo el documento utilizando los datos del permiso
            var document = new
            {
                id = permiso.Id,
                nombreEmpleado = permiso.NombreEmpleado,
                apellidoEmpleado = permiso.ApellidoEmpleado,
                tipoPermiso = permiso.TipoPermiso,
                fechaPermiso = permiso.FechaPermiso.ToString("yyyy-MM-dd"),
                accion = "RemovePermission"
            };   

            var kafkaMessage = new
            {
                id = Guid.NewGuid().ToString(),
                operation = "RemovePermission",
                permiso = permiso
            };

            SendMessageToKafka(kafkaMessage);        

            // Indexo el documento en Elasticsearch
            var indexResponse = _elasticClient.Index<StringResponse>("permisos-n5", JsonConvert.SerializeObject(document));

            if (!indexResponse.Success)
            {
                // Manejo el error si la indexación falla
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al Indexo el documento en Elasticsearch");
            }

            return permiso;
        }

        // POST: api/Permiso/BasicAuthentication
        [HttpPost("BasicAuthentication")]
        public Task<ActionResult<AuthenticationResponse>> BasicAuthentication(BasicAuthentication basicAuthentication)
        {            
            string Buser = _configuration.GetValue<string>("BasicAuthentication:User");
            string Bpass = _configuration.GetValue<string>("BasicAuthentication:Pass");

            var authRequest = new BasicAuthentication
            {
                User = basicAuthentication.User.ToString(),
                Pass = basicAuthentication.Pass.ToString()
            };           

            var authResponseOk = new AuthenticationResponse
            {
                User = basicAuthentication.User,
                Response = "Ok"
            };

            var authResponseFail = new AuthenticationResponse
            {
                User = basicAuthentication.User,
                Response = "Fail"
            };

            if (Buser.Equals(authRequest.User, StringComparison.Ordinal) && Bpass.Equals(authRequest.Pass, StringComparison.Ordinal))
            {
                return Task.FromResult<ActionResult<AuthenticationResponse>>(Ok(authResponseOk));
            }
            else
            {
                return Task.FromResult<ActionResult<AuthenticationResponse>>(BadRequest(authResponseFail));
            }

        }

        private void SendMessageToKafka(object message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                try
                {
                    var serializedMessage = JsonConvert.SerializeObject(message);
                    producer.Produce(_kafkaTopic, new Message<Null, string> { Value = serializedMessage });
                }
                catch (ProduceException<Null, string> e)
                {
                    // Manejo errores de Kafka
                    Console.WriteLine($"Error al enviar mensaje a Kafka: {e.Error.Reason}");
                    //StatusCode(StatusCodes.Status500InternalServerError, "Error kafka:" + e.Error.ToString());
                }
            }
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Id == id);
        }
    }
}