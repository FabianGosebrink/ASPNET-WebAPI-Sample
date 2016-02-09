# Sample ASP.NET WebApi
A simple Asp.Net WebAPI with CRUD Operations, using modelstate, OData enabled &amp; returning correct statuscodes

I used mapper here to map from a DTO to an entity to show how to handle this case. 

You can fire normal queries like GET/POST/PUT/PATCH/DELETE

But also do OData-Queries like 

http://.../api/house?$orderby=Id desc

or

http://.../api/house/1?$select=Street, Id
