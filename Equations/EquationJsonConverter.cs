// <copyright file="EquationJsonConverter.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Equations
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class EquationJsonConverter : JsonConverter<Equation>
    {
        public override Equation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => new Equation(reader.GetString());

        public override void Write(Utf8JsonWriter writer, Equation value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}
