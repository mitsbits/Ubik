using System;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.DTO
{

    public abstract class Catalogued : ICatalogued
    {
        public abstract string Key { get; set; }
        public abstract string HumanKey { get; set; }
        public abstract string Value { get; set; }
        public abstract string Hint { get; set; }
        public abstract string Flag { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}