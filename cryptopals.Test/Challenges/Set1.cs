﻿using System;
using System.Linq;
using cryptopals.Lib;
using cryptopals.Lib.Crypto.Xor;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Challenges
{
    [TestClass]
    public class Set1
    {
        [TestMethod]
        public void Challenge1()
        {
            var input = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            var expected = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";
            var hexString = new HexString(input);

            var bytes = hexString.Bytes.ToArray();
            var actual = Convert.ToBase64String(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Challenge2()
        {
            var a = new HexString("1c0111001f010100061a024b53535009181c");
            var b = new HexString("686974207468652062756c6c277320657965");
            var expected = "746865206b696420646f6e277420706c6179";

            var xord = XorUtil.Xor(a.Bytes.ToArray(), b.Bytes.ToArray());
            var actual = new HexString(xord).ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Challenge3()
        {
            string input = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            var expectedKey = (byte)88;
            var expectedString = "Cooking MC's like a pound of bacon";
            var hexString = new HexString(input);
            var decryptor = new SingleByteXorCracker(new BigramCalculator());

            var actualKey = decryptor.CrackKey(hexString);
            var decryptedBytes = XorUtil.Xor(hexString.Bytes.ToArray(), new[] { actualKey });
            var actualString = System.Text.Encoding.ASCII.GetString(decryptedBytes);

            Assert.AreEqual(expectedKey, actualKey);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Challenge4()
        {
            var expectedString = "Now that the party is jumping\n";
            var expectedKey = 53;
            var strings = Set1Data.Challenge4Input.Split(new[]
            {
                '\n', '\r'
            }, StringSplitOptions.RemoveEmptyEntries);

            var hexStrings = strings.Select(s => new HexString(s)).ToArray();
            ITextScoreCalculator textScoreCalculator = new BigramCalculator();
            var cracker = new SingleByteXorCracker(textScoreCalculator);
            int index;
            var actualKey = cracker.CrackKey(hexStrings, out index);

            var decrytedBytes = XorUtil.Xor(hexStrings[index].Bytes.ToArray(), new[] { actualKey });
            var actualString = System.Text.Encoding.ASCII.GetString(decrytedBytes);

            Assert.AreEqual(expectedString, actualString);
            Assert.AreEqual(expectedKey, actualKey);
        }
    }
}