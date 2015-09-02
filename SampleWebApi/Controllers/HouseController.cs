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
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public IHttpActionResult Get()
        {
            return Ok(Singleton.Instance.Houses.AsQueryable());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetSingle(int id)
        {
            House house = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (house == null)
            {
                return NotFound();
            }

            return Ok(house);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] House house)
        {
            if (house == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            House houseToAdd = new House
            {
                Id = !Singleton.Instance.Houses.Any() ? 1 : Singleton.Instance.Houses.Max(x => x.Id) + 1,
                City = house.City,
                Street = house.Street, 
                ZipCode = house.ZipCode
            };

            Singleton.Instance.Houses.Add(houseToAdd);

            return CreatedAtRoute("DefaultApi", new { id = houseToAdd.Id }, houseToAdd);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] House newHouse)
        {
            if (newHouse == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            House houseToUpdate = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseToUpdate == null)
            {
                return NotFound();
            }

            houseToUpdate.ZipCode = newHouse.ZipCode;
            houseToUpdate.Street = newHouse.Street;
            houseToUpdate.City = newHouse.City;

            //Update to Database --> Is singleton in this case....

            return Ok(houseToUpdate);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            House houseToDelete = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseToDelete == null)
            {
                return NotFound();
            }

            Singleton.Instance.Houses.Remove(houseToDelete);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}