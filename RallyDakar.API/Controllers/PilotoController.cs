using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RallyDakar.API.Models;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RallyDakar.API.Controllers
{
    [ApiController]
    [Route("api/pilotos")]
    public class PilotoController : ControllerBase
    {
        private readonly IPilotoRepositorio _pilotoRepositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<PilotoController> _logger;

        public PilotoController(IPilotoRepositorio pilotoRepositorio, IMapper mapper, ILogger<PilotoController> logger)
        {
            _pilotoRepositorio = pilotoRepositorio;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}", Name = "Obter")]
        public IActionResult Obter(int id)
        {
            try
            {
                _logger.LogInformation($"Obtendo dados do piloto da base: {id}");
                var piloto = _pilotoRepositorio.Obter(id);

                if (piloto == null)
                {
                    _logger.LogWarning($"PilotoId: {id} não encontrado");
                    return NotFound();
                }

                var pilotoModelo = _mapper.Map<PilotoModelo>(piloto);

                _logger.LogInformation($"Retornando piloto modelo");
                return Ok(pilotoModelo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.ToString()}");
                return StatusCode(500, ex.Message.ToString());
            }
        }


        [HttpPost]
        public IActionResult Adicionar([FromBody] PilotoModelo pilotoModelo)
        {
            try
            {
                _logger.LogInformation("Mapeando piloto Modelo");
                var piloto = _mapper.Map<Piloto>(pilotoModelo);

                _logger.LogInformation($"Verificando se existe piloto com o id informado {piloto.Id}");
                if (_pilotoRepositorio.Existe(piloto.Id))
                {
                    _logger.LogWarning($"Já existe um piloto com a mesma identificação {piloto.Id}");
                    return StatusCode(409, "Já existe um piloto com a mesma identificação");
                }

                var numero = int.Parse("");

                _logger.LogInformation($"Adicionando piloto  {piloto.Id}");
                _logger.LogInformation($"Nome piloto:  {piloto.Nome}");
                _logger.LogInformation($"Sobrenome piloto:  {piloto.SobreNome}");
                
                _pilotoRepositorio.Adicionar(piloto);
                
                _logger.LogInformation($"Operação Adicionar Piloto ocorreu sem erros");

                _logger.LogInformation($"Mapeando o retornos");
                var pilotoModeloRetorno = _mapper.Map<PilotoModelo>(pilotoModelo);

                _logger.LogInformation($"Chamando a rota Obter");
                return CreatedAtRoute("Obter", new { id = piloto.Id }, pilotoModeloRetorno);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro: {ex.ToString()}");
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] PilotoModelo pilotoModelo)
        {
            try
            {
                _logger.LogInformation($"Verificando se piloto: {pilotoModelo.Id} existe na base");
                if (!_pilotoRepositorio.Existe(pilotoModelo.Id))
                {
                    _logger.LogWarning($"PilotoId: {pilotoModelo.Id} não encontrado");
                    return NotFound();
                }

                var piloto = _mapper.Map<Piloto>(pilotoModelo);

                _logger.LogInformation($"Atualizando a base de dados com o pilotoid: {piloto.Id}");
                _pilotoRepositorio.Atualizar(piloto);

                _logger.LogInformation($"Operação concluída");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.ToString()}");
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarParcialmentePiloto([FromBody] JsonPatchDocument<PilotoModelo> patchPilotoModelo, int id)
        {
            try
            {
                _logger.LogInformation($"Executando a atualização em patch do pilotoid {id}");
                _logger.LogInformation($"Verificando se pilotoid {id} existe na base");
                if (!_pilotoRepositorio.Existe(id))
                {
                    _logger.LogWarning($"pilotoid: {id} não encontrado");
                    return NotFound();
                }

                _logger.LogInformation($"Obtendo instancia com EFCore {id}");
                var piloto = _pilotoRepositorio.Obter(id);

                _logger.LogInformation($"Mapeando para modelo");
                var pilotoModelo = _mapper.Map<PilotoModelo>(piloto);

                _logger.LogInformation($"Aplicando o patch");
                patchPilotoModelo.ApplyTo(pilotoModelo);

                piloto = _mapper.Map(pilotoModelo, piloto);

                _logger.LogInformation($"Atualizando o pilotoid {id}");
                _pilotoRepositorio.Atualizar(piloto);

                _logger.LogInformation($"Operação concluída");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.ToString()}");
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPiloto(int id)
        {
            try
            {
                _logger.LogInformation($"Obtendo o pilotoid {id} da base");

                var piloto = _pilotoRepositorio.Obter(id);
                if(piloto == null)
                {
                    _logger.LogWarning($"pilotoid: {id} não encontrado");
                    return NotFound();
                }

                _logger.LogWarning($"Deletando o pilotoid {id} da base");
                _pilotoRepositorio.Deletar(piloto);

                _logger.LogWarning($"Finalizada a Operação");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpOptions]
        public IActionResult ListarOperacoesPermitidas()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, PATCH, OPTIONS");
            return Ok();
        }
    }
}
