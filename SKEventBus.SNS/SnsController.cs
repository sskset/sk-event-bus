using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKEventBus.SNS
{
  [ApiController]
  [Route("api/sns")]
  public class SnsController : ControllerBase
  {

    [HttpPost("{subject}")]
    public IActionResult ReceiveMessage(string subject, [FromBody]string payload)
    {

      return Ok(payload);
    }
  }
}
