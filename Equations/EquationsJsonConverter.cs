// <copyright file="EquationsJsonConverter.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Equations
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class EquationsJsonConverter : JsonConverter<Equations>
    {
        public override Equations Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException($"Expected StartArray token.");
            }

            var equations = new Equations();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                Equation equation;

                try
                {
                    equation = new Equation(reader.GetString());
                }
                catch (ArgumentException e)
                {
                    throw new JsonException($"Could not create new Equation. {e.Message}", e);
                }

                if (!equations.TryAdd(equation))
                {
                    throw new JsonException($"Failed to add {equation} to equations.");
                }
            }

            return equations;
        }

        public override void Write(Utf8JsonWriter writer, Equations value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var equation in value)
            {
                writer.WriteStringValue(equation.ToString());
            }

            writer.WriteEndArray();
        }
    }
}
