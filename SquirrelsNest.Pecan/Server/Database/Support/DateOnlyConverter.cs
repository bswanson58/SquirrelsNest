﻿using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SquirrelsNest.Pecan.Server.Database.Support {
    /// <summary>
    /// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa.
    /// </summary>
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime> {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public DateOnlyConverter() : base(
            d => d.ToDateTime( TimeOnly.MinValue ),
            d => DateOnly.FromDateTime( d ) ) { }
    }

    /// <summary>
    /// Converts <see cref="DateOnly?" /> to <see cref="DateTime?"/> and vice versa.
    /// </summary>
    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?> {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public NullableDateOnlyConverter() : base(
            d => d == null
                ? null
                : new DateTime?( d.Value.ToDateTime( TimeOnly.MinValue ) ),
            d => d == null
                ? null
                : new DateOnly?( DateOnly.FromDateTime( d.Value ) ) ) { }
    }
}
