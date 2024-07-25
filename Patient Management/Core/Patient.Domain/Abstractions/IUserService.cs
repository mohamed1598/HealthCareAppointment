using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.Abstractions;

public interface IUserService
{
    Task<Result<string>> GetUserId(string userId);
}
