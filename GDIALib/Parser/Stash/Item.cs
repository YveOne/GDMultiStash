using System;
using System.Collections.Generic;

namespace GDIALib.Parser.Stash
{
    [Serializable]
    public class Item
    {

        private static readonly Dictionary<uint, uint> uint2pos = new Dictionary<uint, uint>() {
            {0, 0},
            {1065353216, 1},
            {1073741824, 2},
            {1077936128, 3},
            {1082130432, 4},
            {1084227584, 5},
            {1086324736, 6},
            {1088421888, 7},
            {1090519040, 8},
            {1091567616, 9},
            {1092616192, 10},
            {1093664768, 11},
            {1094713344, 12},
            {1095761920, 13},
            {1096810496, 14},
            {1097859072, 15},
            {1098907648, 16},
            {1099431936, 17},
        };

        private static readonly Dictionary<uint, uint> pos2uint = new Dictionary<uint, uint>() {
            {0, 0},
            {1, 1065353216},
            {2, 1073741824},
            {3, 1077936128},
            {4, 1082130432},
            {5, 1084227584},
            {6, 1086324736},
            {7, 1088421888},
            {8, 1090519040},
            {9, 1091567616},
            {10, 1092616192},
            {11, 1093664768},
            {12, 1094713344},
            {13, 1095761920},
            {14, 1096810496},
            {15, 1097859072},
            {16, 1098907648},
            {17, 1099431936},
        };

        public override string ToString()
        {
            return $"Item[{BaseRecord},{PrefixRecord},{SuffixRecord},{ModifierRecord},{TransmuteRecord},{MateriaRecord},{Seed},{RelicCompletionBonusRecord},{RelicSeed},{EnchantmentRecord},{EnchantmentSeed},{MateriaCombines},{StackCount}]";
        }

        private static readonly Random Random = new Random();

        public string BaseRecord = "";

        public string PrefixRecord = "";

        public string SuffixRecord = "";

        public string ModifierRecord = "";

        public string TransmuteRecord = "";

        public uint Seed = 0u;

        public string MateriaRecord = "";

        public string RelicCompletionBonusRecord = "";

        public uint RelicSeed = 0u;

        public string EnchantmentRecord = "";

        public uint UNKNOWN_uint = 0u;
        public byte UNKNOWN_byte = 0;

        public uint EnchantmentSeed = 0u;

        public uint MateriaCombines = 0u;

        public uint StackCount = 1u;

        private uint _x = 0;
        private uint _y = 0;

        public uint X {
            get => uint2pos[_x];
            set => _x = pos2uint[value];
        }

        public uint Y
        {
            get => uint2pos[_y];
            set => _y = pos2uint[value];
        }

        public Item()
        {
            this.RandomizeSeed();
            this.RandomizeRelicSeed();
        }

        public uint RandomizeSeed()
        {
            return this.Seed = (uint)Item.Random.Next();
        }

        public uint RandomizeRelicSeed()
        {
            return this.RelicSeed = (uint)Item.Random.Next();
        }

        public bool Read(GDCryptoDataBuffer pCrypto)
        {
            bool flag = !pCrypto.ReadCryptoString(out this.BaseRecord) || !pCrypto.ReadCryptoString(out this.PrefixRecord)
                || !pCrypto.ReadCryptoString(out this.SuffixRecord) || !pCrypto.ReadCryptoString(out this.ModifierRecord)
                || !pCrypto.ReadCryptoString(out this.TransmuteRecord) || !pCrypto.ReadCryptoUInt(out this.Seed)
                || !pCrypto.ReadCryptoString(out this.MateriaRecord) || !pCrypto.ReadCryptoString(out this.RelicCompletionBonusRecord)
                || !pCrypto.ReadCryptoUInt(out this.RelicSeed) || !pCrypto.ReadCryptoString(out this.EnchantmentRecord)
                || !pCrypto.ReadCryptoUInt(out this.UNKNOWN_uint) || !pCrypto.ReadCryptoUInt(out this.EnchantmentSeed)
                || !pCrypto.ReadCryptoUInt(out this.MateriaCombines) || !pCrypto.ReadCryptoUInt(out this.StackCount);

            flag = flag || !pCrypto.ReadCryptoUInt(out _x) || !pCrypto.ReadCryptoUInt(out _y);

            return !flag;
        }

        public void Write(DataBuffer pBuffer)
        {
            pBuffer.WriteString(this.BaseRecord);
            pBuffer.WriteString(this.PrefixRecord);
            pBuffer.WriteString(this.SuffixRecord);
            pBuffer.WriteString(this.ModifierRecord);
            pBuffer.WriteString(this.TransmuteRecord);
            pBuffer.WriteUInt(this.Seed);
            pBuffer.WriteString(this.MateriaRecord);
            pBuffer.WriteString(this.RelicCompletionBonusRecord);
            pBuffer.WriteUInt(this.RelicSeed);
            pBuffer.WriteString(this.EnchantmentRecord);
            pBuffer.WriteUInt(this.UNKNOWN_uint);
            pBuffer.WriteUInt(this.EnchantmentSeed);
            pBuffer.WriteUInt(this.MateriaCombines);
            pBuffer.WriteUInt(this.StackCount);
            pBuffer.WriteUInt(this._x);
            pBuffer.WriteUInt(this._y);
        }

    }
}
