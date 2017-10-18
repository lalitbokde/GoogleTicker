using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CpaTicker.Areas.admin.Classes;

namespace CpaTicker
{
    public static class DtoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<URL, PAGE>()
               .BeforeMap((src, dest) =>
               {
                   dest.Status = PageStatus.Active;
               });
        }
    }
}