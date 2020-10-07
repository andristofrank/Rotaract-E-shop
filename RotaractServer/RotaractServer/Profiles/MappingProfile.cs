using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using RotaractServer.Models;

namespace RotaractServer.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductsDto>();
            CreateMap<ProductsDto, Product>();

            CreateMap<DonationProgram, DonationProgramDto>();
            CreateMap<DonationProgramDto, DonationProgram>();
        }


        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<MappingProfile>();
            });
        }

    }
}