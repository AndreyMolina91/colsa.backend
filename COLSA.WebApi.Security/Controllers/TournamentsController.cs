using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using COLSA.Domain.Interfaces;
using COLSA.Domain.Models;
using COLSA.Infraestructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace COLSA.WebApi.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TournamentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Tournament")]
        public async Task<ActionResult> CreateUsers(TournamentDto tournamentDto)
        {
            var tournamentToDB = _mapper.Map<TournamentModel>(tournamentDto);
            await _unitOfWork.Tournament.AddModel(tournamentToDB);
            return Ok();
        }
    }
}