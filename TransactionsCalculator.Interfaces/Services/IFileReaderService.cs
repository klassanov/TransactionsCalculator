using System.Collections.Generic;
using TransactionsCalculator.Interfaces.Models;

namespace TransactionsCalculator.Interfaces.Services
{
    public interface IFileReaderService
    {
        IEnumerable<ITransaction> ReadFile(string filePath);
    }
}
