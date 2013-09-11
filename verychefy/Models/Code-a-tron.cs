using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc; 
using System.Runtime.Serialization; 
using App_a_matic; 
using App_a_matic.Helper;
using App_a_matic.Orm;

namespace verychefy.Models {

	namespace dbo{

	}
				
	namespace Products{

			public class Ingredients : IOrmModel {

				[AdditionalMetadata("ORM", true)]
				[AdditionalMetadata("ORM_PrimaryKey", true)]
				public int? Ingredient_pk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public string CommonName { get; set; }

				[AdditionalMetadata("ORM", true)]
				public int? Type_fk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public string Details { get; set; }

				[AdditionalMetadata("ORM", true)]
				public string MediaURL { get; set; }

				[IgnoreDataMember]
				public DataOrmContext OrmContext { get; set; }
				public  Ingredients() {
					this.OrmContext = new DataOrmContext {
						Create    = "Products.IngredientsCreate"
						 , Read   = "Products.IngredientsGet"
						 , Update = "Products.IngredientsUpdate"
						 , Delete = "Products.IngredientsDelete"
					};
				}
			}

			public class Pairings : IOrmModel {

				[AdditionalMetadata("ORM", true)]
				[AdditionalMetadata("ORM_PrimaryKey", true)]
				public int? Pairing_Pk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public int? PrimeIngredient_fk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public int? MatchingIngredient_fk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public int? Proximity_Fk { get; set; }

				[IgnoreDataMember]
				public DataOrmContext OrmContext { get; set; }
				public  Pairings() {
					this.OrmContext = new DataOrmContext {
						Create    = "Products.PairingsCreate"
						 , Read   = "Products.PairingsGet"
						 , Update = "Products.PairingsUpdate"
						 , Delete = "Products.PairingsDelete"
					};
				}
			}

			public class Proximity : IOrmModel {

				[AdditionalMetadata("ORM", true)]
				[AdditionalMetadata("ORM_PrimaryKey", true)]
				public int? Proximity_Pk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public string Closeness { get; set; }

				[AdditionalMetadata("ORM", true)]
				public string Markup { get; set; }

				[IgnoreDataMember]
				public DataOrmContext OrmContext { get; set; }
				public  Proximity() {
					this.OrmContext = new DataOrmContext {
						Create    = "Products.ProximityCreate"
						 , Read   = "Products.ProximityGet"
						 , Update = "Products.ProximityUpdate"
						 , Delete = "Products.ProximityDelete"
					};
				}
			}

			public class Types : IOrmModel {

				[AdditionalMetadata("ORM", true)]
				[AdditionalMetadata("ORM_PrimaryKey", true)]
				public int? Type_pk { get; set; }

				[AdditionalMetadata("ORM", true)]
				public string Classification { get; set; }

				[IgnoreDataMember]
				public DataOrmContext OrmContext { get; set; }
				public  Types() {
					this.OrmContext = new DataOrmContext {
						Create    = "Products.TypesCreate"
						 , Read   = "Products.TypesGet"
						 , Update = "Products.TypesUpdate"
						 , Delete = "Products.TypesDelete"
					};
				}
			}

	}
				

}
