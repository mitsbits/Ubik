// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Ubik.Web.SSO
{

    public class UbikRoleClaim : IdentityRoleClaim<int>
    {
        public UbikRoleClaim():base()
        {

        }

        public UbikRoleClaim(string type, string value) : base(type, value)
        {

        }

        public virtual UbikRole Role { get; set; }
    }

    /// <summary>
    /// Represents a claim that is granted to all users within a role.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the role associated with this claim.</typeparam>
    public class IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityRoleClaim()
        {
            Id = default(int);
            RoleId = default(TKey);
        }

        public IdentityRoleClaim(string type, string value) : this()
        {
            ClaimType = type;
            ClaimValue = value;
        }

        /// <summary>
        /// Gets or sets the identifier for this role claim.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the of the primary key of the role associated with this claim.
        /// </summary>
        public virtual TKey RoleId { get; set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}