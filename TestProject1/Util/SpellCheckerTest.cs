using NSem3PT34.Classes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Util
{
    public class SpellCheckerTest
    {
        private void ResetSingleton()
        {
            var field = typeof(SpellChecker)
                .GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);

            field.SetValue(null, null);
        }

        [Fact]
        public void GetInstance_MultipleCalls_ReturnSameInstance()
        {
            ResetSingleton();

            var sc1 = SpellChecker.GetInstance();
            var sc2 = SpellChecker.GetInstance();

            Assert.NotNull(sc1);
            Assert.Same(sc1, sc2);
        }

        [Fact]
        public void LoadDictionary_ValidFile_LoadsWordsCorrectly()
        {
            ResetSingleton();
            var checker = SpellChecker.GetInstance();

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(
                tempFile,
                "hello\nworld\nspell\nchecker\n",
                Encoding.UTF8);

            checker.LoadDictionary(tempFile);

            Assert.False(checker.IsMisspelled("hello"));
            Assert.False(checker.IsMisspelled("world"));
            Assert.False(checker.IsMisspelled("spell"));
            Assert.False(checker.IsMisspelled("checker"));

            Assert.True(checker.IsMisspelled("unknown"));

            File.Delete(tempFile);
        }

        [Fact]
        public void LoadDictionary_IgnoresEmptyLinesAndWhitespace()
        {
            ResetSingleton();
            var checker = SpellChecker.GetInstance();

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(
                tempFile,
                " \n\n test \n\nword\n ",
                Encoding.UTF8);

            checker.LoadDictionary(tempFile);

            Assert.False(checker.IsMisspelled("test"));
            Assert.False(checker.IsMisspelled("word"));
            Assert.True(checker.IsMisspelled(""));

            File.Delete(tempFile);
        }

        [Fact]
        public void LoadDictionary_InvalidPath_DoesNotThrow()
        {
            ResetSingleton();
            var checker = SpellChecker.GetInstance();

            var exception = Record.Exception(() =>
                checker.LoadDictionary("Z:/this/path/does/not/exist.txt"));

            Assert.Null(exception);
        }

        [Fact]
        public void IsMisspelled_EmptyOrNullWord_ReturnsTrue()
        {
            ResetSingleton();
            var checker = SpellChecker.GetInstance();

            Assert.True(checker.IsMisspelled(null));
            Assert.True(checker.IsMisspelled(""));
        }

        [Fact]
        public void IsMisspelled_KnownAndUnknownWords_ReturnCorrectValues()
        {
            ResetSingleton();
            var checker = SpellChecker.GetInstance();

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "apple\nbanana\n", Encoding.UTF8);

            checker.LoadDictionary(tempFile);

            Assert.False(checker.IsMisspelled("apple"));
            Assert.False(checker.IsMisspelled("banana"));
            Assert.True(checker.IsMisspelled("orange"));

            File.Delete(tempFile);
        }
    }
}
