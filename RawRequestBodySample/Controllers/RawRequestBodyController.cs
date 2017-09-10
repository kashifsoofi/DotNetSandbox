using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RawRequestBodySample.Controllers
{
	[Route("api/[controller]")]
	public class RawRequestBodyController : Controller
    {
		// GET api/values
		[HttpGet]
		public string Get()
		{
            return "Please post to api/RawRequestBody to test receiving Raw Request Body";
		}

		// POST api/values
		[HttpPost]
        public async Task<ContentResult> Post([FromBody] string body)
		{
			var requestData = body;

			var response = new ContentResult
			{
				Content = requestData,
				StatusCode = 200
			};
			return await Task.FromResult(response);
		}
	}
}
