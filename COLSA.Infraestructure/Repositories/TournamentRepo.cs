using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COLSA.DataAccess.Context;
using COLSA.Domain.Interfaces;
using COLSA.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace COLSA.Infraestructure.Repositories
{
    public class TournamentRepo : GeneralAsyncRepo<TournamentModel>, ITournament
    {
        public TournamentRepo(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}