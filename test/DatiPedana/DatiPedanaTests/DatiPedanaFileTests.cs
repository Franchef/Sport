using DataPedanaHandler;
using System;
using System.Threading;
using Xunit.Abstractions;

namespace DatiPedanaTests
{
    public class DatiPedanaFileTests
    {
        private readonly ITestOutputHelper testOutput;

        public DatiPedanaFileTests(ITestOutputHelper testOutput)
        {
            this.testOutput=testOutput;
        }
        [Theory]
        [InlineData(@"C:\Users\frank\Downloads\Cappa 1.txt")]
        public void ReadPlatformA(string filePath)
        {
            /// Arrange
            IDatiPedanaFileHandler sut = new DatiPedanaFileHandler();

            /// Act
            var items = sut.ReadPlatformA(filePath).ToArray();

            /// Assert
            Assert.Equal(5887, items.Count());
            foreach (var item in items)
                testOutput.WriteLine("{0}", item);
        }

        [Theory]
        [InlineData(@"C:\Users\frank\Downloads\Cappa 1.txt")]
        public void ReadPlatformB(string filePath)
        {
            /// Arrange
            IDatiPedanaFileHandler sut = new DatiPedanaFileHandler();

            /// Act
            var items = sut.ReadPlatformB(filePath).ToArray();

            /// Assert
            Assert.Equal(5887, items.Count());
            foreach (var item in items)
                testOutput.WriteLine("{0}", item);
        }

        [Theory]
        [InlineData(@"C:\Users\frank\Downloads\Cappa 1.txt")]
        public void ReadFileAndDecimateTOSecondCent(string filePath)
        {
            /// Arrange
            IDatiPedanaFileHandler datiPedanaFileHandler = new DatiPedanaFileHandler();
            var items = datiPedanaFileHandler.ReadPlatformA(filePath);
            var sut = new DatiPedanaDecimator();

            /// Act
            var decimati = sut.ReduceTimePrecision(items, DatiPedanaDecimator.Riduzione.Centesimo).ToArray();

            /// Assert
            foreach ( var item in decimati) 
                testOutput.WriteLine("{0}", item);
        }

        [Theory]
        [InlineData(@"C:\Users\frank\Downloads\Cappa 1.txt")]
        public void ReadFileAndDecimateToSecondDec(string filePath)
        {
            /// Arrange
            IDatiPedanaFileHandler datiPedanaFileHandler = new DatiPedanaFileHandler();
            var items = datiPedanaFileHandler.ReadPlatformA(filePath);
            var sut = new DatiPedanaDecimator();

            /// Act
            var decimati = sut.ReduceTimePrecision(items, DatiPedanaDecimator.Riduzione.Decimo).ToArray();

            /// Assert
            foreach (var item in decimati)
                testOutput.WriteLine("{0}", item);
        }

        [Theory]
        [InlineData(@"C:\Users\frank\Downloads\Cappa 1.txt")]
        public async Task ReadFileRows(string filePath)
        {
            await File
                .ReadLinesAsync(filePath, CancellationToken.None)
                .Skip(25)
                .ForEachAsync(r => testOutput.WriteLine(r));
        }
    }
}
