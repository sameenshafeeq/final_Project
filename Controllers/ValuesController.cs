using penalty__calculator__final__project.DataLayer;
using penalty__calculator__final__project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace penalty__calculator__final__project.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("GetCountriesData")]
        public List<Country> Get()
        {
            SQLDataHelper sqlData = new SQLDataHelper();
            return sqlData.GetCountryData();
        }

        // GET api/values
    

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
