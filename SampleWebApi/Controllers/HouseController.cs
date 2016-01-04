using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using Newtonsoft.Json;
using SampleWebApi.Models;
using SampleWebApi.Repositories;
using SampleWebApi.Services;

namespace SampleWebApi.Controllers
{
    [RoutePrefix("api/house")]
    public class HouseController : ApiController
    {
        private readonly IHouseRepository _houseRepository;
        const int MaxPageSize = 10;
        private readonly IHouseMapper _houseMapper;

        public HouseController(IHouseRepository houseRepository, IHouseMapper houseMapper)
        {
            _houseRepository = houseRepository;
            _houseMapper = houseMapper;
        }

        [HttpGet]
        [EnableQuery(PageSize = MaxPageSize)]
        [Route("")]
        public IHttpActionResult Get(int page = 1, int pageSize = MaxPageSize)
        {
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            var paginationHeader = new
            {
                totalCount = _houseRepository.GetAll().Count
                // Add more headers here if you want...
                // Link to next and previous page etc.
                // Also see OData-Options for this
            };

            List<HouseEntity> result = _houseRepository.GetAll()
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();

            HttpContext.Current.Response.AppendHeader("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

            return Ok(result.Select(x => _houseMapper.MapToDto(x)));
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetSingleHouse")]
        [EnableQuery(PageSize = 1)]
        public IHttpActionResult GetSingle(int id)
        {
            HouseEntity houseEntity = _houseRepository.GetSingle(id);

            if (houseEntity == null)
            {
                return NotFound();
            }

            return Ok(_houseMapper.MapToDto(houseEntity));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] HouseDto houseDto)
        {
            if (houseDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity houseEntity = _houseMapper.MapToEntity(houseDto);

            _houseRepository.Add(houseEntity);
            
            return CreatedAtRoute("GetSingleHouse", new { id = houseEntity.Id }, _houseMapper.MapToDto(houseEntity));
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] HouseDto houseDto)
        {
            if (houseDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity houseEntityToUpdate = _houseRepository.GetSingle(id);

            if (houseEntityToUpdate == null)
            {
                return NotFound();
            }

            houseEntityToUpdate.ZipCode = houseDto.ZipCode;
            houseEntityToUpdate.Street = houseDto.Street;
            houseEntityToUpdate.City = houseDto.City;

            HouseEntity houseEntity = _houseRepository.Update(houseEntityToUpdate);

            return Ok(_houseMapper.MapToDto(houseEntity));
        }

        [HttpPatch]
        [Route("{id:int}")]
        public IHttpActionResult Patch(int id, Delta<HouseDto> houseDto)
        {
            if (houseDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity houseEntityToUpdate = _houseRepository.GetSingle(id);

            if (houseEntityToUpdate == null)
            {
                return NotFound();
            }

            HouseDto existingHouse = _houseMapper.MapToDto(houseEntityToUpdate);
            houseDto.Patch(existingHouse);

            HouseEntity patched = _houseRepository.Update(_houseMapper.MapToEntity(existingHouse));

            return Ok(_houseMapper.MapToDto(patched));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            HouseEntity houseEntityToDelete = _houseRepository.GetSingle(id);

            if (houseEntityToDelete == null)
            {
                return NotFound();
            }

            _houseRepository.Delete(id);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}