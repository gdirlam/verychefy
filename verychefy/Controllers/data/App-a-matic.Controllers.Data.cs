using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using App_a_matic;
using App_a_matic.Orm; 
using App_a_matic.Helper;
using verychefy.Models.Products;

namespace verychefy.Data.Controllers {

	namespace dbo{

	    using  verychefy.Models.dbo;

	}				
					namespace Products{

	    using  verychefy.Models.Products;

			public class IngredientsController : App_a_matic.Controllers.ApiController {
			
			#region "Ingredients: GET, PUT, POST, DELETE"

				// GET Data/Products/Ingredients
				public List<Ingredients> Get() {
					return base.Get<Ingredients>(); 
				}
				// GET Data/Products/Ingredients/1
				public Ingredients Get(int id) {
					return base.Get<Ingredients>(id);
				}
				// POST Data/Products/Ingredients
				public void Post([FromBody]IOrmModel model) {
					base.Post<Ingredients>((IOrmModel)model);
				}
				// PUT Data/Products/Ingredients/1
				public void Put(int id, [FromBody] IOrmModel model) {
					base.Put<Ingredients>(id, (IOrmModel)model);
				}
				// DELETE Data/Products/Ingredients/1
				public void Delete(int id) {
					base.Delete<Ingredients>(id);
				}

			#endregion

			}

			public class PairingsController : App_a_matic.Controllers.ApiController {
			
			#region "Pairings: GET, PUT, POST, DELETE"

				// GET Data/Products/Pairings
				public List<Pairings> Get() {
					return base.Get<Pairings>(); 
				}
				// GET Data/Products/Pairings/1
				public Pairings Get(int id) {
					return base.Get<Pairings>(id);
				}
				// POST Data/Products/Pairings
				public void Post([FromBody]IOrmModel model) {
					base.Post<Pairings>((IOrmModel)model);
				}
				// PUT Data/Products/Pairings/1
				public void Put(int id, [FromBody] IOrmModel model) {
					base.Put<Pairings>(id, (IOrmModel)model);
				}
				// DELETE Data/Products/Pairings/1
				public void Delete(int id) {
					base.Delete<Pairings>(id);
				}

			#endregion

			}

			public class ProximityController : App_a_matic.Controllers.ApiController {
			
			#region "Proximity: GET, PUT, POST, DELETE"

				// GET Data/Products/Proximity
				public List<Proximity> Get() {
					return base.Get<Proximity>(); 
				}
				// GET Data/Products/Proximity/1
				public Proximity Get(int id) {
					return base.Get<Proximity>(id);
				}
				// POST Data/Products/Proximity
				public void Post([FromBody]IOrmModel model) {
					base.Post<Proximity>((IOrmModel)model);
				}
				// PUT Data/Products/Proximity/1
				public void Put(int id, [FromBody] IOrmModel model) {
					base.Put<Proximity>(id, (IOrmModel)model);
				}
				// DELETE Data/Products/Proximity/1
				public void Delete(int id) {
					base.Delete<Proximity>(id);
				}

			#endregion

			}

			public class TypesController : App_a_matic.Controllers.ApiController {
			
			#region "Types: GET, PUT, POST, DELETE"

				// GET Data/Products/Types
				public List<Types> Get() {
					return base.Get<Types>(); 
				}
				// GET Data/Products/Types/1
				public Types Get(int id) {
					return base.Get<Types>(id);
				}
				// POST Data/Products/Types
				public void Post([FromBody]IOrmModel model) {
					base.Post<Types>((IOrmModel)model);
				}
				// PUT Data/Products/Types/1
				public void Put(int id, [FromBody] IOrmModel model) {
					base.Put<Types>(id, (IOrmModel)model);
				}
				// DELETE Data/Products/Types/1
				public void Delete(int id) {
					base.Delete<Types>(id);
				}

			#endregion

			}

	}				
				
}