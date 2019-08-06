using System.Linq;
using System.Threading.Tasks;
using LCM.Domain.Entities;
using LCM.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LCM.Controllers
{
    [Route("user")]
    [ApiController]
    public class LCMUserController : ControllerBase
    {
        private readonly LCMContext _context;
        private readonly UserManager<LCMUser> _userManager;

        public LCMUserController(UserManager<LCMUser> userManager, LCMContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        [Route("currentuser")]
        public async Task<object> GetUser()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);

            return new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.UserName
            };
        }

        [HttpGet]
        [Authorize]
        [Route("history")]
        public IQueryable<LCMHistory> Histories()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var history = _context.LCMHistory.Where(e => e.UserId == userId);
            return history;
        }

        [HttpPost]
        [Authorize]
        [Route("lcm")]
        public async Task<long> Calculate([FromBody] int[] numbers)
        {
            var nums = string.Join(",", numbers);

            int CalculateGcd(int a, int b)
            {
                while (true)
                {
                    if (a == 0 || b == 0) return 0;

                    if (a == b) return a;

                    if (a > b)
                    {
                        a -= b;
                        continue;
                    }

                    var a1 = a;
                    b -= a1;
                }
            }

            int CalculateLcm(int a, int b)
            {
                return a * b / CalculateGcd(a, b);
            }

            var lcm = numbers[0];
            for (var i = 1; i < numbers.Length; i++)
                lcm = CalculateLcm(numbers[i], lcm);

            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var lcmHistory = new LCMHistory {UserId = userId, Input = nums, Result = lcm.ToString()};
            await _context.LCMHistory.AddAsync(lcmHistory);
            await _context.SaveChangesAsync();

            return lcm;
        }

        [HttpPost]
        [Route("factolcm")]
        public long Calc([FromBody] int[] numbers)
        {
            long lcm = 1;
            var divisor = 2;
            while (true)
            {
                var cnt = 0;
                var divisible = false;
                for (var i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i] == 0) return 0;

                    if (numbers[i] < 0) numbers[i] = numbers[i] * -1;

                    if (numbers[i] == 1) cnt++;

                    if (numbers[i] % divisor != 0) continue;
                    divisible = true;
                    numbers[i] = numbers[i] / divisor;
                }

                if (divisible)
                    lcm *= divisor;
                else
                    divisor++;

                if (cnt == numbers.Length) return lcm;
            }
        }
    }
}