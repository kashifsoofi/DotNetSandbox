using System;
using DocDB.Models;

namespace DocDB.Services;

public interface IQueryParser
{
    Query Parse(string q);
}

