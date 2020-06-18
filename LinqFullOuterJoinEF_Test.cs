using System;
using System.Linq;
using System.Linq.Expressions;
using Csla;
using Csla.Data.EF6;
using NrgNet.DAL.EF;
using NrgNet.Library.TypeExtensionsUtils;

namespace WinFormTestsMM.Linq
{
	public class LinqFullOuterJoinEF_Test
	{
		public static void Execute()
		{
			Expression<Func<Person, Guid>> ExpKey = k => k.PersonId;

			Expression<Func<Person, Person, PersonExt>> ExpPersonaExt = (p, c) => new PersonExt
			{
				SpecV = p != null ? p.Spec : "",
				SpecC = c != null ? c.Spec : "",
				Name = p != null ? p.Name : c.Name,
				PersonId = p != null ? p.PersonId : c.PersonId
			};

			using (var ctx = DbContextManager<NrgNetContext>.GetManager())
			{
				IQueryable<Person> vendors = ctx.DbContext.GenPersonas.Where(r => r.ComProveedor != null).Select(s => new Person { Spec = "V", Name = s.Nombre, PersonId = s.PersonaId });

				IQueryable<Person> customers = ctx.DbContext.GenPersonas.Where(r => r.VenCliente != null).Select(s => new Person { Spec = "C", Name = s.Nombre, PersonId = s.PersonaId });

				IQueryable<PersonExt> fj = vendors.FullOuterJoin(customers, ExpKey, ExpKey, ExpPersonaExt);
			}
		}
	}

	public class Person
	{
		public string Spec { get; set; }
		public string Name { get; set; }
		public Guid PersonId { get; set; }
	}

	public class PersonExt
	{
		public string SpecV { get; set; }
		public string SpecC { get; set; }
		public string Name { get; set; }
		public Guid PersonId { get; set; }
	}
}
