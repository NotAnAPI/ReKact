using System;
using System.Linq;
using NotAnAPI.ReKact.Core;
using NotAnAPI.ReKact.Core.Enums;
using NUnit.Framework;

namespace NotAnAPI.ReKact.Tests
{
    public class KactTests
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public void Kact_WithValid_Kact_String([Values("0,1,1170,20,0,0,220", "0,1,1170,20,0,0,220;1,1,1860,-2,0,0,356;")] string kactString)
        {
            Kact kact = new Kact(kactString);
            Assert.AreEqual(true, kact.IsValid);
        }

        [Test]
        public void Kact_WithValid_Kact_String_With_Whitespaces()
        {
            string kactString = " 	  \t0,1,1170,20,0,0,220\r\n \r \n \n \r ";
            Kact kact = new Kact(kactString);
            Assert.AreEqual(true, kact.IsValid);
        }

        [Test]
        public void Kact_WithInvalid_Kact_String([Values("0,1", "99,2;1,1,18;", "99", "NotAnAPI", "NotAnAPI")] string kactString)
        {
            Kact kact = new Kact(kactString);
            Assert.AreEqual(false, kact.IsValid);
        }

        [TestCase("0,1,1170,20,0,0,220", 0, ActTypes.KeyDown, 1170, 20, 0, 220, -1)]
        [TestCase("1,1,1860,-2,0,0,356;", 1, ActTypes.KeyDown, 1860, -2, 0, 356, -1)]
        public void Kact_Parse(string kactString, int expectedIndex, ActTypes expectedActType, int expectedTime, int expectedKeyCode, int expectedModifier, int expectedFormElement, int expectedNotTrusted)
        {
            Kact kact = new Kact(kactString);
            KeyAct expected = new KeyAct
            {
                Index = expectedIndex, Type = expectedActType, Time = expectedTime, KeyCode = expectedKeyCode,
                Modifier = expectedModifier, FormElement = expectedFormElement, NotTrusted = expectedNotTrusted
            };
            Assert.AreEqual(expected, kact.Keys[0]);
        }

        [TestCase("0,1,1170,20,0,0,220", ExpectedResult = 0)]
        [TestCase("0,1,1170,20,0,0,220;1,1,1860,-2,0,0,356;", ExpectedResult = 0)]
        [TestCase("0,1,1170,20,0,0,220;1,1,1860,-2,0,0,356;2,1,1900,13,0,0,333", ExpectedResult = 1)]
        public int Kact_Stats_Submissions(string kactString)
        {
            Kact kact = new Kact(kactString);
            return kact.Submissions;
        }

        [TestCase("1,1,1860,-2,0,8,356;", true, false, false, false)]
        [TestCase("1,1,1860,-2,0,10,356;", true, false, true, false)]
        [TestCase("1,1,1860,-2,0,11,356;", true, false, true, true)]
        [TestCase("1,1,1860,-2,0,4,356;", false, true, false, false)]
        [TestCase("1,1,1860,-2,0,6,356;", false, true, true, false)]
        [TestCase("1,1,1860,-2,0,5,356;", false, true, false, true)]
        [TestCase("1,1,1860,-2,0,1,356;", false, false, false, true)]
        public void KeyAct_CalculatedModifiers(string kactString, bool shift, bool ctrl, bool meta, bool alt)
        {
            Kact kact = new Kact(kactString);
            Assert.AreEqual(shift, kact.Keys[0].Shift);
            Assert.AreEqual(ctrl, kact.Keys[0].Ctrl);
            Assert.AreEqual(meta, kact.Keys[0].Meta);
            Assert.AreEqual(alt, kact.Keys[0].Alt);
        }
    }
}