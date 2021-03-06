﻿using Rock.Encryption.XSerializer;
using Rock.Immutable;
using XSerializer;

namespace Rock.Encryption
{
    /// <summary>
    /// Provides a set of static methods used for doing field-level encryption
    /// during a serialization operation.
    /// </summary>
    public static class SerializingCrypto
    {
        private static Semimutable<CryptoEncryptionMechanism> _encryptionMechanism = new Semimutable<CryptoEncryptionMechanism>(GetDefaultCryptoEncryptionMechanism);

        /// <summary>
        /// Gets the current instance of <see cref="CryptoEncryptionMechanism"/>.
        /// </summary>
        /// <remarks>
        /// Each method of the <see cref="SerializingCrypto"/> class ultimately uses the value
        /// of this property and calls one of its methods.
        /// </remarks>
        public static CryptoEncryptionMechanism EncryptionMechanism
        {
            get { return _encryptionMechanism.Value; }
        }

        /// <summary>
        /// Sets the value of the <see cref="EncryptionMechanism"/> property.
        /// </summary>
        /// <param name="cryptoEncryptionMechanism">The new value of the <see cref="EncryptionMechanism"/> property.</param>
        /// <remarks>
        /// Each method of the <see cref="SerializingCrypto"/> class ultimately uses the value
        /// of this property and calls one of its methods.
        /// </remarks>
        public static void SetEncryptionMechanism(CryptoEncryptionMechanism cryptoEncryptionMechanism)
        {
            _encryptionMechanism.SetValue(() => cryptoEncryptionMechanism ?? GetDefaultCryptoEncryptionMechanism());
        }

        /// <summary>
        /// Serializes the specified instance to XML, encrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="instance">The instance to serialize.</param>
        /// <returns>An XML document that represents the instance.</returns>
        public static string ToXml<T>(T instance)
        {
            return ToXml(instance, null);
        }

        /// <summary>
        /// Serializes the specified instance to XML, encrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="instance">The instance to serialize.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An XML document that represents the instance.</returns>
        public static string ToXml<T>(T instance, object keyIdentifier)
        {
            var serializer = new XmlSerializer<T>(x => x
                .WithEncryptionMechanism(EncryptionMechanism)
                .WithEncryptKey(keyIdentifier ?? typeof(T)));
            return serializer.Serialize(instance);
        }

        /// <summary>
        /// Deserializes the specified XML to an object, decrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="xml">The XML to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T FromXml<T>(string xml)
        {
            return FromXml<T>(xml, null);
        }

        /// <summary>
        /// Deserializes the specified XML to an object, decrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="xml">The XML to deserialize.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The deserialized object.</returns>
        public static T FromXml<T>(string xml, object keyIdentifier)
        {
            var serializer = new XmlSerializer<T>(x => x
                .WithEncryptionMechanism(EncryptionMechanism)
                .WithEncryptKey(keyIdentifier ?? typeof(T)));
            return serializer.Deserialize(xml);
        }

        /// <summary>
        /// Serializes the specified instance to JSON, encrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="instance">The instance to serialize.</param>
        /// <returns>A JSON document that represents the instance.</returns>
        public static string ToJson<T>(T instance)
        {
            return ToJson(instance, null);
        }

        /// <summary>
        /// Serializes the specified instance to JSON, encrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="instance">The instance to serialize.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>A JSON document that represents the instance.</returns>
        public static string ToJson<T>(T instance, object keyIdentifier)
        {
            var serializer =
                new JsonSerializer<T>(new JsonSerializerConfiguration
                {
                    EncryptionMechanism = EncryptionMechanism,
                    EncryptKey = keyIdentifier ?? typeof(T)
                });

            return serializer.Serialize(instance);
        }

        /// <summary>
        /// Deserializes the specified JSON to an object, decrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="json">The JSON to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T FromJson<T>(string json)
        {
            return FromJson<T>(json, null);
        }

        /// <summary>
        /// Deserializes the specified JSON to an object, decrypting any properties marked
        /// with the [Encrypt] attribute.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="json">The JSON to deserialize.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The deserialized object.</returns>
        public static T FromJson<T>(string json, object keyIdentifier)
        {
            var serializer =
                new JsonSerializer<T>(new JsonSerializerConfiguration
                {
                    EncryptionMechanism = EncryptionMechanism,
                    EncryptKey = keyIdentifier ?? typeof(T)
                });

            return serializer.Deserialize(json);
        }

        private static CryptoEncryptionMechanism GetDefaultCryptoEncryptionMechanism()
        {
            return new CryptoEncryptionMechanism(Crypto.Current);
        }
    }
}
