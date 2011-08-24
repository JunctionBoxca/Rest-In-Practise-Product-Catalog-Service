using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using ProductCatalog.Tests.Writer.Utility;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Persistence
{
    [TestFixture]
    public class FileNameTests
    {
        [Test]
        public void ShouldAddAtomExtensionToValueWithoutExtension()
        {
            FileName fileName = new FileName("abc");
            Assert.AreEqual("abc.atom", fileName.GetValue());
        }

        [Test]
        public void ShouldPreserveExistingAtomExtension()
        {
            FileName fileName = new FileName("abc.atom");
            Assert.AreEqual("abc.atom", fileName.GetValue());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentException), ExpectedMessage = "Invalid extension: [.txt]. Filename only supports .atom extension.")]
        public void ShouldThrowExceptionIfSuppliedValueHasDifferentExtension()
        {
            new FileName("abc.txt");
        }

        [Test]
        public void ShouldBeAbleToCreateTempFileName()
        {
            FileName fileName = FileName.TempFileName();
            Assert.IsTrue(new Regex(@"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}.atom").IsMatch(fileName.GetValue()));
        }

        [Test]
        public void ShouldExhibitValueTypeEquality()
        {
            FileName fileName1 = new FileName("A");
            FileName fileName2 = new FileName("A");
            FileName fileName3 = new FileName("B");

            Assert.True(fileName1.Equals(fileName2));
            Assert.False(fileName1.Equals(fileName3));
            Assert.True(fileName1.Equals(fileName1));
            Assert.False(fileName1.Equals(new object()));
            Assert.False(fileName1.Equals(null));

            Assert.True(fileName1.GetHashCode().Equals(fileName2.GetHashCode()));
            Assert.False(fileName1.GetHashCode().Equals(fileName3.GetHashCode()));
            Assert.True(fileName1.GetHashCode().Equals(fileName1.GetHashCode()));
            Assert.False(fileName1.GetHashCode().Equals(new object().GetHashCode()));
        }

        [Test]
        public void ShouldBeAbleToCloneSelf()
        {
            FileName fileName = new FileName("A");
            FileName clone = fileName.Clone();

            Assert.AreEqual(fileName, clone);
            Assert.False(ReferenceEquals(fileName, clone));
        }

        [Test]
        public void ToStringShouldReturnStringValueOfFileName()
        {
            FileName fileName = new FileName("A");
            Assert.AreEqual("A.atom", fileName.ToString());
        }

        [Test]
        public void ShouldCreateXmlWriter()
        {
            FileName fileName = new FileName("temp");
            using (XmlWriter writer = fileName.CreateXmlWriter(@"..\..\Data\", new XmlWriterSettings{CloseOutput = true}))
            {
                Assert.IsNotNull(writer);
            }
            File.Delete(Path.Combine(@"..\..\Data\", fileName.GetValue()));
        }

        [Test]
        public void ShouldCreateXmlReader()
        {
            FileName fileName = new FileName("current.atom");
            using (XmlReader reader = fileName.CreateXmlReader(@"..\..\Data\productcatalog\", new XmlReaderSettings { CloseInput = true }))
            {
                Assert.IsNotNull(reader);
            }
        }
    }
}