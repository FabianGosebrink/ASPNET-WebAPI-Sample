using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using Microsoft.Owin;
using Owin;
using SampleWebApi.Models;
using SampleWebApi.Services;

[assembly: OwinStartup(typeof(SampleWebApi.Startup))]

namespace SampleWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.AddODataQueryFilter();

            app.UseWebApi(config);

            List<HouseEntity> houses = new List<HouseEntity>()
            {
                new HouseEntity() {City = "Town1", Id = 1, Street = "Street1", ZipCode = 1234},
                new HouseEntity() {City = "Town2", Id = 2, Street = "Street2", ZipCode = 5678}
            };

            Singleton.Instance.Houses = houses;
        }
    }
}
