using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleWebApi.Models;

namespace SampleWebApi.Services
{
    public class HouseMapper
    {
        public HouseDto MapToDto(HouseEntity houseEntity)
        {
            return new HouseDto()
            {
                Id = houseEntity.Id,
                ZipCode = houseEntity.ZipCode,
                City = houseEntity.City,
                Street = houseEntity.Street
            };
        }

        public HouseEntity MapToEntity(HouseDto houseDto)
        {
            return new HouseEntity()
            {
                Id = houseDto.Id == 0 ? Singleton.Instance.Houses.Max(x => x.Id) + 1 : houseDto.Id,
                ZipCode = houseDto.ZipCode,
                City = houseDto.City,
                Street = houseDto.Street
            };
        }
    }
}