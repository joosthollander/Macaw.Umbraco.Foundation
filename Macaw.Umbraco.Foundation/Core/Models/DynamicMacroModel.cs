﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.cms.businesslogic.macro;
using Umbraco.Core.Dynamics;

namespace Macaw.Umbraco.Foundation.Core.Models
{
	public class DynamicMacroModel : DynamicObject, INullModel
    {
        protected IEnumerable<MacroPropertyModel> Source;
        protected ISiteRepository Repository;

        public DynamicMacroModel(IEnumerable<MacroPropertyModel> source, ISiteRepository repository)
        {
            Source = source;
            Repository = repository;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var property = Source.FirstOrDefault(s => s.Key.Equals(binder.Name));

			if (property != null)
			{
				switch (property.Type)
				{
					case "contentPicker":
						result = Repository.FindById(int.Parse(property.Value));
						break;
					case "mediaCurrent":
						result = Repository.FindMediaById(int.Parse(property.Value));
						break;
					default:
						result = property.Value;
						break;
				}
			}
			else
				result = new DynamicNull();

            return true;
        }

		public bool IsNull()
		{
			return false;
		}
	}
}
