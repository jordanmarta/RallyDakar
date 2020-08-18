using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using RallyDakar.API.Models;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System;
using System.Collections.Generic;

namespace RallyDakar.API.Controllers
{
    [ApiController]
    [Route("api/equipes/{equipeId}/telemetria")]
    public class TelemetriaController : ControllerBase
    {
        private readonly ITelemetriaRepositorio _telemetriaRepositorio;
        private readonly IEquipeRepositorio _equipeRepositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<TelemetriaController> _logger;

        public TelemetriaController(ITelemetriaRepositorio telemetriaRepositorio, 
            IEquipeRepositorio equipeRepositorio, IMapper mapper, ILogger<TelemetriaController> logger)
        {
            _telemetriaRepositorio = telemetriaRepositorio;
            _equipeRepositorio = equipeRepositorio;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}", Name = "Obter")]
        public ActionResult<IEnumerable<TelemetriaModelo>> Obter(int equipeId)
        {
            try
            {
                _logger.LogInformation($"Verificando se equipe: {equipeId} existe na base");
                if (!_equipeRepositorio.Existe(equipeId))
                {
                    _logger.LogWarning($"Equipe id não foi identificado - Equipeid: {equipeId}");
                    return NotFound();
                }

                _logger.LogInformation($"Obtendo os dados da telemetria para a equipe: {equipeId}");
                var dadosTelemetria =_telemetriaRepositorio.ObterTodosPorEquipe(equipeId);

                if (!dadosTelemetria.Any())
                {
                    _logger.LogInformation($"Não foi retornado dados de telemetria para a equipe informada: {equipeId}");
                    return NotFound("Não foi retornado dados de telemetria para a equipe informada");
                }

                var dadosTelemetriaModelo = _mapper.Map<IEnumerable<TelemetriaModelo>>(dadosTelemetria);

                return Ok(dadosTelemetria);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.ToString()}");
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}
