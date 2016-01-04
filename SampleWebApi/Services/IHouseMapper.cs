using SampleWebApi.Models;

namespace SampleWebApi.Services
{
    public interface IHouseMapper
    {
        HouseDto MapToDto(HouseEntity houseEntity);
        HouseEntity MapToEntity(HouseDto houseDto);
    }
}