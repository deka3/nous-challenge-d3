using AutoMapper;
using CleaningManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningManagement.Service.Infrastructure
{
    public class CleaningPlanProfile:Profile
    {
        public CleaningPlanProfile()
        {
            this.CreateMap<CleaningPlan, Dto.CleaningPlan>().ReverseMap();
        }
    }
}
