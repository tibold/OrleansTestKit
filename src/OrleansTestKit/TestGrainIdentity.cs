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

        public Guid PrimaryKey { get; }

        public long PrimaryKeyLong { get; }

        public string PrimaryKeyString { get; }

        public string KeyExtension { get; }

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
                        return $"{PrimaryKey.ToString()}@{KeyExtension}";
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
            PrimaryKey = id;
            _keyType = KeyType.Guid;
        }

        public TestGrainIdentity(Guid id, string extension)
        {
            PrimaryKey = id;
            KeyExtension = extension;
            _keyType = KeyType.GuidCompound;
        }

        public TestGrainIdentity(long id)
        {
            PrimaryKeyLong = id;
            _keyType = KeyType.Long;
        }

        public TestGrainIdentity(string id)
        {
            PrimaryKeyString = id;
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

            keyExt = KeyExtension;
            return PrimaryKey;
        }

        public uint GetUniformHashCode() 
        {
            throw new NotImplementedException();
        }
    }
}