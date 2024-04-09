﻿using AutoMapper;
using furni.Areas.Admin.Models;
using furni.Data;

namespace furni.Areas.Admin.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<ApplicationUser, UserModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}
