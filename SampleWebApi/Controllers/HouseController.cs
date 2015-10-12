using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using Newtonsoft.Json;
using SampleWebApi.Models;
using SampleWebApi.Services;

namespace SampleWebApi.Controllers
{
    [RoutePrefix("api/house")]
    public class HouseController : ApiController
    {
        const int MaxPageSize = 10;
        private readonly HouseMapper _houseMapper;

        public HouseController()
        {
            _houseMapper = new HouseMapper();
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [Route("")]
        public IHttpActionResult Get(int page = 1, int pageSize = MaxPageSize)
        {
            if(pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            var paginationHeader = new
            {
                totalCount = Singleton.Instance.Houses.Count
                // Add more headers here if you want...
                // Link to next and previous page etc.
                // Also see OData-Options for thiss
            };

            List<HouseEntity> result = Singleton.Instance.Houses
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList();

            HttpContext.Current.Response.AppendHeader("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

            return Ok(result.Select(x => _houseMapper.MapToDto(x)));
        }

        [HttpGet]
        [Route("{id:int}")]
        [EnableQuery(PageSize = 1)]
        public IHttpActionResult GetSingle(int id)
        {
            HouseEntity houseEntity = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntity == null)
            {
                return NotFound();
            }

            return Ok(_houseMapper.MapToDto(houseEntity));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] HouseDto houseDto)
        {
            if(houseDto == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity houseEntity = _houseMapper.MapToEntity(houseDto);

            Singleton.Instance.Houses.Add(houseEntity);

            return CreatedAtRoute("DefaultApi", new { id = houseEntity.Id }, _houseMapper.MapToDto(houseEntity));
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] HouseDto houseDto)
        {
            if(houseDto == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity houseEntityToUpdate = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntityToUpdate == null)
            {
                return NotFound();
            }

            houseEntityToUpdate.ZipCode = houseDto.ZipCode;
            houseEntityToUpdate.Street = houseDto.Street;
            houseEntityToUpdate.City = houseDto.City;

            //Update to Database --> Is singleton in this case....

            return Ok(_houseMapper.MapToDto(houseEntityToUpdate));
        }

        [HttpPatch]
        [Route("{id:int}")]
        public IHttpActionResult Patch(int id, Delta<HouseDto> houseDto)
        {
            if(houseDto == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity houseEntityToUpdate = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntityToUpdate == null)
            {
                return NotFound();
            }

            HouseDto existingHouse = _houseMapper.MapToDto(houseEntityToUpdate);
            houseDto.Patch(existingHouse);

            int index = Singleton.Instance.Houses.FindIndex(x => x.Id == id);
            Singleton.Instance.Houses[index] = _houseMapper.MapToEntity(existingHouse);

            return Ok(existingHouse);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            HouseEntity houseEntityToDelete = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntityToDelete == null)
            {
                return NotFound();
            }

            Singleton.Instance.Houses.Remove(houseEntityToDelete);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}