# SampleWebApi
A simple Asp.Net WebAPI with CRUD Operations, using modelstate, OData enabled &amp; returning correct statuscodes

You can fire normal queries like GET/POST/PUT/DELETE

But also do OData-Queries like 

http://.../api/house?$orderby=Id desc

or

http://.../api/house/1?$select=Street, Id
