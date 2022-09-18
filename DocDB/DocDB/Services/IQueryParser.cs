using System;
using DocDB.Models;

namespace DocDB.Services;

public interface IQueryParser
{
    Query Parse(bool skipIndex, string q);
}

