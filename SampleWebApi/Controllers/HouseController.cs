using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using SampleWebApi.Models;
using SampleWebApi.Services;

namespace SampleWebApi.Controllers
{
    [RoutePrefix("api/house")]
    public class HouseController : ApiController
    {
        private readonly HouseMapper _houseMapper;

        public HouseController()
        {
            _houseMapper = new HouseMapper();
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public IHttpActionResult Get()
        {
            return Ok(Singleton.Instance.Houses.Select(x => _houseMapper.MapToDto(x)));
        }

        [HttpGet]
        [Route("{id:int}")]
        [EnableQuery(PageSize = 1)]
        public IHttpActionResult GetSingle(int id)
        {
            HouseEntity houseEntity = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseEntity == null)
            {
                return NotFound();
            }

            return Ok(_houseMapper.MapToDto(houseEntity));
        }

        [HttpPost]
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

            Singleton.Instance.Houses.Add(houseEntity);

            return CreatedAtRoute("DefaultApi", new { id = houseEntity.Id }, _houseMapper.MapToDto(houseEntity));
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

            HouseEntity houseEntityToUpdate = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseEntityToUpdate == null)
            {
                return NotFound();
            }

            houseEntityToUpdate.ZipCode = houseDto.ZipCode;
            houseEntityToUpdate.Street = houseDto.Street;
            houseEntityToUpdate.City = houseDto.City;

            //Update to Database --> Is singleton in this case....

            return Ok(_houseMapper.MapToDto(houseEntityToUpdate));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            HouseEntity houseEntityToDelete = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseEntityToDelete == null)
            {
                return NotFound();
            }

            Singleton.Instance.Houses.Remove(houseEntityToDelete);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}