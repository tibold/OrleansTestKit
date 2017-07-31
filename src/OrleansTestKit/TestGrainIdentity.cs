using System;
using System.Diagnostics;
using Orleans.Core;

namespace Orleans.TestKit
{
    [DebuggerStepThrough]
    public sealed class TestGrainIdentity : IGrainIdentity
    {
        private enum KeyType
        {
            String,
            Guid,
            Long,
            GuidCompound
        }

        private readonly KeyType _keyType;

        private Guid primaryKey;
        private long primaryKeyLong;
        private string primaryKeyString;
        private string primaryKeyExtension;

        public Guid PrimaryKey
            => _keyType == KeyType.Guid
            ? primaryKey
            : throw new InvalidOperationException("This property cannot be used if the grain uses the primary key extension feature.");

        public long PrimaryKeyLong
            => _keyType == KeyType.Long
            ? primaryKeyLong
            : throw new InvalidOperationException("This property cannot be used if the grain uses the primary key extension feature.");

        public string PrimaryKeyString
            => _keyType == KeyType.String
            ? primaryKeyString
            : throw new InvalidOperationException("This property cannot be used if the grain uses the primary key extension feature.");
        
        public string IdentityString
        {
            get
            {
                switch (_keyType)
                {
                    case KeyType.String:
                        return PrimaryKeyString;
                    case KeyType.Guid:
                        return PrimaryKey.ToString();
                    case KeyType.GuidCompound:
                        return $"{primaryKey}@{primaryKeyExtension}";
                    case KeyType.Long:
                        return PrimaryKeyLong.ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool IsClient { get { throw new NotImplementedException(); } }

        public int TypeCode { get { throw new NotImplementedException(); } }

        public TestGrainIdentity(Guid id)
        {
            primaryKey = id;
            _keyType = KeyType.Guid;
        }

        public TestGrainIdentity(Guid id, string extension)
        {
            primaryKey = id;
            primaryKeyExtension = extension;
            _keyType = KeyType.GuidCompound;
        }

        public TestGrainIdentity(long id)
        {
            primaryKeyLong = id;
            _keyType = KeyType.Long;
        }

        public TestGrainIdentity(string id)
        {
            primaryKeyString = id;
            _keyType = KeyType.String;
        }

        public long GetPrimaryKeyLong(out string keyExt)
        {
            throw new NotImplementedException();
        }

        public Guid GetPrimaryKey(out string keyExt)
        {
            if(_keyType != KeyType.GuidCompound)
            {
                // FIXME: No idea what errors should we throw here.
                throw new InvalidOperationException();
            }

            keyExt = primaryKeyExtension;
            return primaryKey;
        }

        public uint GetUniformHashCode() 
        {
            throw new NotImplementedException();
        }
    }
}