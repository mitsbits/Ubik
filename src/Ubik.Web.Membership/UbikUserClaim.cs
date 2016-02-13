// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Ubik.Web.SSO
{

    public class UbikUserClaim: IdentityUserClaim<int> {
        public UbikUserClaim():base()
        {
        }

        public UbikUserClaim(string type, string value) : base(type, value)
        {
        }

        public virtual UbikUser User { get; set; }
    }
    /// <summary>
    /// Represents a claim that a user possesses.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for this user that possesses this claim.</typeparam>
    public class IdentityUserClaim<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityUserClaim()
        {
            Id = default(int);
            UserId = default(TKey);
        }

        public IdentityUserClaim(string type, string value) : this()
        {
            ClaimType = type;
            ClaimValue = value;
        }

        /// <summary>
        /// Gets or sets the identifier for this user claim.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the of the primary key of the user associated with this claim.
        /// </summary>
        public virtual TKey UserId { get; set; }

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